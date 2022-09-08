using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterJump : MonoBehaviour
{
    private GroundCheck groundCheck;
    private Rigidbody2D playerRigidbody;
    private Animator animator;

    [SerializeField, Range(0.2f, 1.25f)] private float timeToJumpApex;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplyer;
    [SerializeField, Range(1f, 10f)] private float downwardMovementMultiplyer;
    [SerializeField, Range(0, 1)] private int maxAirJumps;
    [SerializeField] private bool variableJumpHeight;
    [SerializeField, Range(1f, 10f)] private float jumpCutoff;
    [SerializeField] private float speedLimit;
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime;
    [SerializeField, Range(0f, 0.3f)] private float jumpBuffer;
    [SerializeField, Range(2f, 5.5f)] private float jumpHeight;

    private bool isJumpKeyPressed;
    private bool desiredJump;
    private bool isPlayerOnGround;
    private bool canJumpAgain = false;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool currentlyJumping;
    private Vector2 velocity;
    private float defaultGravity;
    private float gravityMultiplier;
    private float jumpSpeed;

    public bool IsPlayerOnGround 
    {
        get 
        {
            return isPlayerOnGround;
        }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        groundCheck = GetComponent<GroundCheck>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultGravity = 1f;
        gravityMultiplier = 1f; 
    }

    // Update is called once per frame
    void Update()
    {
        SetPhysics();
        isPlayerOnGround = groundCheck.CheckIsOnGround();

        if (jumpBuffer > 0)
        {
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;
                if (jumpBufferCounter > jumpBuffer)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }

            }

            
        }
        if (!isPlayerOnGround && !currentlyJumping)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }
    }
    private void FixedUpdate()
    {
        velocity = playerRigidbody.velocity;
        if (desiredJump) 
        {
            //Debug.Log("Jump");
            DoAJump();
            playerRigidbody.velocity = velocity;
            return;
        }
        CalculateGravity();
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.started)
        {
            desiredJump = true;
            isJumpKeyPressed = true;
            //Debug.Log("Jump");
        }
        if(context.canceled)
        {
            isJumpKeyPressed = false;
        }
    }

    private void SetPhysics() 
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        //Debug.Log(newGravity);
        //Debug.Log(Physics2D.gravity.y);
        //Debug.Log(gravityMultiplier);
        playerRigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void CalculateGravity() 
    {
        if (playerRigidbody.velocity.y > 0.01f)
        {
            if (isPlayerOnGround)
            {
                gravityMultiplier = defaultGravity;
            }
            else
            {
                if (variableJumpHeight)
                {
                    if (isJumpKeyPressed && currentlyJumping)
                    {
                        gravityMultiplier = upwardMovementMultiplyer;
                    }
                    else
                    {
                        gravityMultiplier = jumpCutoff;
                    }
                }
                else 
                {
                    gravityMultiplier = upwardMovementMultiplyer;
                }
            }
        }
        else if (playerRigidbody.velocity.y < -0.01f)
        {
            if (isPlayerOnGround)
            {
                gravityMultiplier = defaultGravity;
            }
            else
            {
                gravityMultiplier = downwardMovementMultiplyer;
            }
        }
        else 
        {
            if (isPlayerOnGround) 
            {
                currentlyJumping = false;
                
            }
            gravityMultiplier = defaultGravity;
        }
        playerRigidbody.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }
    private void DoAJump() 
    {
        //Debug.Log("Jump");
        //animator.SetTrigger("Jump");
        
        if (isPlayerOnGround || (coyoteTimeCounter > 0.01f && coyoteTimeCounter < coyoteTime) || canJumpAgain) 
        {
            desiredJump = false;
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;

            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);
            //Debug.Log(playerRigidbody.gravityScale);
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * playerRigidbody.gravityScale * jumpHeight);
            //Debug.Log(jumpSpeed);

            if (velocity.y > 0)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f) 
            {
                jumpSpeed += Mathf.Abs(playerRigidbody.velocity.y);
            }
            velocity.y += jumpSpeed;
            currentlyJumping = true;

        }
    }
    public void OnRadialSliderValueChanged(GameObject radialSliderGameObject) 
    {
        AnnularSlider radialSlider = radialSliderGameObject.GetComponent<AnnularSlider>();
        jumpCutoff = radialSlider.Value;
    
    }
    public void OnSliderValueChanged(GameObject sliderGameObject) 
    {
        Slider slider = sliderGameObject.GetComponent<Slider>();
        float value = slider.value;
        switch (sliderGameObject.name) 
        {
            case "Height Slider":
                jumpHeight = value;
                break;
            case "Coyote Time Slider":
                coyoteTime = value;
                break;
            case "JumpBuffer Slider":
                jumpBuffer = value;
                break;
            case "Terminal Velocity Slider":
                jumpHeight = value;
                break;
        }
    
    }

    public void OnBooleanValueChanged(GameObject toggleGameObject) 
    {
        Toggle toggle = toggleGameObject.GetComponent<Toggle>();
        bool isOn = toggle.isOn;
        switch (toggleGameObject.name)
        {
            case "Double Jump":
                maxAirJumps = isOn ? 1 : 0;
                break;
            case "Variable Height":
                variableJumpHeight = isOn;
                break;

        }

    }
  
}
