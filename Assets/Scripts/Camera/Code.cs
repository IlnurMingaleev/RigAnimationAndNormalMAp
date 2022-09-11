using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Code : MonoBehaviour
{

	public float moveSpeedHorizontal;
	public float moveSpeedVertical;

	public float moveSpeedSmoothV = 2f; // The rate of smoothing from start to stop. 
	public float moveSpeedSmoothH = 2f; // Smaller numbers produce a faster change in velocity.

	public float currentVerticalSpeed; //Mathf.smoothDamp names. One for each axis.
	public float currentHorizontalSpeed;


	float horizontalSpeedV; //References for the third statement in Mathf.smoothDamp
	float verticalSpeedV;

	void Start()
	{



	}

	void Update()
	{

		// Mathf.smoothDamp equations, given variable names. 

		currentHorizontalSpeed = Mathf.SmoothDamp(currentHorizontalSpeed, moveSpeedHorizontal, ref horizontalSpeedV, moveSpeedSmoothH);
		currentVerticalSpeed = Mathf.SmoothDamp(currentVerticalSpeed, moveSpeedVertical, ref verticalSpeedV, moveSpeedSmoothV);

		if (moveSpeedVertical >= 6f)
		{  // Maximum and minimum speeds for my van.
			moveSpeedVertical = 6f;
		}
		if (moveSpeedVertical <= -6f)
		{
			moveSpeedVertical = -6f;
		}
		if (moveSpeedHorizontal >= 6f)
		{
			moveSpeedHorizontal = 6f;
		}
		if (moveSpeedHorizontal <= -6f)
		{
			moveSpeedHorizontal = -6f;
		}

		//First these lines check to see that the Vertical/Horizontal speed is not Zero. Then it checks to make sure I'm not pressing any
		//movement keys. As the currrentSpeeds decrease to smaller and smaller numbers, I simply set the value to zero to save on processor power.
		//I can't use decimals here for percentages because Mathf.smoothDamp requires floats, and they don't like decimals. 

		if (currentVerticalSpeed != 0f && currentVerticalSpeed <= currentVerticalSpeed * 10f / 100f && currentVerticalSpeed >= currentVerticalSpeed * -10f / 100f &&
			(!Input.GetKey(KeyCode.W)) && (!Input.GetKey(KeyCode.S)) && (moveSpeedVertical <= 0.05f) && (moveSpeedVertical >= -0.05f))
		{
			currentVerticalSpeed = 0f;
		}
		if (currentHorizontalSpeed != 0f && currentHorizontalSpeed <= currentHorizontalSpeed * 10f / 100f && currentHorizontalSpeed >= currentHorizontalSpeed * -10f / 100f &&
			(!Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)) && (moveSpeedHorizontal <= 0.05f) && (moveSpeedHorizontal >= -0.05f))
		{
			currentHorizontalSpeed = 0f;
		}


		//Here, I've adjusted the time it takes to accelerate and decelerate. If you're pushing any of the movement keys it will adjust the time to a smaller value,
		//providing a fast boost of spped. Once you have released the movement key, the value is increased to 2f, allowing the van to cruise for a time.
		//An interesting trick can be performed here: If you use a single value like "moveSpeedSmooth", the van appears to break before cornering. This could
		//be VERY helpful in a game that had a 2D grid like layout that required quick turns from one axis to the other without drifting.
		//To test this, simply swap out the moveSpeedSmooth values in the individual Mathf.smoothDamp equations above so that they match. 
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
			moveSpeedSmoothV = 0.25F;
		else
			moveSpeedSmoothV = 2f;
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
			moveSpeedSmoothH = 0.25f;
		else
			moveSpeedSmoothH = 2f;

		// Also, spacebar is the brake. :)


		//This should be self explanatory. It covers what happens if you press one direction on an axis or the other, and what happens
		//if you're not pressing any buttons on a particular axis. If you release the W and S keys, for example, instead of abruptly stopping,
		//it reduces your moveSpeed by a percentage, and continues with the transform for a smooth glide effect.
		if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			transform.Translate(Vector2.up * currentVerticalSpeed * Time.deltaTime);
			moveSpeedVertical += 3.0f * Time.deltaTime;
		}
		if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
		{
			transform.Translate(Vector2.up * currentVerticalSpeed * Time.deltaTime);
			moveSpeedVertical -= 3.0f * Time.deltaTime;
		}
		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && currentVerticalSpeed != 0f)
		{
			moveSpeedVertical = (moveSpeedVertical * 90f / 100f) * Time.deltaTime;
			transform.Translate(Vector2.up * currentVerticalSpeed * Time.deltaTime);

		}
		if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector2.left * currentHorizontalSpeed * Time.deltaTime);
			moveSpeedHorizontal += 3.0f * Time.deltaTime;
		}
		if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
		{
			transform.Translate(Vector2.left * currentHorizontalSpeed * Time.deltaTime);
			moveSpeedHorizontal -= 3.0f * Time.deltaTime;
		}
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && currentHorizontalSpeed != 0f)
		{
			moveSpeedHorizontal = (moveSpeedHorizontal * 90f / 100f) * Time.deltaTime;
			transform.Translate(Vector2.left * currentHorizontalSpeed * Time.deltaTime);

		}
	}
}