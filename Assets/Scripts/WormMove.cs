using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMove : MonoBehaviour
{
	public float moveSpeed = 1000;
	public float maxAngVel = 4f;
	public float maxSpeed = 100f;
	public float jumpForce = 50f;
	public bool underwater = false;
	private float underwaterScale = 0.7f;
	private float underwaterDrag = 2f;
	private float timeUnderwater = 0f;

	public GameObject generalRope;
	public GameObject ropeDrop;
	
	private Vector3 initialScale;
	private Rigidbody rb;
	private JumpTrigger jumpTrigger;
	private bool grounded;
	private bool crouching;
	private float groundedDrag = 1.5f;


	public float range = 50f;
	public float minRopeLength = 0.5f;
	public int numSegments = 10;
	public float ropeSpacing = 0.05f;
	public GameObject grappleHitPrefab;
	private GameObject grappleHit;
	private bool canGrapple;
	private Vector3 buttPosition;
	private bool grappling;
	private bool itsGrappleTime = false;
	private float howLongDoesGrappleTimeLast = 0.2f;
	private float howMuchLongerIsItGrappleTime;

	private GameObject currentGrapplePoint;
	public GameObject grapplePointPrefab;
	//private ConfigurableJoint joint;
	//public GameObject jointPoint;
	private SpringJoint spring;
	private float ropeLength;

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
		rb = GetComponent<Rigidbody>();
		initialScale = transform.localScale;
		grappleHit = Instantiate(grappleHitPrefab, Vector3.zero, Quaternion.identity);
		spring = GetComponent<SpringJoint>();
		//joint = GetComponent<ConfigurableJoint>();
		//jointPoint.transform.position = new Vector3(3000, 3000, 3000);
		//hinge = grappleHit.GetComponent<HingeJoint>();

    }

	// Update is called once per frame
	void Update()
	{

		grounded = jumpTrigger.getGrounded();
		//buttPosition = transform.position + transform.TransformDirection(Vector3.down) * 1.777f;
		buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.localScale.y;

		if (grounded)
		{
			canGrapple = true;
		}

		if (GetComponentInChildren<BodyFriction>().getFriction())
		{
			rb.drag = groundedDrag;
		}
		else
		{
			rb.drag = 0;
		}

		
		if (underwater)
		{
			timeUnderwater += Time.deltaTime;
			if (rb.velocity.magnitude > maxSpeed)// * underwaterScale)
			{
				rb.velocity = rb.velocity.normalized * maxSpeed; //* underwaterScale;
			}
			if (timeUnderwater > 0.2f)
			{
				rb.drag = underwaterDrag;
			}
			rb.maxAngularVelocity = maxAngVel * underwaterScale;
			rb.angularDrag = 1.5f * underwaterDrag;
		}
		else
		{
			timeUnderwater = 0;
			if (rb.velocity.magnitude > maxSpeed)
			{
				rb.velocity = rb.velocity.normalized * maxSpeed;
			}
			rb.maxAngularVelocity = maxAngVel;
			rb.angularDrag = 1.5f;
		}

		

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
			//transform.position = transform.position - transform.up * 0.75f;
			crouching = true;
		}
		animator.SetBool("Crouching", crouching);



		RaycastHit hit;
		Vector3 hitPoint = Vector3.zero;
		//Vector3 buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.localScale.y;

		if (!grappling)
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

		if (grappleHit.activeSelf)
		{
			tail.material = tailOn;
		}
		else
		{
			tail.material = tailOff;
		}

		if (Input.GetMouseButtonDown(0) && canGrapple) //on click, makes the first point
		{
			if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range))
			{
				GameObject grapplePoint = Instantiate(grapplePointPrefab, hit.point, Quaternion.identity);
				grapplePoint.transform.position = Vector3.MoveTowards(grapplePoint.transform.position, buttPosition, 0);
				currentGrapplePoint = grapplePoint;
				ropeLength = Vector3.Distance(currentGrapplePoint.transform.position, buttPosition);
				grappling = true;
				canGrapple = false;
				rb.AddForce(Vector3.down * 4 * rb.velocity.magnitude, ForceMode.Impulse);
				//Debug.Log(Vector3.Distance(buttPosition, hit.point));
			}
			
		}
		

		if (currentGrapplePoint != null) //if grappling
		{

			if (Physics.Linecast(buttPosition, currentGrapplePoint.transform.position, out hit)) //checks whether a line can be drawn to the worm
			{
				Debug.DrawLine(buttPosition, currentGrapplePoint.transform.position);
				//Debug.Log(hit.collider.gameObject);

				if (hit.collider.gameObject.tag != "GP") //if the line hit something other than the player
				{                                           //creates a new point and sets that to the currentGrapplePoint
															//RaycastHit hit2;
					if (Physics.Linecast(buttPosition, currentGrapplePoint.transform.position, out hit))
					{
						Debug.DrawLine(buttPosition, currentGrapplePoint.transform.position, Color.red);
						GameObject grapplePoint = Instantiate(grapplePointPrefab, hit.point, Quaternion.identity);
						grapplePoint.GetComponent<GrapplePoint>().setLastPoint(currentGrapplePoint);
						currentGrapplePoint.GetComponent<GrapplePoint>().setNextPoint(grapplePoint);
						grapplePoint.transform.position = Vector3.MoveTowards(grapplePoint.transform.position, buttPosition, 0);

						currentGrapplePoint = grapplePoint;
						currentGrapplePoint.GetComponent<GrapplePoint>().setCurrent(true);
						Debug.Log("made a new point");
					}
				}

			}

			GameObject lastPoint = currentGrapplePoint.GetComponent<GrapplePoint>().getLastPoint();
			if (lastPoint != null)
			{
				Debug.DrawLine(buttPosition, lastPoint.transform.position);
			}
			if (lastPoint != null && Physics.Linecast(buttPosition, lastPoint.transform.position, out hit))//If the last point can draw a line to the worm
			{                                                                                               //changes the current point to the last point.

				Debug.Log(hit.collider.gameObject);
				if (hit.collider.gameObject.tag == "GP")
				{
					
					int disTemp = (int)Vector3.Distance(currentGrapplePoint.transform.position, buttPosition);
					//Debug.Log(disTemp);
					if (!sweepArea(buttPosition, currentGrapplePoint.transform.position, lastPoint.transform.position, 100))
					{
						GameObject temp = currentGrapplePoint;
						currentGrapplePoint = currentGrapplePoint.GetComponent<GrapplePoint>().getLastPoint();
						GameObject.Destroy(temp);
						Debug.Log("destroyed a point");
						currentGrapplePoint.GetComponent<GrapplePoint>().setCurrent(true);
					}
				}
			}

			
			float currentRopeLength = currentGrapplePoint.GetComponent<GrapplePoint>().getTotalDistance();
			spring.connectedBody = currentGrapplePoint.GetComponent<Rigidbody>();
			spring.maxDistance = ropeLength - currentRopeLength;
			spring.connectedAnchor = Vector3.zero;
			

			LineRenderer line = currentGrapplePoint.GetComponent<LineRenderer>();
			line.SetPosition(0, currentGrapplePoint.transform.position);
			line.SetPosition(1, buttPosition);

			
		}
		if (Input.GetMouseButtonUp(0)) //If left click is released
		{
			grappling = false;
			if (currentGrapplePoint != null)
			{
				currentGrapplePoint.GetComponent<GrapplePoint>().destroySelf(); //destroys all the grapplePoints
				//currentGrapplePoint = null;
			}
			spring.maxDistance = Mathf.Infinity;
		}


		//GRAPPLE CODE: this if statement is just to collapse the code
		if (false) //GRAPPLE CODE
		{
			/*
			//Grapple
			RaycastHit hit;
			Vector3 hitPoint = Vector3.zero;
			Vector3 buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.localScale.y;

			if (!grappling)
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

			if (Input.GetMouseButtonDown(0))
			{
				itsGrappleTime = true;
			}
			if (itsGrappleTime)
			{
				howMuchLongerIsItGrappleTime += Time.deltaTime;
				if (howMuchLongerIsItGrappleTime > howLongDoesGrappleTimeLast || Input.GetMouseButtonUp(0))
				{
					itsGrappleTime = false;
					howMuchLongerIsItGrappleTime = 0;
				}
			}
			Debug.Log(itsGrappleTime);
			//rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			if (itsGrappleTime && grappleHit.activeSelf && canGrapple)
			{
				Debug.Log(rb.velocity.y);
				canGrapple = false;
				itsGrappleTime = false;
				howMuchLongerIsItGrappleTime = 0;
				grappling = true;
				if (rb.velocity.y > 0)
				{
					rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / Mathf.Clamp(Mathf.Pow(1.03f, rb.velocity.y), 1, 3), rb.velocity.z);
					//Debug.Log("Yep");
				}


				//so that grappling to a wall youre moving towards doesnt kill your momentum
				float pullForce = 20f;
				Vector3 tailFacing = transform.TransformDirection(Vector3.down);
				Vector3 forwardVelocity = Vector3.Project(rb.velocity, tailFacing);
				rb.AddForce(transform.TransformDirection(Vector3.down) * pullForce * forwardVelocity.magnitude, ForceMode.Impulse);



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


			*/
		}


		
	}



	private void FixedUpdate()
	{
		//Debug.Log("clear");
		float moveY = Input.GetAxis("Vertical") * Mathf.Abs(Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;
		if (underwater)
		{
			moveY *= underwaterScale;
		}

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

	private bool sweepArea(Vector3 origin, Vector3 start, Vector3 end, int iterations) //returns whether there is a collider in the triangle formed by the three vectors.
	{
		start = Vector3.MoveTowards(start, origin, 0.5f);
		end = Vector3.MoveTowards(end, origin, 0.5f);
		Debug.Log("SWEEPING");
		Vector3 raycastLocation = start;
		float itrDelta = Vector3.Distance(start, end) / iterations;

		//bool willReturn = false;
		RaycastHit hitt;
		for (int i = 0; i < iterations; i++)
		{
			Debug.DrawLine(origin, raycastLocation);
			raycastLocation = Vector3.MoveTowards(raycastLocation, end, itrDelta);
			if (Physics.Linecast(origin, raycastLocation, out hitt))
			{
				if (hitt.collider.gameObject.tag != "GP")
				{
					Debug.Log(hitt.collider.gameObject);
					return true;
					//willReturn = true;
				}
			}
			
		}
		//return willReturn;
		return false;
	}


	/*
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
			rope.GetComponent<GrappleFadeIn>().setTimer(i);
			prevRope.GetComponent<Rigidbody>().velocity = storedVelocity * (segments - i) / segments;
			//prevRope.GetComponent<Rigidbody>().AddTorque(getRandomVector(800f, 0f, 0f));
			prevRope.GetComponent<HingeJoint>().connectedBody = rope.GetComponent<Rigidbody>();
			

			GameObject drop = Instantiate(ropeDrop, buttPosition - (i) * (transform.TransformDirection(Vector3.up) * length / segments), transform.rotation);
			drop.GetComponent<FixedJoint>().connectedBody = prevRope.GetComponent<Rigidbody>();
			drop.GetComponent<GrappleFadeIn>().setTimer(i);
			prevRope = rope;
		}
		rope.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
		rope.GetComponent<BoxCollider>().enabled = false;
		rope.GetComponent<GrappleFadeIn>().setTimer(-1);
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
	*/
}
