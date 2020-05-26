using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMove : MonoBehaviour
{
	public float moveSpeed = 1000;
	public float maxAngVel = 4f;
	public float angDragGrounded = 6f;
	public float angDragAirborn = 4f;
	public float maxSpeed = 100f;
	public float jumpForce = 50f;
	public float terminalVelocity = 45f;
	//public bool underwater = false;
	//private float underwaterScale = 0.7f;
	//private float underwaterDrag = 2f;
	//private float timeUnderwater = 0f;

	public GameObject generalRope;
	public GameObject ropeDrop;
	
	private Vector3 initialScale;
	private Rigidbody rb;
	private JumpTrigger jumpTrigger;
	private bool grounded;
	private bool solidGround;
	private bool crouching;
	public float groundedDrag = 1.5f;


	public float range = 50f;
	//public float minRopeLength = 0.5f;
	//public int numSegments = 10;
	//public float ropeSpacing = 0.05f;
	public float stamina = 10f;
	public float staminaRefresh = 1f;
	public float refreshDelay = 1f;
	public float staminaOnClick = 1f;
	private float refreshTimer;
	public GameObject grappleHitPrefab;
	private GameObject grappleHit;
	private bool canGrapple;
	private Vector3 buttPosition;
	private bool grappling;
	private bool itsGrappleTime = false;
	private float howLongDoesGrappleTimeLast = 0.2f;
	private float howMuchLongerIsItGrappleTime;
	private float grappleTimer = 0;
	

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
	public Material tailOnExerted;
	public Material tailOff;
	public Material tailOffExerted;
	public MeshRenderer tail;

	public Animator animator;

	public ParticleSystem dust;
	public TrailRenderer trail;
	private Vector3 prevVelocity;
	public float smokeSpeed;
	public ParticleSystem speedLines;
	public ParticleSystem jump;

	public float heldItemDistance;
	private bool holdingObject;
	private GameObject heldObject;

	public Vector3 initRot;
	//private HingeJoint hinge;

	// Start is called before the first frame update
	void Start()
    {
		transform.eulerAngles = new Vector3(transform.eulerAngles.x + initRot.x, transform.eulerAngles.y + initRot.y, transform.eulerAngles.z + initRot.z);
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
		if (PauseMenu.isPaused)
		{
			return;
		}
		if (grappling)
		{
			LineRenderer line = currentGrapplePoint.GetComponent<LineRenderer>();
			line.SetPosition(0, currentGrapplePoint.transform.position);
			line.SetPosition(1, transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y); //absolute butt position
		}
		if (DetectWin.hasWon)
		{
			return;
		}

		grounded = jumpTrigger.getGrounded();
		solidGround = jumpTrigger.getSolidGround();
		buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y * 0.8f;

		if (solidGround)
		{
			canGrapple = true;
		}

		if (grounded)
		{
			rb.angularDrag = angDragGrounded;
		}
		else
		{
			rb.angularDrag = angDragAirborn;
		}

		if (GetComponentInChildren<BodyFriction>().getFriction())
		{
			rb.drag = groundedDrag;
		}
		else
		{
			rb.drag = 0;
		}

		Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		if (horizontalVelocity.magnitude > maxSpeed)
		{
			rb.velocity = (horizontalVelocity.normalized * maxSpeed) + rb.velocity.y * Vector3.up;
		}
		rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -terminalVelocity * 1.5f, terminalVelocity), rb.velocity.z);
		
		rb.maxAngularVelocity = maxAngVel;

		

		//To make sure the worm stays on the axis.
		rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, 0);
		//rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
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


		//Debug.Log(grappleTimer);
		RaycastHit hit;
		Vector3 hitPoint = Vector3.zero;
		//Vector3 buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.localScale.y;

		//Debug.Log(grappleTimer);
		if (!grappling)
		{
			refreshTimer += Time.deltaTime;
			if (refreshTimer > refreshDelay)
			{
				grappleTimer -= staminaRefresh * Time.deltaTime;
				grappleTimer = Mathf.Clamp(grappleTimer, 0, stamina);
			}
			//hinge.connectedBody = null;
			if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range) && canGrapple && grappleTimer < stamina && !holdingObject)
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
		else
		{
			//grappleTimer += Time.deltaTime;
			refreshTimer = 0;
		}

		if (grappleHit.activeSelf || holdingObject)
		{
			tail.material.Lerp(tailOn, tailOnExerted, (grappleTimer / stamina) * 2 - 1);
		}
		else
		{
			tail.material.Lerp(tailOff, tailOffExerted, (grappleTimer / stamina) * 2 - 1);
		}
		grappleHit.GetComponent<MeshRenderer>().material = tail.material;
		foreach(LineRenderer rend in FindObjectsOfType<LineRenderer>())
		{
			if (rend.gameObject.tag == "GP")
			{
				rend.material = tail.material;
			}
		}


		if (Input.GetMouseButtonDown(0))
		{
			if (holdingObject)
			{
				holdingObject = false;
				heldObject.GetComponent<LineRenderer>().enabled = false;
				EggBreak eggBreak = heldObject.GetComponent<EggBreak>();
				if (eggBreak != null)
				{
					eggBreak.prime();
				}
				heldObject = null;
				spring.maxDistance = Mathf.Infinity;
			}
			else
			{
				itsGrappleTime = true;

				if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range)) //grabbing object
				{
					if (hit.collider.gameObject.tag == "Egg")
					{
						itsGrappleTime = false;
						GameObject egg = hit.collider.gameObject;
						Rigidbody eggRB = egg.GetComponent<Rigidbody>();
						spring.connectedBody = eggRB;
						eggRB.isKinematic = false;
						spring.maxDistance = heldItemDistance;
						holdingObject = true;
						//grappling = true;
						heldObject = egg;
					}
				}
			}
			
		}
		if (holdingObject)
		{
			LineRenderer line = heldObject.GetComponent<LineRenderer>();
			line.enabled = true;
			line.SetPosition(0, heldObject.transform.position);
			line.SetPosition(1, transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y); //absolute butt position
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
		if (itsGrappleTime && canGrapple) //on click, makes the first point
		{
			
			if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range))
			{
				GameObject grapplePoint = Instantiate(grapplePointPrefab, hit.point, Quaternion.identity);
				grapplePoint.transform.position = Vector3.MoveTowards(grapplePoint.transform.position, buttPosition, 0);
				currentGrapplePoint = grapplePoint;
				ropeLength = Vector3.Distance(currentGrapplePoint.transform.position, buttPosition);

				grappling = true;
				canGrapple = false;
				itsGrappleTime = false;
				howMuchLongerIsItGrappleTime = 0;

				if (rb.velocity.y < 0)
				{
					rb.AddForce(Vector3.down * 4 * rb.velocity.magnitude, ForceMode.Impulse);
				}
				//if (grappleTimer + staminaOnClick < stamina)
				//{
				//	grappleTimer += staminaOnClick;
				//}
				//Debug.Log(Vector3.Distance(buttPosition, hit.point));
			}
			
		}

		if (currentGrapplePoint != null) //if grappling
		{
			
			if (Physics.Linecast(buttPosition, currentGrapplePoint.transform.position, out hit)) //checks whether a line can be drawn to the worm
			{
				//Debug.DrawLine(buttPosition, currentGrapplePoint.transform.position);
				//Debug.Log(hit.collider.gameObject);

				if (hit.collider.gameObject.tag != "GP") //if the line hit something other than the player
				{                                           //creates a new point and sets that to the currentGrapplePoint
															//RaycastHit hit2;
					if (Physics.Linecast(buttPosition, currentGrapplePoint.transform.position, out hit))
					{
						//Debug.DrawLine(buttPosition, currentGrapplePoint.transform.position, Color.red);
						GameObject grapplePoint = Instantiate(grapplePointPrefab, hit.point, Quaternion.identity);
						grapplePoint.GetComponent<GrapplePoint>().setLastPoint(currentGrapplePoint);
						currentGrapplePoint.GetComponent<GrapplePoint>().setNextPoint(grapplePoint);
						grapplePoint.transform.position = Vector3.MoveTowards(grapplePoint.transform.position, buttPosition, 0);

						currentGrapplePoint = grapplePoint;
						currentGrapplePoint.GetComponent<GrapplePoint>().setCurrent(true);
						//Debug.Log("made a new point");
					}
				}

			}

			GameObject lastPoint = currentGrapplePoint.GetComponent<GrapplePoint>().getLastPoint();
			hit = new RaycastHit();
			if (lastPoint != null && Physics.Linecast(buttPosition, lastPoint.transform.position, out hit))//If the last point can draw a line to the worm
			{                                                                                               //changes the current point to the last point.
				
				if (hit.collider.gameObject.tag == "GP" && !hit.collider.gameObject.GetComponent<GrapplePoint>().getCurrent())
				{
					
					int disTemp = (int)Vector3.Distance(currentGrapplePoint.transform.position, buttPosition);
					//Debug.Log(disTemp);
					if (!sweepArea(buttPosition, currentGrapplePoint.transform.position, lastPoint.transform.position, 100))
					{
						GameObject temp = currentGrapplePoint;
						currentGrapplePoint = currentGrapplePoint.GetComponent<GrapplePoint>().getLastPoint();
						currentGrapplePoint.GetComponent<GrapplePoint>().setCurrent(true);
						GameObject.Destroy(temp);
						//Debug.Log("destroyed a point");
						
					}
				}
			}


			float stretchLeway = range / 13;
			float currentRopeLength = currentGrapplePoint.GetComponent<GrapplePoint>().getTotalDistance();
			if (Vector3.Distance(currentGrapplePoint.transform.position, buttPosition) + currentRopeLength > (ropeLength + stretchLeway))
			{
				//Debug.Log(Vector3.Distance(currentGrapplePoint.transform.position, buttPosition) + currentRopeLength - (ropeLength + stretchLeway) > 1);
				grappleTimer += Time.deltaTime * Mathf.Pow((Vector3.Distance(currentGrapplePoint.transform.position, buttPosition) + currentRopeLength - (ropeLength + stretchLeway)), 3);
			}
			else
			{
				//spring.maxDistance = ropeLength - currentRopeLength;
			}
			spring.connectedBody = currentGrapplePoint.GetComponent<Rigidbody>();
			spring.maxDistance = ropeLength - currentRopeLength;
			spring.connectedAnchor = Vector3.zero;
			
			
			

			LineRenderer line = currentGrapplePoint.GetComponent<LineRenderer>();
			line.SetPosition(0, currentGrapplePoint.transform.position);
			line.SetPosition(1, transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y); //absolute butt position

			
		}
		//*
		if ((!Input.GetMouseButton(0) || grappleTimer >= stamina) && !holdingObject) //If left click is released
		{
			itsGrappleTime = false;
			howMuchLongerIsItGrappleTime = 0;
			grappling = false;
			if (currentGrapplePoint != null)
			{
				currentGrapplePoint.GetComponent<GrapplePoint>().destroySelf(); //destroys all the grapplePoints
				//currentGrapplePoint = null;
			}
			spring.maxDistance = Mathf.Infinity;
		}
		//*/


		
	}



	private void FixedUpdate()
	{
		if (PauseMenu.isPaused || DetectWin.hasWon)
		{
			return;
		}
		//Debug.Log("clear");
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
				/*if (grappling)
				{
					rb.AddRelativeForce(new Vector3(0, jumpForce * 0.7f, 0), ForceMode.Impulse);
				}
				else
				{
					rb.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
				}*/
				rb.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
				playJump();
			}
			crouching = false;
		}

		if (rb.velocity.magnitude > smokeSpeed)
		{
			
			if (grounded)
			{
				playDust();
				trail.enabled = false;
			}
			else
			{
				playSpeedLines();
				trail.enabled = true;
			}
		}
		else
		{
			trail.enabled = false;
		}
		/*
		if (grounded && rb.velocity.magnitude + 1f < prevVelocity.magnitude)
		{
			playDust();
		}
		prevVelocity = rb.velocity;*/


	}

	private bool sweepArea(Vector3 origin, Vector3 start, Vector3 end, int iterations) //returns whether there is a collider in the triangle formed by the three vectors.
	{
		start = Vector3.MoveTowards(start, origin, 0.5f);
		end = Vector3.MoveTowards(end, origin, 0.5f);
		//Debug.Log("SWEEPING");
		Vector3 raycastLocation = start;
		float itrDelta = Vector3.Distance(start, end) / iterations;

		//bool willReturn = false;
		RaycastHit hitt;
		for (int i = 0; i < iterations; i++)
		{
			raycastLocation = Vector3.MoveTowards(raycastLocation, end, itrDelta);
			int layerMask = 1 << 8;
			if (Physics.Linecast(origin, raycastLocation, out hitt, layerMask))
			{
				//if (hitt.collider.gameObject.tag != "GP")
				//{
					//Debug.Log("Sweep hit " + hitt.collider.gameObject);
					return true;
					//willReturn = true;
				//}
			}
			//else if(Physics.CheckSphere
			
		}
		//return willReturn;
		return false;
	}

	private void playDust()
	{
		dust.Play();
	}
	private void playJump()
	{
		jump.Play();
	}

	private void playSpeedLines()
	{
		speedLines.Play();
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
