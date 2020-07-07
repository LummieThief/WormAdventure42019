using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateRope1 : MonoBehaviour
{
	public int numSegments;
	public float spacing;
	public GameObject segment;
	//private SpringJoint spring;
	private void Start()
	{
		//spring = GetComponent<SpringJoint>();
		makeRope(gameObject, 0);
	}

	private GameObject makeRope(GameObject lastRope, int segmentNumber)
	{
		GameObject thisRope = Instantiate(segment, transform.position + Vector3.down * spacing * 2 * (numSegments - segmentNumber), Quaternion.identity);
		var joint = thisRope.GetComponent<HingeJoint>();

		SpringJoint spring;
		spring = thisRope.GetComponent<SpringJoint>();
		spring.connectedBody = GetComponent<Rigidbody>();
		spring.maxDistance = (numSegments - segmentNumber) * spacing * 2;

		if (segmentNumber == numSegments)
		{
			joint.connectedBody = gameObject.GetComponent<Rigidbody>();
			thisRope.GetComponent<MeshRenderer>().enabled = false;
		}
		else
		{
			if (segmentNumber == 0)
			{
				thisRope.GetComponent<Rigidbody>().mass = 50;
			}
			joint.connectedBody = makeRope(thisRope, segmentNumber + 1).GetComponent<Rigidbody>();
		}
		return thisRope;
	}
}