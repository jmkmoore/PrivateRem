using UnityEngine;
using System.Collections;
using Prime31;

public class FireSeed : MonoBehaviour {

    private CharacterController2D _controller;


    public float projectileSpeed = 15;
    private float movementAmt;
    bool fireRight = true;
    public int bulletDamage;
    public float direction;
    public string myType;

    public float timeAlive;
    public float MaxTime;

    public float xAngle;
    public float yAngle;

    public bool isRoller = false;

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
    }


    void onTriggerExitEvent(Collider2D col)
    {
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        timeAlive = 0;
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
    }

    // Update is called once per frame
    void Update()
    {
        movementAmt = projectileSpeed * Time.deltaTime;
        yAngle += -1 * Time.deltaTime;
        timeAlive += Time.deltaTime;
        if (timeAlive > MaxTime)
        {
            DestroyObject(gameObject);
        }
        _controller.move(direction * (new Vector3(xAngle, yAngle)) * movementAmt);
        if (_controller.collisionState.becameGroundedThisFrame && !isRoller)
        {
            projectileSpeed = 0;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            DestroyObject(gameObject);
            GameObject go = other.gameObject.transform.parent.gameObject;
            PlayerHealth ph = (PlayerHealth)go.GetComponent("PlayerHealth");
            ph.adjustCurrentHealth(-bulletDamage);
        }
    }

    void OnColliderEnter2D(Collider2D other)
    {
    }

    public void setDirection(float directionValue)
    {
        direction = directionValue;
    }

    public void setAngle(float radian){
        xAngle = Mathf.Cos(radian);
        yAngle = Mathf.Sin(radian);
    }

    public void setRoller(bool roll)
    {
        isRoller = roll;
    }
}
