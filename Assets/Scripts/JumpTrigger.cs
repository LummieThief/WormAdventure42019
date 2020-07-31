using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tells the worm whether it is grounded or not.
public class JumpTrigger : MonoBehaviour
{
	private bool grounded;
	private bool solidGround;
	private float baseYScale;
	private float maxScale = 1.2f;
	private float minScale = 0.9f;

	private void Start()
	{
		baseYScale = transform.localScale.y;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Solid" || other.tag == "Solid Excluded")
		{
			grounded = true;
			if (collisionAlongTransform(10))
			{
				solidGround = true;
			}
			//Vector3 collisionPoint = other.ClosestPoint(transform.position)
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Solid" || other.tag == "Solid Excluded")
		{
			grounded = true;
			if (collisionAlongTransform(10))
			{
				solidGround = true;
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Solid" || other.tag == "Solid Excluded")
		{
			grounded = false;
			solidGround = false;
		}
	}

	public bool getGrounded()
	{
		return grounded;
	}

	public bool getSolidGround()
	{
		return solidGround;
	}

	private void Update()
	{
		//Debug.Log(grounded);
		float rot = transform.eulerAngles.x;

		if (transform.eulerAngles.x < 180)
		{
			rot += 360;
		}
		float distanceFrom360 = Mathf.Abs(rot - 360);
		float scaleAmount = minScale + (maxScale - minScale) * (distanceFrom360 / 90);

		transform.localScale = new Vector3(scaleAmount, baseYScale, scaleAmount);

		
		//Debug.Log(scaleAmount);
	}

	private bool collisionAlongTransform(int iterations)
	{
		RaycastHit hit;
		for (int i = 0; i <= iterations; i++)
		{
			Vector3 point = transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y * (-1 + 2 * ((i * 1.0f) / iterations));
			LayerMask mask = 1 << 8; //blocks
			if (Physics.Raycast(point, Vector3.down, out hit, 1, mask))
			{
				//Debug.Log("solid ground");
				return true;
			}
			Debug.DrawRay(point, Vector3.down, Color.red);
		}
		//Debug.Log("Not solid ground");

		return false;
	}
}
