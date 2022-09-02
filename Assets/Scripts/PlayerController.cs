using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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

    [SerializeField, Range(0f, 10f)][Tooltip("Maximum movement speed")] private float maxSpeed;
    [SerializeField, Range(0f, 50f)][Tooltip("How fast to reach max speed")] private float maxAcceleration;
    [SerializeField, Range(0f, 50f)][Tooltip("How fast to stop after letting go")] private float maxSlowDown;
    [SerializeField, Range(0f, 50f)][Tooltip("How fast to stop when changing direction")] private float maxTurnSpeed;
    [SerializeField, Range(0f, 50f)][Tooltip("How fast to reach max speed when in mid-air")] private float maxAirAcceleration;
    [SerializeField, Range(0f, 50f)][Tooltip("How fast to stop in mid-air when no direction is used")] private float maxAirSlowDown;
    [SerializeField, Range(0f, 50f)][Tooltip("How fast to stop in mid-air when no direction is used")] private float maxAirTurnSpeed;

    private float acceleration;
    private float slowDown;
    private float turnSpeed;
    private bool isMoveInstant;
    public bool isOnGround;
    public float movementX;

    [SerializeField] private Slider accelerationSlider;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider slowDownSlider;

    public Vector2 Velocity 
    {
        get 
        {
            return velocity;    
        }
    }
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


    public void OnMove(InputAction.CallbackContext context)
    {

        movementX = context.ReadValue<Vector2>().x;
   
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
        isOnGround = groundCheck.CheckIsOnGround();

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
    public void OnSliderValueChanged(GameObject slider) 
    {
        Slider tempSlider = slider.GetComponent<Slider>();
        float value = tempSlider.value;
        switch (slider.name) 
        {
            case "AccelerationSlider":
                maxAcceleration = value;
                break;
            case "SpeedSlider":
                maxSpeed = value;
                break;
            case "DeselerationSlider":
                maxSlowDown = value;
                break;


        }
    
    }

}
