using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Animator animator;
    bool isFacingRight;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        float movementX = Input.GetAxis("Horizontal");

        if (movementX != 0)
        {
            animator.SetBool("Walking", true);
            if (movementX > 0 && !isFacingRight) 
            {
                Flip();
            }
            if (movementX < 0 && isFacingRight) 
            {
                Flip();
            }
            Vector3 movement = new Vector3(movementX, 0, 0);

            movement *= Time.deltaTime * speed;

            transform.Translate(movement);
        }
        else 
        {
            animator.SetBool("Walking", false);
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
