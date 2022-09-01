using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterJump : MonoBehaviour
{
    private GroundCheck groundCheck;
    private Rigidbody2D playerRigidbody;
    private Animator animator;

    [SerializeField] private float timeToJumpApex;
    [SerializeField] private float upwardMovementMultiplyer;
    [SerializeField] private float downwardMovementMultiplyer;
    [SerializeField] private int maxAirJumps;
    [SerializeField] private bool variableJumpHeight;
    [SerializeField] private float jumpCutoff;
    [SerializeField] private float speedLimit;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float jumpHeight;

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
                        gravityMultiplier = downwardMovementMultiplyer;
                    }
                    else
                    {
                        gravityMultiplier = upwardMovementMultiplyer;
                    }
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
            if (isPlayerOnGround && velocity.y == 0) 
            {
                currentlyJumping = false;
                gravityMultiplier = defaultGravity;
            }
        }
        playerRigidbody.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }
    private void DoAJump() 
    {
        Debug.Log("Jump");
        animator.SetTrigger("Jump");
        
        if (isPlayerOnGround || (coyoteTimeCounter > 0.01f && coyoteTimeCounter < coyoteTime) || canJumpAgain) 
        {
            desiredJump = false;
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;

            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);
            Debug.Log(playerRigidbody.gravityScale);
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * playerRigidbody.gravityScale * jumpHeight);
            Debug.Log(jumpSpeed);

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
  
}
