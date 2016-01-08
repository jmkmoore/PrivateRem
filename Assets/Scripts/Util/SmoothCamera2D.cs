using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	public float turnaroundDampTime = 0.5f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	public Vector2 Camera_Min;
	public Vector2 Camera_Max;

	public float xOffset = 0;
	public float yOffset = 0;
	public float distance = 62f;

	//private Vector3 point;
	private Vector3 delta;
	private Vector3 destination;
	private float xOffsetCorrected;

	private bool locked = false;
	private Vector3 lockPos;
	//Tracking scale here, so if this isn't equal to the scale of Tien's model then we're changing directions.
	private float currentDirection; 
	private IEnumerator runningCoroutine;
	private float flipPercent = 1;


	void Start(){
		xOffsetCorrected = xOffset;
		if(target != null)currentDirection = target.transform.localScale.x;
	}

	// Update is called once per frame

	void Update () 
	{
		if (locked) {
			//point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			destination = lockPos;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}else if (target)
		{
			//point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance)); //(new Vector3(0.5, 0.5, point.z));
			destination = transform.position + delta;
			destination -= new Vector3(xOffsetCorrected, yOffset, 0);
			CheckBounds();
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

		}else{
			target = GameObject.FindGameObjectWithTag("Player").transform;
			if(target.transform.parent != null) target = target.transform.parent;	
			currentDirection = target.transform.localScale.x;
		}

		if (currentDirection != target.transform.localScale.x) {
			currentDirection = target.transform.localScale.x;
			if(runningCoroutine != null)StopCoroutine(runningCoroutine);
			runningCoroutine = FlipCamera ();
			StartCoroutine(runningCoroutine);
		}


	}

	IEnumerator FlipCamera(){

		float start = xOffset * currentDirection * -1;
		float elapsed = turnaroundDampTime * (1 - flipPercent);
		

		while (elapsed < turnaroundDampTime) {
			
			elapsed += Time.deltaTime;          
			
			flipPercent = elapsed / turnaroundDampTime;         
			xOffsetCorrected = Mathf.Lerp(start,start * -1f,flipPercent);
			
			yield return null;
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

	public void Shake(float mag, float dur){
		StartCoroutine (ShakeCoroutine (mag, dur));
	}

	//Algorithm inspired by http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html
	private IEnumerator ShakeCoroutine(float magnitude, float duration){
		float elapsed = 0.0f;
		
		//Vector3 originalCamPos = Camera.main.transform.position;
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;          
			
			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;
			
			transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
			
			yield return null;
		}
		
		//Camera.main.transform.position = originalCamPos;
	}
}