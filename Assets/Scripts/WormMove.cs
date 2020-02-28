using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMove : MonoBehaviour
{
	public float moveSpeed = 1000;
	public float maxAngVel = 4f;
	public float maxSpeed = 100f;
	public float jumpForce = 50f;

	public GameObject generalRope;
	public GameObject ropeDrop;
	
	private Vector3 initialScale;
	private Rigidbody rb;
	private JumpTrigger jumpTrigger;
	private bool grounded;
	private bool crouching;

	//private BonkTrigger bonkTrigger;
	public float bonkThreshold;
	private Vector3 prevVelocity;

	public float range = 50f;
	public float minRopeLength = 0.5f;
	public int numSegments = 10;
	public float ropeSpacing = 0.05f;
	public GameObject grappleHitPrefab;
	private GameObject grappleHit;
	private bool canGrapple;
	private Vector3 buttPosition;
	private bool grappling;

	public GameObject cameraRig;
	public float rotFlipOffset = 30;
	private bool walkForward = true;

	public Material tailOn;
	public Material tailOff;
	public MeshRenderer tail;

	public Animator animator;
	//private HingeJoint hinge;

	// Start is called before the first frame update
	void Start()
    {
		jumpTrigger = GetComponentInChildren<JumpTrigger>();
		//bonkTrigger = GetComponentInChildren<BonkTrigger>();
		rb = GetComponent<Rigidbody>();
		initialScale = transform.localScale;
		grappleHit = Instantiate(grappleHitPrefab, Vector3.zero, Quaternion.identity);
		//hinge = grappleHit.GetComponent<HingeJoint>();

    }

	// Update is called once per frame
	void Update()
	{
		grounded = jumpTrigger.getGrounded();
		if (grounded)
		{
			canGrapple = true;
		}
		rb.maxAngularVelocity = maxAngVel;
		if (rb.velocity.magnitude > maxSpeed)
		{
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}

		//Debug.Log(transform.up);
		Debug.DrawRay(transform.position, transform.up * 10, Color.green);
		Debug.DrawRay(transform.position, rb.velocity, Color.blue);
		Debug.DrawRay(transform.position, Vector3.Project(rb.velocity, transform.up), Color.red);
		//Debug.Log(Vector3.Project(rb.velocity, transform.up).magnitude > bonkThreshold);

		//bool bonking = ;
		//if (bonking)
		//{
			//Debug.Log(rb.velocity.magnitude);
		//}
		

		//To make sure the worm stays on the axis.
		rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, 0);
		if (transform.position.x != 0)
		{
			float xDis = transform.position.x;
			transform.position = Vector3.Scale(transform.position, new Vector3(0, 1, 1));
			foreach (OuterWilds n in FindObjectsOfType<OuterWilds>())
			{
				GameObject obj = n.gameObject;
				obj.transform.position = new Vector3(obj.transform.position.x - xDis, obj.transform.position.y, obj.transform.position.z);
			}
		}
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,
											Mathf.Round(transform.eulerAngles.y / 180) * 180,
											Mathf.Round(transform.eulerAngles.z / 180) * 180);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			transform.localScale = Vector3.Scale(initialScale, new Vector3(1, 0.75f, 1));
			transform.position = transform.position - transform.up * 0.75f;
			crouching = true;
		}
		animator.SetBool("Crouching", crouching);


		//Grapple
		RaycastHit hit;
		Vector3 hitPoint = Vector3.zero;
		Vector3 buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.localScale.y;

		if (Input.GetMouseButtonDown(0) && grappleHit.activeSelf && canGrapple)
		{
			canGrapple = false;
			grappling = true;
			if (Vector3.Distance(buttPosition, grappleHit.transform.position) < (minRopeLength + ropeSpacing) * numSegments)
			{
				float numRopes = Vector3.Distance(buttPosition, grappleHit.transform.position) / minRopeLength;
				createRope(numRopes, Vector3.Distance(buttPosition, grappleHit.transform.position), 0.05f);
			}
			else
			{
				createRope(numSegments, Vector3.Distance(buttPosition, grappleHit.transform.position), 0.05f);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			destroyRope();
			grappling = false;
		}

		if (grappleHit.activeSelf)
		{
			tail.material = tailOn;
		}
		else
		{
			tail.material = tailOff;
		}

		if (!Input.GetMouseButton(0))
		{
			//hinge.connectedBody = null;
			if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range) && canGrapple)
			{
				grappleHit.SetActive(true);
				grappleHit.transform.position = hit.point;
				//Debug.DrawRay(buttPosition, transform.TransformDirection(Vector3.down) * range, Color.red);
			}
			else
			{
				grappleHit.SetActive(false);
			}
		}

		float bonkScale = Vector3.Project(prevVelocity.normalized, transform.up.normalized).magnitude;
		bonkScale += bonkScale * Mathf.Sign(Vector3.Dot(prevVelocity, transform.up));
		bonkScale /= 2;
		bonkScale += 0.01f;
		//Debug.Log(bonkScale);
		//bonkScale += Mathf.Sign(Vector3.Dot(rb.velocity, transform.up));
		//Debug.Log("0." + Mathf.Round(bonkScale * 100));
		Debug.Log(bonkThreshold / bonkScale);
		if ((prevVelocity.magnitude - rb.velocity.magnitude) > (bonkThreshold / bonkScale) && !grappling)
		{
			prevVelocity = rb.velocity;
			rb.velocity /= 5;
			rb.AddForce(-prevVelocity * 2f, ForceMode.Impulse);
			Debug.Log("bonk");
			animator.SetBool("Bonking", true);
		}
		else
		{
			animator.SetBool("Bonking", false);
		}
		prevVelocity = rb.velocity;


		
	}

	private void FixedUpdate()
	{
		Debug.Log("clear");
		float moveY = Input.GetAxis("Vertical") * Mathf.Abs(Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;

		//Camera direction change
		float cameraRot = cameraRig.transform.eulerAngles.y;
		if (cameraRot > 180)
		{
			cameraRot -= 360;
		}
		//Debug.Log(cameraRot);
		if (Mathf.Abs(cameraRot) <= 90 - rotFlipOffset)
		{
			walkForward = true;
		}
		else if (Mathf.Abs(cameraRot) >= 90 + rotFlipOffset)
		{
			walkForward = false;
		}

		if (walkForward)
		{
			rb.AddRelativeTorque(new Vector3(moveY, 0, 0), ForceMode.Impulse);
		}
		else
		{
			rb.AddRelativeTorque(new Vector3(-moveY, 0, 0), ForceMode.Impulse);
		}

		if (crouching && !Input.GetKey(KeyCode.Space))
		{
			transform.localScale = initialScale;
			if (grounded)
			{
				rb.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
			}
			crouching = false;
		}


	}
	/*
	public void LateUpdate()
	{
		if (bonkTrigger.isBonking() && Vector3.Project(rb.velocity, transform.up).magnitude > bonkThreshold)
		{
			Debug.Log("bonk");

		}
	}
	*/

	private void createRope(float segments, float length, float spacing)
	{
		GameObject rope = null;
		Vector3 storedVelocity = rb.velocity;
		float yScale = (length / segments) / 2 - spacing / 2;
		float xScale = 0.2f;
		//the bottom of the rope connected to the swinging object
		buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.localScale.y;
		GameObject prevRope = Instantiate(generalRope, transform.position, transform.rotation);
		prevRope.transform.localScale = new Vector3(xScale, transform.localScale.y, xScale);
		prevRope.GetComponent<Rigidbody>().velocity = storedVelocity;
		prevRope.GetComponent<MeshRenderer>().enabled = false;
		prevRope.GetComponent<BoxCollider>().enabled = false;
		gameObject.AddComponent<FixedJoint>().connectedBody = prevRope.GetComponent<Rigidbody>();
		//the middle segments of the rope
		for (int i = 0; i <= segments; i++)
		{
			rope = Instantiate(generalRope, buttPosition - (i + 0.5f) * (transform.TransformDirection(Vector3.up) * length / segments), transform.rotation);
			rope.transform.localScale = new Vector3(xScale, yScale, xScale);
			prevRope.GetComponent<HingeJoint>().connectedBody = rope.GetComponent<Rigidbody>();
			prevRope.GetComponent<Rigidbody>().velocity = storedVelocity * (segments - i) / segments;
			prevRope.GetComponent<Rigidbody>().AddTorque(getRandomVector(300f, 0f, 0f));

			GameObject drop = Instantiate(ropeDrop, buttPosition - (i) * (transform.TransformDirection(Vector3.up) * length / segments), transform.rotation);
			drop.GetComponent<FixedJoint>().connectedBody = prevRope.GetComponent<Rigidbody>();
			prevRope = rope;
		}
		rope.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
		rope.GetComponent<BoxCollider>().enabled = false;
		Destroy(rope.GetComponent<HingeJoint>());

	}

	private void destroyRope()
	{
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Rope"))
		{
			Destroy(obj);
		}
		Destroy(gameObject.GetComponent<FixedJoint>());
	}
	private Vector3 getRandomVector(float xRange, float yRange, float zRange)
	{
		return new Vector3(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange), Random.Range(-zRange, zRange));
	}
}
