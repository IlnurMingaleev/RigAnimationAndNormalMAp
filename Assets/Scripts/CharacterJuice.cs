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
    [SerializeField] private Vector3 jumpStretchSettings;
    [SerializeField] private Vector3 landSquashSettings;
    [SerializeField] private float landSquashMultiplyer;
    [SerializeField] private float jumpStretchMultiplyer;
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
    private bool jumpStretching;
    private bool landSquashing;
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
    void Update()
    {
        //TiltCharacter();
        ControlMovementParticle();
        //We need to change the character's running animation to suit their current speed
        runningSpeed = Mathf.Clamp(Mathf.Abs(playerController.Velocity.x), 0, maxSpeed);
        

        CheckForLanding();

    }

    /*private void TiltCharacter() 
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
        if (!playerGrounded && characterJump.isPlayerOnGround)
        {
            playerGrounded = true;
            //animator.SetFloat("directionX", playerController.movementX);
            animator.SetTrigger("Land");
            landParticles.Play();

            if(!landSquashing && landSquashMultiplyer  > 1) 
            {
                StartCoroutine(JumpPress(landSquashSettings.x * landSquashMultiplyer, landSquashSettings.y / landSquashMultiplyer, landSquashSettings.z, landDrop, false));
            }
        }
        else if (playerGrounded && !characterJump.isPlayerOnGround) 
        {
            playerGrounded = false;
            moveParticles.Stop();
        }
    }

    public void JumpEffects() 
    {
        //animator.SetFloat("directionX",playerController.movementX);
        animator.ResetTrigger("Land");
        
        animator.SetTrigger("Jump");
        if (!jumpStretching && jumpStretchMultiplyer > 1) 
        {
            StartCoroutine(JumpPress(jumpStretchSettings.x / jumpStretchMultiplyer, jumpStretchSettings.y * jumpStretchMultiplyer, jumpStretchSettings.z, 0, true));
        }
        jumpParticles.Play();
    }

    IEnumerator JumpPress(float xPress, float yPress, float seconds, float dropAmount, bool jumpPress) 
    {
        if (jumpPress) { jumpStretching = true; }
        else { landSquashing = true; }
        pressing = true;

        Vector3 originalSize = transform.localScale;
        if (playerController.movementX < 0) xPress *= (-1);
        Vector3 targetSize = new Vector3(xPress, yPress, originalSize.z);

        //Vector3 originalPosition = gameObject.transform.position;
        //Vector3 targetPosition = new Vector3(originalPosition.x,originalPosition.y - dropAmount, originalPosition.z);
        float t = 0f;
        while (t <= 1.0) 
        {
            t += Time.deltaTime / 0.01f;
            gameObject.transform.localScale = Vector3.Lerp(originalSize,targetSize,t);
            //gameObject.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, t);
            yield return null;

        }

        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            gameObject.transform.localScale = Vector3.Lerp(targetSize, originalSize, t);
            //gameObject.transform.localPosition = Vector3.Lerp(targetPosition, originalPosition, t);
            yield return null;

        }
        if (jumpPress) { jumpStretching = false; }
        else { landSquashing = false; }



    }
    public void OnSliderValueChanged(GameObject sliderGameObject)
    {
        Slider tempSlider = sliderGameObject.GetComponent<Slider>();
        float value = tempSlider.value;
        switch (sliderGameObject.name)
        {
            case "Jump Slider":
                jumpStretchMultiplyer = value;
                break;
            case "Land Slider":
                landSquashMultiplyer = value;
                break;
            case "Angle Slider":
                maxTilt = value;
                break;
            case "Tilt Speed Slider":
                tiltSpeed = value;
                break;

        }

    }
    public void ControlMovementParticle() 
    {
        if (Mathf.Approximately(playerController.velocity.x, 0f))
        {
            moveParticles.Stop();
        }
        else 
        {
            moveParticles.Play();
        }
    }
}
