using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterJump : MonoBehaviour, IDataSaveLoad
{
    private GroundCheck groundCheck;
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private CharacterJuice characterJuice;

    [SerializeField, Range(0.2f, 1.25f)][Tooltip("Time to reach the maximum height")] private float timeToJumpApex;
    [SerializeField, Range(0f, 5f)][Tooltip("Multiplyer to spcify gravity scale")] private float upwardMovementMultiplyer;
    [SerializeField, Range(1f, 10f)][Tooltip("Multiplyer to spcify gravity scale")] private float downwardMovementMultiplyer;
    [SerializeField, Range(0, 1)][Tooltip("")] private int maxAirJumps;
    [SerializeField][Tooltip("Jump depends on duration of key press")] private bool variableJumpHeight;
    [SerializeField, Range(1f, 10f)][Tooltip("Coefficient for specifying downward gravity scale")] private float jumpCutoff;
    [SerializeField][Tooltip("Downward Speed Limit")] private float speedLimit;
    [SerializeField, Range(0f, 0.3f)]  [Tooltip("Cheats on favor of player")] private float coyoteTime;
    [SerializeField, Range(0f, 0.3f)] [Tooltip("Cheats on favor of player")] private float jumpBuffer;
    [SerializeField, Range(2f, 5.5f)] [Tooltip("Height of jump")] private float jumpHeight;
    [SerializeField]  [Tooltip("Coefficient for regulating jump height")] private float jumpHeightMultiplyer;

    private bool isJumpKeyPressed;
    private bool desiredJump;
    public bool isPlayerOnGround;
    private bool canJumpAgain = false;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool currentlyJumping;
    private Vector2 velocity;
    private float defaultGravityScale;
    private float gravityMultiplier;
    private float jumpSpeed;
    private float scaleOfTime;

    
    // Start is called before the first frame update
    void Awake()
    {
        characterJuice = GetComponent<CharacterJuice>();
        groundCheck = GetComponent<GroundCheck>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        defaultGravityScale = 1f;
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
        if (!currentlyJumping && !isPlayerOnGround)
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
            
        }
        if(context.canceled)
        {
            isJumpKeyPressed = false;
        }
    }

    private void SetPhysics() 
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        
        playerRigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void CalculateGravity() 
    {
        if (playerRigidbody.velocity.y > 0.01f)
        {
            if (isPlayerOnGround)
            {
                gravityMultiplier = defaultGravityScale;
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
                gravityMultiplier = defaultGravityScale;
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
            gravityMultiplier = defaultGravityScale;
        }
        playerRigidbody.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }
    private void DoAJump() 
    {
       
        if (isPlayerOnGround || (coyoteTimeCounter > 0.01f && coyoteTimeCounter < coyoteTime) || canJumpAgain) 
        {
            desiredJump = false;
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;

            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);
            
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * playerRigidbody.gravityScale * jumpHeight);
            

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

            if (characterJuice != null) 
            {
                characterJuice.JumpEffects();
            }
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
                jumpHeight = value * jumpHeightMultiplyer;
                break;
            case "Coyote Time Slider":
                coyoteTime = value;
                break;
            case "JumpBuffer Slider":
                jumpBuffer = value;
                break;
            case "Terminal Velocity Slider":
                speedLimit = value;
                break;
            case "Time Scale Slider":
                Time.timeScale = value;
                scaleOfTime = value;
                break;
            case "Duration Slider":
                timeToJumpApex = value;
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

    public void LoadData(GameData data)
    {
        this.timeToJumpApex = data.timeToJumpApex;
        this.upwardMovementMultiplyer = data.upwardMovementMultiplyer;
        this.downwardMovementMultiplyer = data.downwardMovementMultiplyer;
        this.maxAirJumps = data.maxAirJumps;
        this.variableJumpHeight = data.variableJumpHeight;
        this.jumpCutoff = data.jumpCutoff;
        this.speedLimit = data.speedLimit;
        this.coyoteTime = data.coyoteTime;
        this.jumpBuffer = data.jumpBuffer;
        this.jumpHeight = data.jumpHeight;
        this.jumpHeightMultiplyer = data.jumpHeightMultiplyer;
}

    public void SaveData(GameData data)
    {
        data.timeToJumpApex = this.timeToJumpApex;
        data.upwardMovementMultiplyer = this.upwardMovementMultiplyer;
        data.downwardMovementMultiplyer = this.downwardMovementMultiplyer;
        data.maxAirJumps = this.maxAirJumps;
        data.variableJumpHeight = this.variableJumpHeight;
        data.jumpCutoff = this.jumpCutoff;
        data.speedLimit = this.speedLimit;
        data.coyoteTime = this.coyoteTime;
        data.jumpBuffer = this.jumpBuffer;
        data.jumpHeight = this.jumpHeight;
        data.jumpHeightMultiplyer = this.jumpHeightMultiplyer;
    }
}
