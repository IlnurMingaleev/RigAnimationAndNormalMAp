using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJuice : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterJump characterJump;
    private Animator animator;

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

    [Header("Calcultions")]
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
    }

    // Update is called once per frame
    void Update()
    {
        TiltCharacter();

        //We need to change the character's running animation to suit their current speed
        runningSpeed = Mathf.Clamp(Mathf.Abs(playerController.Velocity.x), 0, maxSpeed);
        //animator.SetFloat("runSpeed", runningSpeed);

        CheckForLanding();

    }

    private void TiltCharacter() 
    {
        float tiltDirection = 0;
        if (playerController.Velocity.x != 0) 
        {
            tiltDirection = Mathf.Sign(playerController.Velocity.x);
        }
        Vector3 tiltVector = new Vector3(0, 0, Mathf.Lerp(-maxTilt, maxTilt, Mathf.InverseLerp(-1, 1, tiltDirection)));
        animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, Quaternion.Euler(tiltVector), tiltSpeed * Time.deltaTime);
            
    }
    private void CheckForLanding() 
    {
        if (!playerGrounded && characterJump.IsPlayerOnGround)
        {
            playerGrounded = true;

            if(landPressing && landPressMultiplyer  > 1) 
            {
                StartCoroutine(JumpPress(landPressSettings.x / landPressMultiplyer, landPressSettings.y * landPressMultiplyer, landPressSettings.z, landDrop, false));
            }
        }
        else if (playerGrounded && !characterJump.IsPlayerOnGround) 
        {
            playerGrounded = false;
        }
    }

    private void JumpEffects() 
    {
        if (!jumpPressing && jumpPressMultiplyer > 1) 
        {
            StartCoroutine(JumpPress(jumpPressSettings.x / jumpPressMultiplyer, jumpPressSettings.y * jumpPressMultiplyer, jumpPressSettings.z, 0, true));
        }
    }

    IEnumerator JumpPress(float xPress, float yPress, float seconds, float dropAmount, bool jumpPress) 
    {
        if (jumpPress) { jumpPressing = true; }
        else { landPressing = true; }
        pressing = true;

        Vector3 originalSize = Vector3.one;
        Vector3 targetSize = new Vector3(xPress, yPress, originalSize.z);

        Vector3 originalPosition = Vector3.zero;
        Vector3 targetPosition = new Vector3(0, -dropAmount, 0);
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
}
