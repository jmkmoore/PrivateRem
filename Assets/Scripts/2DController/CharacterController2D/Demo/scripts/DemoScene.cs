﻿using UnityEngine;
using System.Collections;


public class DemoScene : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
    public float dashBoost = 15f;
    private int dashCount = 0;
    private int dashMax = 1;

    public float airDashTime = 0f;
    public float comboTime = 2f;
    public float comboCountdown = 0f;
    public int attackCount = 0;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

    private bool left = false;

    public float ButtonDelay;
    float lastJump = 0;
    float lastUse = 0;
    public bool isDashing = false;
    BoxCollider2D myBoxCollider;

    public int jumpCount= 0;

    public GameObject attackBox;
    public GameObject projectile;

    private float shotCountdown = .5f;
    public float shotTime = 0;

    public PlayerMode pm;

    void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();

		// listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        myBoxCollider = gameObject.GetComponent<BoxCollider2D>();
        pm = gameObject.GetComponent<PlayerMode>();
    }


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.tag + " "  + col.name + " ");
        if (col.tag.Equals("DestructPlat"))
        {
            FallApart obj = (FallApart)col.gameObject.GetComponent<FallApart>();
            obj.triggerTimer();
        }
        if (col.tag.Equals("DeathWall"))
        {
            gameObject.GetComponent<PlayerHealth>().adjustCurrentHealth(-100000);
        }

	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion
	// the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;

        if (isDashing)
        {
            if (Input.GetKey(KeyCode.D))
            {
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                left = false;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                left = true;
            }
            if (airDashTime > 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isDashing = false;
                    airDashTime = 0;
                    normalizedHorizontalSpeed = 0;
                }
            }
                /**
            if (airDashTime > 0.5f)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    normalizedHorizontalSpeed = 0;
                }
            }**/
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                left = false;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                left = true;
            }
            else
            {
                normalizedHorizontalSpeed = 0;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (left)
                    normalizedHorizontalSpeed = -1;
                else
                    normalizedHorizontalSpeed = 1;
                isDashing = true;
                _animator.Play(Animator.StringToHash("Airdash"));
            }
        }
        /**
        if (_controller.isGrounded)
        {
            jumpCount = 0;
            _velocity.y = 0;
        }
        **/
        updateTimers();

        #region combat

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (attackCount == 0 || attackCount == 3)
            {
                _animator.Play(Animator.StringToHash("Jab"));
                attackCount++;
                comboCountdown = 0;
                comboCountdown += Time.deltaTime;
                if (attackCount >= 3)
                {
                    attackCount = 1;
                }
                if (pm.mode.Equals("melee"))
                {
                    attack(20);
                }
                else
                {
                    attack(10);
                }
            }
            else if (attackCount == 1 && comboCountdown < comboTime)
            {
                _animator.Play(Animator.StringToHash("Cross"));
                attackCount++;
                comboCountdown = 0;
                comboCountdown += Time.deltaTime;
                if (pm.mode.Equals("melee"))
                {
                    attack(30);
                }
                else
                {
                    attack(15);
                }
            }
            else if (attackCount == 2 && comboCountdown < comboTime)
            {
                _animator.Play(Animator.StringToHash("Kick"));
                attackCount++;
                comboCountdown = 0;
                comboCountdown += Time.deltaTime;

                if (pm.mode.Equals("melee"))
                {
                    attack(40);
                }
                else
                {
                    attack(20);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (shotTime == 0 || shotTime > 0.4f)
            {
                _animator.Play(Animator.StringToHash("TienRanged"));
                shotTime = 0;
                shotTime += Time.deltaTime;
                spawnProjectile();
            }
        }
        #endregion
        
        #region Movement Animation
        if (_controller.isGrounded && normalizedHorizontalSpeed != 0 && !isDashing)
        {
            _animator.Play(Animator.StringToHash("Run"));
        }
        if (_controller.isGrounded && normalizedHorizontalSpeed == 0 && !isDashing && comboCountdown == 0 && shotTime == 0)
        {
            _animator.Play(Animator.StringToHash("Idle"));
        }
        #endregion

        // we can only jump whilst grounded
        if ((_controller.isGrounded || jumpCount < 2) && Input.GetKeyDown(KeyCode.W))
        {
            if (!_controller.isGrounded)
            {
                jumpCount = 2;
            }
            else
            {
                jumpCount++;
            }
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _animator.StopPlayback();
            _animator.Play(Animator.StringToHash("Jump"));
        }
        /**
        if (jumpCount != 0 && !_controller.isGrounded && _velocity.y < 0 && !isDashing)
        {
            _animator.Play(Animator.StringToHash("TienFall"));
        }
        **/


        // apply horizontal speed smoothing it
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?

        if (!isDashing)
            _velocity.y = 0;
        else
            _velocity.y += gravity * Time.deltaTime;


        if (normalizedHorizontalSpeed != 0  && pm.mode.Equals("speed"))
        {
            normalizedHorizontalSpeed *= pm.speed;
        }

        if (!isDashing)
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
        else{
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * dashBoost, Time.deltaTime * groundDamping);
            _velocity.y = -gravity * Time.deltaTime;
            myBoxCollider.size = new Vector3(10f, 1.75f);
        }
        _controller.move(_velocity * Time.deltaTime);

    }

    public void attack(int attackDamage)
    {
        PlayerAttack attack = (PlayerAttack)attackBox.GetComponent<PlayerAttack>();
        attack.setAttackDamage(attackDamage);
        PlayerAttack attackClone = (PlayerAttack)Instantiate(attack, new Vector3(transform.position.x + (6f * transform.localScale.x), transform.position.y + 5.5f, transform.position.z), transform.rotation);
    }

    public void shoutOut()
    {

    }

    public void spawnProjectile()
    {
        Projectile ball = (Projectile)projectile.GetComponent("Projectile");
        ball.setDirection(transform.localScale.x);
        Projectile ballClone = (Projectile)Instantiate(ball, new Vector3(transform.position.x + (3.5f * transform.localScale.x), transform.position.y + 7f, transform.position.z), transform.rotation);
    }

    public void updateTimers()
    {

        if (isDashing)
        {
            airDashTime += Time.deltaTime;
        }
        if (airDashTime > .5f)
        {
            myBoxCollider.size = new Vector3(1.75f, 10);
            isDashing = false;
            airDashTime = 0;
        }

        if (shotTime != 0)
        {
            shotTime += Time.deltaTime;
        }

        if (shotTime > .3f)
        {
            shotTime = 0;
        }
        
        
        if (comboCountdown > comboTime)
        {
            attackCount = 0;
            comboCountdown = 0;
        }

        if (comboCountdown != 0 && attackCount != 0)
        {
            comboCountdown += Time.deltaTime;
        }

    }

}
