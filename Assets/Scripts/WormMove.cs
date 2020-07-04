using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMove : MonoBehaviour
{
	public bool twod;


	private int frame = 0;

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

	private bool jumpUp;
	private bool jumpDown;
	private bool jumpHeld;


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
	private bool startingGrapple;
	private bool lockGrapple;
	

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
	private Game game;
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
		game = FindObjectOfType<Game>();
		if (game.getStartingGrapple())
		{
			animator.SetBool("Asleep", true);
			startingGrapple = true;
			if (!lockGrapple)
			{
				canGrapple = true;
			}
		}
    }

	// Update is called once per frame
	void Update()
	{
		if (PauseMenu.isPaused || SaveLoad.initializing || DetectWin.hasWon)
		{
			return;
		}
		if (grappling)
		{
			LineRenderer line = currentGrapplePoint.GetComponent<LineRenderer>();
			line.SetPosition(0, currentGrapplePoint.transform.position);
			line.SetPosition(1, transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y); //absolute butt position
		}

		


		grounded = jumpTrigger.getGrounded();
		solidGround = jumpTrigger.getSolidGround();
		buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y * 0.7f;

		if (solidGround && !grappling && !startingGrapple)
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
		if (twod)
		{
			rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
		}

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

		if (jumpDown)
		{
			transform.localScale = Vector3.Scale(initialScale, new Vector3(1, 0.75f, 1));
			//transform.position = transform.position - transform.up * 0.75f;
			crouching = true;
		}
		animator.SetBool("Crouching", crouching);


		RaycastHit hit;
		Vector3 hitPoint = Vector3.zero;

		if (!grappling)
		{
			if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range) && canGrapple && grappleTimer < stamina && !holdingObject)
			{
				grappleHit.SetActive(true);
				grappleHit.transform.position = hit.point;
			}
			else
			{
				grappleHit.SetActive(false);
			}
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


		if (Input.GetMouseButtonDown(0) || (startingGrapple && frame == 2))
		{
			if (holdingObject && !startingGrapple)
			{
				grabObject(null);
			}
			else
			{
				itsGrappleTime = true;

				if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range)) //grabbing object
				{
					if (hit.collider.gameObject.tag == "Egg")
					{
						grabObject(hit.collider.gameObject);

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
			}
			
		}

		if (currentGrapplePoint != null) //if grappling
		{
			
			if (Physics.Linecast(buttPosition, currentGrapplePoint.transform.position, out hit)) //checks whether a line can be drawn to the worm
			{

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

			spring.connectedBody = currentGrapplePoint.GetComponent<Rigidbody>();
			spring.maxDistance = ropeLength - currentRopeLength;
			spring.connectedAnchor = Vector3.zero;
			
			
			

			LineRenderer line = currentGrapplePoint.GetComponent<LineRenderer>();
			line.SetPosition(0, currentGrapplePoint.transform.position);
			line.SetPosition(1, transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y); //absolute butt position

			
		}
		//*
		if (startingGrapple)
		{
			if (!StartMenu.isOpen)
			{
				if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) //worm drops
				{
					Debug.Log("drop");
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;

					startingGrapple = false;
					game.setStartingGrapple(false);
					itsGrappleTime = false;
					animator.SetBool("Asleep", false);
					howMuchLongerIsItGrappleTime = 0;
					grappling = false;
					if (currentGrapplePoint != null)
					{
						currentGrapplePoint.GetComponent<GrapplePoint>().destroySelf(); //destroys all the grapplePoints
																						//currentGrapplePoint = null;
					}
					if (!holdingObject)
					{
						spring.maxDistance = Mathf.Infinity;
					}
					float startingJoltForce = 3000f;
					rb.AddForce(Vector3.up * startingJoltForce);
				}
			}
			
		}
		else if (!Input.GetMouseButton(0) && !holdingObject) //If left click is released
		{
			itsGrappleTime = false;
			howMuchLongerIsItGrappleTime = 0;
			grappling = false;
			if (currentGrapplePoint != null)
			{
				currentGrapplePoint.GetComponent<GrapplePoint>().destroySelf(); //destroys all the grapplePoints
			}
			spring.maxDistance = Mathf.Infinity;
		}
		//*/

		setJumpInput(twod);
		frame++;
	}



	private void FixedUpdate()
	{
		if (startingGrapple)
		{
			return;
		}

		if (PauseMenu.isPaused || DetectWin.hasWon)
		{
			return;
		}

		float moveY = 0;
		if (twod)
		{
			if (Input.GetKey(KeyCode.D))
				moveY += 1;
			if (Input.GetKey(KeyCode.A))
				moveY -= 1;
			moveY *= moveSpeed * Time.deltaTime;
		}
		else
		{
			moveY = Input.GetAxis("Vertical") * Mathf.Abs(Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;
		}

		//Camera direction change
		if (!twod && Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f)
		{
			float cameraRot = cameraRig.transform.eulerAngles.y;
			if (cameraRot > 180)
			{
				cameraRot -= 360;
			}
			if (Mathf.Abs(cameraRot) <= 90 - rotFlipOffset)
			{
				walkForward = true;
			}
			else if (Mathf.Abs(cameraRot) >= 90 + rotFlipOffset)
			{
				walkForward = false;
			}
		}
		if (walkForward)
		{
			rb.AddRelativeTorque(new Vector3(moveY, 0, 0), ForceMode.Impulse);
		}
		else
		{
			rb.AddRelativeTorque(new Vector3(-moveY, 0, 0), ForceMode.Impulse);
		}

		if (crouching && !jumpHeld)
		{
			transform.localScale = initialScale;
			if (grounded)
			{
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


	}

	private bool sweepArea(Vector3 origin, Vector3 start, Vector3 end, int iterations) //returns whether there is a collider in the triangle formed by the three vectors.
	{
		start = Vector3.MoveTowards(start, origin, 0.5f);
		end = Vector3.MoveTowards(end, origin, 0.5f);
		Vector3 raycastLocation = start;
		float itrDelta = Vector3.Distance(start, end) / iterations;

		RaycastHit hitt;
		for (int i = 0; i < iterations; i++)
		{
			raycastLocation = Vector3.MoveTowards(raycastLocation, end, itrDelta);
			int layerMask = 1 << 8;
			if (Physics.Linecast(origin, raycastLocation, out hitt, layerMask))
			{
				return true;
			}
			
		}
		return false;
	}

	public void grabObject(GameObject obj)
	{
		if (obj != null) //grabbing a new object
		{
			obj.AddComponent<OuterWilds>();
			itsGrappleTime = false;
			Rigidbody objRB = obj.GetComponent<Rigidbody>();
			spring.connectedBody = objRB;
			objRB.isKinematic = false;
			spring.maxDistance = heldItemDistance;
			holdingObject = true;
			//grappling = true;
			heldObject = obj;
		}
		else //releasing the current object.
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
	}

	public bool getHolding()
	{
		return holdingObject;
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

	private void setJumpInput(bool iid)
	{
		#region 2d code
		/*if (iid) //jump control stuff
		{
			bool j = Input.GetAxis("Jump") > 0;



			if (jumpHeld && !j) //if jump was pressed last frame and jump is not pressed now
			{
				jumpUp = true;
			}
			else
			{
				jumpUp = false;
			}

			if (!jumpHeld && j) //if jump was not pressed last frame and jump is pressed now
			{
				jumpDown = true;
			}
			else
			{
				jumpDown = false;
			}


			jumpHeld = j;
		}
		else
		{
		*/
		#endregion
		jumpUp = Input.GetKeyUp(KeyCode.Space);
		jumpDown = Input.GetKeyDown(KeyCode.Space);
		jumpHeld = Input.GetKey(KeyCode.Space);
	}

	public void load(Vector3 pos, Quaternion rot, Vector3 vel, bool hasEgg)
	{
		initRot = Vector3.zero;
		transform.position = pos;
		transform.rotation = rot;
		canGrapple = false;

		if(rb == null)
		{
			rb = GetComponent<Rigidbody>();
		}
		rb.velocity = vel;
		canGrapple = false;
		lockGrapple = true;

		if (hasEgg)
		{
			var eg = Instantiate(game.egg, transform.position, new Quaternion(0f, 0f, -0.1f, 0.5f));
			grabObject(eg);
			
		}
	}

}
