using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour {


	public float targetXOffset = 0;
	public float targetYOffset = 0;
	public float targetDistance = 0;

	public float dampTime = 0.15f;
	public bool lockCamera = false;
	public Vector3 lockCoords;

	public bool ShakeCamera = false;
	public float shakeMagnitude = 0;
	public float shakeDuration = 0;
	public bool onlyShakeFirstTime = false;

	private SmoothCamera2D cam;

	// Use this for initialization
	void Start () {
		cam = Camera.main.GetComponent<SmoothCamera2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D other) {
		cam.dampTime = dampTime;
		cam.xOffset = targetXOffset;
		cam.yOffset = targetYOffset;
		cam.distance = targetDistance;
        if (other.tag.Equals("Player"))
        {
            if (lockCamera)
            {
                cam.LockCamera(lockCoords);
            }
            else
            {
                cam.UnlockCamera();
            }
        }

		if (ShakeCamera) {
			cam.Shake (shakeMagnitude, shakeDuration);
			if(onlyShakeFirstTime) ShakeCamera = false;
		}
	}

}
