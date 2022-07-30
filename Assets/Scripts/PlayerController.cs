using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        float movementX = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(movementX, 0, 0);

        movement *= Time.deltaTime * speed;

        transform.Translate(movement);

    }
}
