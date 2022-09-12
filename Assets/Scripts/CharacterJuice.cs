using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterJuice : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterJump characterJump;
    private Animator animator;
    private Rigidbody2D rigidbody;
    [SerializeField] private Transform mainBone;
    
    [Header("Components - Particles")]
    [SerializeField] private ParticleSystem moveParticles;
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem landParticles;



    [Header("Settings - Squash and Stretch")]
    [SerializeField] private Vector3 jumpPressSettings;
    [SerializeField] private Vector3 landPressSettings;
    [SerializeField] private float landPressMultiplyer;
    [SerializeField] private float jumpPressMultiplyer;
    [SerializeField] private float landDrop = 1f ;

    [Header("Tilting")]
    [SerializeField] private bool leanForward;
    [SerializeField] private float maxTilt;
    [SerializeField] private float tiltSpeed;

    [Header("Calculations")]
    private float runningSpeed;
    private float maxSpeed;

    [Header("Current State")]
    private bool pressing;
    private bool jumpPressing;
    private bool landPressing;
    private bool playerGrounded;

    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterJump = GetComponent<CharacterJump>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       //TiltCharacter();

        //We need to change the character's running animation to suit their current speed
        runningSpeed = Mathf.Clamp(Mathf.Abs(playerController.Velocity.x), 0, maxSpeed);
        //animator.SetFloat("runSpeed", runningSpeed);

        CheckForLanding();

    }


    private void TiltCharacter()
    {
        //See which direction the character is currently running towards, and tilt in that direction
        float directionToTilt = 0;
        if (playerController.velocity.x != 0)
        {
            directionToTilt = Mathf.Sign(playerController.velocity.x);
        }

        //Create a vector that the character will tilt towards
        //Vector3 targetRotVector = new Vector3(0, 0, Mathf.Lerp(-maxTilt, maxTilt, Mathf.InverseLerp(-1, 1, directionToTilt)));

        //And then rotate the character in that direction
        //mainBone.transform.rotation = Quaternion.RotateTowards(mainBone.transform.rotation, Quaternion.Euler(-targetRotVector), tiltSpeed * Time.deltaTime);
        //mainBone.localEulerAngles = new Vector3(0,0, 90 - maxTilt);
       
    }
   /* private void TiltCharacter() 
    {
        float tiltDirection = 0;
        if (playerController.Velocity.x != 0) 
        {
            tiltDirection = Mathf.Sign(playerController.Velocity.x);
            
        }
        Vector3 tiltVector = new Vector3(0, 0, Mathf.Lerp(-maxTilt, maxTilt, Mathf.InverseLerp(-1, 1, tiltDirection)));
        animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, Quaternion.Euler(-tiltVector), tiltSpeed * Time.deltaTime);
            
    }*/
    private void CheckForLanding() 
    {
        if (!playerGrounded && characterJump.IsPlayerOnGround)
        {
            playerGrounded = true;
            animator.SetTrigger("Land");
            landParticles.Play();

            //Debug.Log("Before Couroutine");
            moveParticles.Play();
            
            if(!landPressing && landPressMultiplyer  > 1) 
            {
                //Debug.Log("We start couroutine");
                StartCoroutine(JumpPress(landPressSettings.x / landPressMultiplyer, landPressSettings.y * landPressMultiplyer, landPressSettings.z, landDrop, false));
            }
        }
        else if (playerGrounded && !characterJump.IsPlayerOnGround) 
        {
            playerGrounded = false;
            moveParticles.Stop();
        }
    }

    public void JumpEffects() 
    {
        animator.ResetTrigger("Land");
        animator.SetTrigger("Jump");
        if (!jumpPressing && jumpPressMultiplyer > 1) 
        {
            //Debug.Log("Starting couroutine in jump effects");
            StartCoroutine(JumpPress(jumpPressSettings.x / jumpPressMultiplyer, jumpPressSettings.y * jumpPressMultiplyer, jumpPressSettings.z, 0, true));
        }
        jumpParticles.Play();
    }

    IEnumerator JumpPress(float xPress, float yPress, float seconds, float dropAmount, bool jumpPress) 
    {
        if (jumpPress) { jumpPressing = true; }
        else { landPressing = true; }
        pressing = true;

        Vector3 originalSize = Vector3.one * 0.1f;
        Vector3 targetSize = new Vector3(xPress, yPress, originalSize.z);

        Vector3 originalPosition = gameObject.transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x,originalPosition.y - dropAmount, originalPosition.z);
        float t = 0f;
        while (t <= 1.0) 
        {
            t += Time.deltaTime / 0.01f;
            gameObject.transform.localScale = Vector3.Lerp(originalSize,targetSize,t);
            gameObject.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, t);
            yield return null;

        }

        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            gameObject.transform.localScale = Vector3.Lerp(targetSize, originalSize, t);
            gameObject.transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, t);
            yield return null;

        }
        if (jumpPress) { jumpPressing = false; }
        else { landPressing = false; }



    }
    public void OnSliderValueChanged(GameObject sliderGameObject)
    {
        Slider tempSlider = sliderGameObject.GetComponent<Slider>();
        float value = tempSlider.value;
        switch (sliderGameObject.name)
        {
            case "Jump Slider":
                jumpPressMultiplyer = value;
                break;
            case "Land Slider":
                landPressMultiplyer = value;
                break;
            case "Angle Slider":
                maxTilt = value;
                break;
            case "Tilt Speed Slider":
                tiltSpeed = value;
                break;

        }

    }
}
