using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	public Vector2 Camera_Min;
	public Vector2 Camera_Max;

	public float xOffset = 0;
	public float yOffset = 0;
	public float distance = 62f;

	private Vector3 point;
	private Vector3 delta;
	private Vector3 destination;

	private bool locked = false;
	private Vector3 lockPos;
	

	// Update is called once per frame

	void Update () 
	{
		if (locked) {
			point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			destination = lockPos;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}else if (target)
		{
			point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance)); //(new Vector3(0.5, 0.5, point.z));
			destination = transform.position + delta;
			destination -= new Vector3(xOffset, yOffset, 0);
			CheckBounds();
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}else{
			target = GameObject.FindGameObjectWithTag("Player").transform;
		}

	}

	void CheckBounds(){
		if(destination.x > Camera_Max.x) destination.x = Camera_Max.x;
		else if(destination.x < Camera_Min.x) destination.x = Camera_Min.x;
		if(destination.y > Camera_Max.y) destination.y = Camera_Max.y;
		else if(destination.y < Camera_Min.y) destination.y = Camera_Min.y;
	}

	public void LockCamera(){
		lockPos = transform.position;
		locked = true;
	}

	public void LockCamera(Vector3 lockVec){
		lockPos = lockVec;
		locked = true;

	}

	public void LockCamera(Vector2 lockVec){
		lockPos = new Vector3 (lockVec.x, lockVec.y, transform.position.z);
		locked = true;
	}

	public void UnlockCamera(){
		locked = false;
	}
}