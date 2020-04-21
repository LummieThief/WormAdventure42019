using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFriction : MonoBehaviour
{
	private bool hasFriction;
	private Vector3 initialScale;
	private float minScale = 0.97f;
	private float maxScale = 1.06f;

	private void Start()
	{
		initialScale = transform.localScale;
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Solid")
		{
			hasFriction = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Solid")
		{
			hasFriction = false;
		}
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

		transform.localScale = new Vector3(scaleAmount, initialScale.y, scaleAmount);

		Debug.Log(getFriction());
	}


	public bool getFriction()
	{
		return hasFriction;
	}
}
