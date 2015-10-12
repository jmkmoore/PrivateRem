using UnityEngine;
using System.Collections;
using Prime31;

public class DaddySpiderController : MonoBehaviour {

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }

    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
//        if (col.name.Equals("TienHitBox"))
//            updateAttack(true);
//        if (col.name.Equals("Wall"))
//        {
//            updateDirection();
//        }
    }


    void onTriggerExitEvent(Collider2D col)
    {
/**        if (col.name.Equals("TienHitBox"))
        {
            updateAttack(false);
        }
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
**/
    }

    #endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
