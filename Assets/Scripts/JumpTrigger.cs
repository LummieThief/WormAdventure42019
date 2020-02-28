using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tells the worm whether it is grounded or not.
public class JumpTrigger : MonoBehaviour
{
	private bool grounded;
	private float maxScale = 1.2f;
	private float minScale = 0.9f;

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Solid")
		{
			grounded = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Solid")
		{
			grounded = false;
		}
	}

	public bool getGrounded()
	{
		return grounded;
	}

	private void Update()
	{
		float rot = transform.eulerAngles.x;

		if (transform.eulerAngles.x < 180)
		{
			rot += 360;
		}
		float distanceFrom360 = Mathf.Abs(rot - 360);
		float scaleAmount = minScale + (maxScale - minScale) * (distanceFrom360 / 90);

		transform.localScale = new Vector3(scaleAmount, 1, scaleAmount);

		//Debug.Log(scaleAmount);
	}
}
