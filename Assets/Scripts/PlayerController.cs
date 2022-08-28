using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    
    private PlayerControls playerControls;
    private Animator animator;
    private GroundCheck groundCheck;
    private bool isKeyPressed; 
    private Rigidbody2D playerRigidbody;
    bool isFacingRight;
    private Vector2 velocity;
    private Vector2 targetVelocity;
    private float maxSpeedChange;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float maxSlowDown;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float maxAirAcceleration;
    [SerializeField] private float maxAirSlowDown;
    [SerializeField] private float maxAirTurnSpeed;

    private float acceleration;
    private float slowDown;
    private float turnSpeed;
    private bool isMoveInstant;
    public bool isOnGround;
    public float movementX;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isFacingRight = true;
        groundCheck = GetComponent<GroundCheck>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();   
    }
    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move()
    {

        movementX = playerControls.Land.Move.ReadValue<Vector2>().x;
        //groundCheck.CheckIsOnGround();
        
        if (movementX != 0)
        {
            isKeyPressed = true;
            animator.SetBool("Walking", true);
            if (movementX > 0 && !isFacingRight)
            {
                Flip();
            }
            if (movementX < 0 && isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            isKeyPressed = false;
            animator.SetBool("Walking", false);
        }
        targetVelocity = new Vector2(movementX, 0f) * maxSpeed;
    }

    private void RunWithAcceleration() 
    {
        acceleration = isOnGround ? maxAcceleration : maxAirAcceleration;
        turnSpeed = isOnGround ? maxTurnSpeed : maxAirTurnSpeed;
        slowDown = isOnGround ? maxSlowDown : maxAirSlowDown;

        if (isKeyPressed)
        {
            if (Mathf.Sign(movementX) == Mathf.Sign(velocity.x))
            {
                maxSpeedChange = acceleration * Time.deltaTime;
            }
            else
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
        }
        else 
        { 
            maxSpeedChange = slowDown * Time.deltaTime;
        }

        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocity.x, maxSpeedChange);

        playerRigidbody.velocity = velocity;
    }
    private void RunWithoutAcceleration() 
    {
        velocity.x = targetVelocity.x;
        playerRigidbody.velocity = velocity;

    }
    private void FixedUpdate()
    {
        groundCheck.CheckIsOnGround();

        velocity = playerRigidbody.velocity;

        if (!isMoveInstant)
        {
            RunWithoutAcceleration();
        }
        else 
        {
            if (isOnGround)
            {
                RunWithoutAcceleration();
            }
            else
            {
                RunWithAcceleration();
            }
            
        }
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }

}
