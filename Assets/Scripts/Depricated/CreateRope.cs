using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Doesnt do anything right now but i also dont want to delete it
public class CreateRope : MonoBehaviour
{
	public GameObject generalRope;
	public int segments = 10;
	public float spacing = 0.1f;
	public float length = 10;

	private GameObject prevRope, rope;
	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 storedVelocity = rb.velocity;
		float yScale = (length / segments) / 2 - spacing / 2;
		if (Input.GetKeyDown(KeyCode.R))
		{
			//the bottom of the rope connected to the swinging object
			GameObject prevRope = Instantiate(generalRope, transform.position, transform.rotation);
			prevRope.transform.localScale = new Vector3(0.1f, transform.localScale.y, 0.1f);
			prevRope.GetComponent<Rigidbody>().velocity = storedVelocity;
			gameObject.AddComponent<FixedJoint>().connectedBody = prevRope.GetComponent<Rigidbody>();
			//the middle segments of the rope

			Vector3 buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.localScale.y * 1.05f;
			for (int i = 0; i <= segments; i++)
			{
				rope = Instantiate(generalRope, buttPosition - (i + 0.5f) * (transform.TransformDirection(Vector3.up) * length / segments), transform.rotation);
				rope.transform.localScale = new Vector3(0.1f, yScale, 0.1f);
				prevRope.GetComponent<HingeJoint>().connectedBody = rope.GetComponent<Rigidbody>();
				prevRope.GetComponent<Rigidbody>().velocity = storedVelocity * (segments - i) / segments;
				prevRope.GetComponent<Rigidbody>().AddTorque(getRandomVector(300f, 0f, 0f));
				prevRope = rope;
			}
			rope.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
			Destroy(rope.GetComponent<HingeJoint>());
		}
		if (Input.GetKeyUp(KeyCode.R))
		{
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Rope"))
			{
				Destroy(obj);
			}
			Destroy(gameObject.GetComponent<FixedJoint>());
		}

	}

	private Vector3 getRandomVector(float xRange, float yRange, float zRange)
	{
		return new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), Random.Range(-zRange, zRange));
	}
}