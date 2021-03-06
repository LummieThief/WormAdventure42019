﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	
	private Vector3 initialScale;
	private Rigidbody rb;
	private JumpTrigger jumpTrigger;
	private GroundedEnough groundedEnoughTrigger;
	private bool grounded;
	private bool solidGround;
	private bool groundedEnough;
	private bool crouching;
	public float groundedDrag = 1.5f;

	private bool jumpUp;
	private bool jumpDown;
	private bool jumpHeld;
	private bool jumpHasBeenSet;
	public float coyoteTime = 1f;
	private float coyoteTimer = 999f;


	public float range = 50f;

	public GameObject grappleHitPrefab;
	private GameObject grappleHit;
	public GameObject wormGrapplePointPrefab;

	public GameObject ropeStart;
	private RopeSegmentController rsc;

	private bool canGrapple;
	private Vector3 buttPosition;
	private bool grappling;
	private bool itsGrappleTime = false;
	private float howLongDoesGrappleTimeLast = 0.2f;
	private float howMuchLongerIsItGrappleTime;
	private float grappleTimer = 0;
	private float maxGrappleTime = 5f;
	private bool grapplingToWorm = false;
	//private float grapplingToWormTimer = 0;
	private GameObject wormGrapplePoint;

	private bool startingGrapple;
	private bool lockGrapple;
	private float currentRopeLength = -1;
	

	private SpringJoint spring;
	private float ropeLength;

	private GameObject cameraRig;
	private CameraFollow cameraFollow;
	public float rotFlipOffset = 30;
	private bool walkForward = true;

	private Material tailOn;
	private Material tailOnExerted;
	private Material tailOff;
	private Material tailOffExerted;
	//private Material tailMat;

	private MeshRenderer tail;

	private Animator animator;
	private bool dead = false;

	public ParticleSystem dust;
	public TrailRenderer trail;
	private Vector3 prevVelocity;
	private Vector3 prevAngVel;
	public float smokeSpeed;
	private ParticleSystem speedLines;
	public ParticleSystem jump;
	public ParticleSystem explosion;
	public ParticleSystem winExplosion;
	private BodyFriction friction;

	public float heldItemDistance;
	private bool holdingObject;
	private GameObject heldObject;

	public Vector3 initRot;
	private Game game;
	private SoundManager snd;
	private float lastVolume;

	//public List<Material> grassMaterials;
	public List<Material> stoneMaterials;
	public List<Material> woodMaterials;

	private float artificialWindSpeed;

	private float floorIsLavaTimer = 0f;
	private float floorIsLavaTime = 45;

	private Transform velocityPoint;

	private GameObject model;
	private bool flippedModel;
	private Vector3 prevRot;
	//private HingeJoint hinge;

	// Start is called before the first frame update
	void Start()
    {
		prevRot = transform.eulerAngles;
		cameraRig = GameObject.FindGameObjectWithTag("MainCamera");
		speedLines = cameraRig.transform.Find("Main Camera").Find("Speed Lines").GetComponent<ParticleSystem>();
		cameraFollow = cameraRig.GetComponent<CameraFollow>();
		
		transform.eulerAngles = new Vector3(transform.eulerAngles.x + initRot.x, transform.eulerAngles.y + initRot.y, transform.eulerAngles.z + initRot.z);
		jumpTrigger = GetComponentInChildren<JumpTrigger>();
		groundedEnoughTrigger = GetComponentInChildren<GroundedEnough>();
		friction = GetComponentInChildren<BodyFriction>();
		rb = GetComponent<Rigidbody>();
		initialScale = transform.localScale;
		grappleHit = Instantiate(grappleHitPrefab, Vector3.zero, Quaternion.identity);
		spring = GetComponent<SpringJoint>();
		game = FindObjectOfType<Game>();
		velocityPoint = GetComponentInChildren<VelocityPoint>().transform;

		//reset these when the model changes
		animator = transform.parent.GetComponentInChildren<Animator>();
		tail = animator.transform.Find("Tail").GetComponent<MeshRenderer>();
		TailLightBank tlb = tail.GetComponent<TailLightBank>();
		tailOn = tlb.getFreshOn();
		tailOff = tlb.getFreshOff();
		tailOnExerted = tlb.getExertedOn();
		tailOffExerted = tlb.getExertedOff();
		//tailMat = tailOn;
		//

		Transform outerWildsWorld = GameObject.FindGameObjectWithTag("OuterWildsWorld").transform;
		ParticleSystem.MainModule module = dust.main;
		module.simulationSpace = ParticleSystemSimulationSpace.Custom;
		module.customSimulationSpace = outerWildsWorld;
		module = jump.main;
		module.simulationSpace = ParticleSystemSimulationSpace.Custom;
		module.customSimulationSpace = outerWildsWorld;
		if (game == null)
		{
			startingGrapple = false;
		}
		else if (game.getStartingGrapple())
		{
			animator.SetBool("Asleep", true);
			startingGrapple = true;
			if (!lockGrapple)
			{
				canGrapple = true;
			}
		}
		else
		{
			if (SceneManager.GetActiveScene().name == "Attempt 2")
			{
				lastVolume = 1f;
			}
			else
			{
				lastVolume = 0.12f;
			}

			
		}

		/*
		if (game != null && game.getSkin() != 0)
		{
			changeSkin(game.getSkin());
		}
		*/
		if (PlayerPrefs.HasKey("Skin") && PlayerPrefs.GetInt("Skin") != 0)
		{
			changeSkin(PlayerPrefs.GetInt("Skin"));
		}
		snd = FindObjectOfType<SoundManager>();
		rsc = ropeStart.GetComponent<RopeSegmentController>();

    }

	// Update is called once per frame
	void Update()
	{
		if (rsc == null)
		{
			rsc = ropeStart.GetComponent<RopeSegmentController>();
		}
		if (snd == null)
		{
			snd = FindObjectOfType<SoundManager>();
		}

		float useThisSpeed = Mathf.Max(rb.velocity.magnitude, artificialWindSpeed);
		if (artificialWindSpeed < 0)
		{
			useThisSpeed = 0;
		}
		
		float vol = Mathf.Clamp(((useThisSpeed - smokeSpeed * 0.9f) / smokeSpeed), 0f, 1f);
		vol = Mathf.Lerp(lastVolume, vol, 1.5f * Time.deltaTime) * Time.timeScale;
		//Debug.Log(vol);
		if (snd != null)
		{
			snd.playSpeedWind(vol);
		}
		lastVolume = vol;

		//model.transform.localRotation = Quaternion.identity;
		//model.transform.localRotation = Quaternion.Euler(270, 0, -180);


		if (StartMenu.isOpen && model != null)
		{
			if (transform.eulerAngles.x > 180)
			{
				model.transform.localRotation = Quaternion.Euler(270, 0, -180);
				flippedModel = true;
			}
			else
			{
				model.transform.localRotation = Quaternion.Euler(90, 0, 0);
				flippedModel = false;
			}
		}
		else if ((grounded || groundedEnough) && model != null)
		{
			if (transform.eulerAngles.y > 179)
			{
				//*
				if (transform.eulerAngles.x > 0 && transform.eulerAngles.x < 180)
				{
					model.transform.localRotation = Quaternion.Euler(90, 0, 0);
					flippedModel = false;
				}
				else
				{
					model.transform.localRotation = Quaternion.Euler(270, 0, -180);
					flippedModel = true;
				}
				//*/
			}
			else
			{
				//as soon as it crossed 0 it flipped, and then as soon as it crossed 340 it flipped again.
				//Debug.Log(transform.eulerAngles.x);

				if (prevRot.x >= 180 && transform.eulerAngles.x < 340 && transform.eulerAngles.x > 300)
				{
					model.transform.localRotation = Quaternion.Euler(270, 0, -180); //flipped
					flippedModel = true;
				}
				else if (prevRot.x <= 180 && transform.eulerAngles.x > 20 && transform.eulerAngles.x < 60)
				{
					model.transform.localRotation = Quaternion.Euler(90, 0, 0); //correct
					flippedModel = false;

				}
			}



			
			//*/
			
		}
		prevRot = transform.rotation.eulerAngles;






		if (PauseMenu.isPaused || SaveLoad.initializing)
		{
			return;
		}
		else if (cameraFollow.getMouseFrozen() && !startingGrapple)
		{
			tail.sharedMaterial.Lerp(tailOff, tailOff, 1);
			return;
		}


		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			changeSkin(1);
			return;
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			changeSkin(0);
			return;
		}


		grounded = jumpTrigger.getGrounded();
		
		solidGround = jumpTrigger.getSolidGround();
		groundedEnough = groundedEnoughTrigger.getGroundedEnough();
		buttPosition = transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y * 0.9f;

		if (solidGround && !grappling && !startingGrapple) //resets your grapple when you touch the ground
		{
			grappleTimer = 0;
			//grapplingToWormTimer = 0;
			canGrapple = true;
		}

		if (grounded) //chooses your airborn strafe speed vs grounded turn speed
		{
			rb.angularDrag = angDragGrounded;
		}
		else
		{
			rb.angularDrag = angDragAirborn;
		}
		

		
		if (friction.getFriction()) //if grounded adds drag to the worm to emulate friction.
		{
			rb.drag = groundedDrag;
		}
		else
		{
			rb.drag = 0;
		}
		
		
		
		//caps the worms speed
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


		if (cameraFollow.getState() == 1)
		{
			grappleHit.SetActive(false);
			return;
		}


		if (jumpDown) //if jump is held down
		{
			transform.localScale = Vector3.Scale(initialScale, new Vector3(1, 0.75f, 1));
			crouching = true;
			snd.stopDescrunch();
			snd.playScrunch();
		}
		animator.SetBool("Crouching", crouching);
	
		

		//*
		RaycastHit hit;
		Vector3 hitPoint = Vector3.zero;

		if (!grappling && !dead) //shows the grapple indicator
		{
			LayerMask mask = 11001 << 8; //blocks and egg and wormshadow
			float distanceBetweenCenterAndButt = Vector3.Distance(buttPosition, transform.position);
			if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, range + distanceBetweenCenterAndButt, mask)
				&& canGrapple && !holdingObject && !grapplingToWorm)
			{

				if (hit.collider.gameObject.GetComponent<IceBlock>() != null)
				{
					grappleHit.SetActive(false);
					
				}
				else
				{
					grappleHit.SetActive(true);
					grappleHit.transform.position = new Vector3(0, hit.point.y, hit.point.z);
				}
			}
			else
			{
				grappleHit.SetActive(false);
			}
		}


		if (grappleHit.activeSelf || holdingObject) //changes the color of the worms tail based on if it can grapple
		{
			tail.sharedMaterial.Lerp(tailOn, tailOnExerted, (grappleTimer / maxGrappleTime) * 1.1f);
		}
		else
		{
			tail.sharedMaterial.Lerp(tailOff, tailOffExerted, (grappleTimer / maxGrappleTime) * 1.1f);
		}

		grappleHit.GetComponent<MeshRenderer>().sharedMaterial = tail.sharedMaterial; //makes the indicator the same color as the tail
		

		
		if (!grappling && (Input.GetMouseButtonDown(0) || (startingGrapple && frame == 2))) //starts the grapple (or at least grapple time)
		{
			if (holdingObject && !startingGrapple)
			{
				grabObject(null);
			}
			else
			{
				itsGrappleTime = true;
				LayerMask mask = 11001 << 8; //blocks and egg and wormshadow
				if (Physics.Raycast(buttPosition, transform.TransformDirection(Vector3.down), out hit, range, mask)) //grabbing object
				{
					if (hit.collider.gameObject.tag == "Egg")
					{
						grabObject(hit.collider.gameObject);
					}
					else if (hit.collider.gameObject.tag == "WormShadow" && !startingGrapple)
					{
						//Debug.Log("grappling to worm");
						//grappleTimer = 0;
						grappleToWorm(hit.collider.transform, grappleHit.transform.position);
						
					}
				}
			}
			
		}
		//*/

		if (itsGrappleTime) //adds some buffer to the grapple
		{
			howMuchLongerIsItGrappleTime += Time.deltaTime;
			if (howMuchLongerIsItGrappleTime > howLongDoesGrappleTimeLast || Input.GetMouseButtonUp(0))
			{
				itsGrappleTime = false;
				howMuchLongerIsItGrappleTime = 0;
			}
		}
		if (itsGrappleTime && canGrapple && grappleHit.activeSelf) //this is run on the first frame of the grapple. Sets everything up
		{
			grappling = true;
			itsGrappleTime = false;
			howMuchLongerIsItGrappleTime = 0;

			rsc.activate(Vector3.MoveTowards(grappleHit.transform.position, buttPosition, 0.1f));
			rsc.setTime(grappleTimer);
			ropeLength = Vector3.Distance(buttPosition, grappleHit.transform.position);

			spring.connectedBody = rsc.getCurrentPoint().GetComponent<Rigidbody>();
			spring.maxDistance = ropeLength;
			spring.connectedAnchor = Vector3.zero;
			//Debug.Log("Grapple spot 1");

			float maxPitch = 2;
			float minPitch = 0.4f;
			float distanceScaled = maxPitch - (maxPitch - minPitch) * Vector3.Distance(buttPosition, grappleHit.transform.position) / range;

			if (!startingGrapple)
			{
				snd.playGrapple(distanceScaled);
			}
		}



		
		if (grappling) //The code for the grapple that gets run every frame to update the rope position.
		{

			//IF THERES ANYTHING WRONG WITH GRAPPLE TO WORM LOOK HERE FIRST
			if (!startingGrapple && !grapplingToWorm)
			{
				grappleTimer = rsc.getTime();

				if (currentRopeLength != rsc.getLength())
				{
					currentRopeLength = rsc.getLength();

					if (rsc.getCurrentPoint() != null && rsc.getCurrentPoint().GetComponent<Rigidbody>() != null)
					{
						spring.connectedBody = rsc.getCurrentPoint().GetComponent<Rigidbody>();
						spring.maxDistance = ropeLength - currentRopeLength;
						spring.connectedAnchor = Vector3.zero;
					}
				}
			}
			else if(!startingGrapple)
			{
				grappleTimer += Time.deltaTime;
			}
		}
		

		
		if (holdingObject) //draws a line to the object
		{
			LineRenderer line = heldObject.GetComponent<LineRenderer>();
			line.sharedMaterial = tail.sharedMaterial;
			line.enabled = true;
			line.SetPosition(0, heldObject.transform.position);
			line.SetPosition(1, transform.position + transform.TransformDirection(Vector3.down) * transform.lossyScale.y); //absolute butt position
		}

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
					howMuchLongerIsItGrappleTime = 0;
					grappling = false;
					canGrapple = false;
					rsc.deactivate();
					if (!holdingObject)
					{
						spring.maxDistance = Mathf.Infinity;
					}

					animator.SetBool("Asleep", false);

					if (!holdingObject)
					{
						spring.maxDistance = Mathf.Infinity;
						
					}
					float startingJoltForce = 3000f;
					rb.AddForce(Vector3.up * startingJoltForce);
				}
			}
			
		}
		else if (grappling && !holdingObject && !DetectWin.hasWon && (!Input.GetMouseButton(0) 
			|| (!grapplingToWorm && rsc.getTime() > maxGrappleTime) || grappleTimer > maxGrappleTime)
			|| grapplingToWorm && Input.GetKeyDown(KeyCode.Escape)) //If left click is released
		{
			itsGrappleTime = false;
			howMuchLongerIsItGrappleTime = 0;
			grappling = false;
			//Debug.LogError("released");

			if (grappleTimer > maxGrappleTime)
			{
				canGrapple = false;
			}
			if (grapplingToWorm)
			{
				grapplingToWorm = false;
				wormGrapplePoint.GetComponent<WormGrapplePoint>().destroyLine();
				Destroy(wormGrapplePoint);
			}
			else
			{
				grappleTimer = rsc.getTime();
			}
			rsc.deactivate();
			spring.maxDistance = Mathf.Infinity;
			currentRopeLength = -1;
		}

		if (!solidGround && !startingGrapple)
		{
			floorIsLavaTimer += Time.deltaTime;
			if (floorIsLavaTimer > floorIsLavaTime)
			{
				floorIsLavaTimer = 0;
				AchievementManager.Achieve("ACH_FLOOR_IS_LAVA");
			}
		}
		else
		{
			floorIsLavaTimer = 0;
		}
		//Debug.Log(floorIsLavaTimer);

		setJumpInput(twod);
		frame++;
		
	}



	private void FixedUpdate()
	{		

		if (startingGrapple)
		{
			return;
		}

		if (PauseMenu.isPaused)
		{
			return;
		}
		if (DetectWin.hasWon)
		{
			rsc.setTime(0);
			return;
		}


		if (cameraFollow.getState() == 1 || dead || cameraFollow.getMouseFrozen())
		{
			rsc.deactivate();
			grappleHit.SetActive(false);
			return;
		}
	

		/*float hitThreshold = 20f;
		if (Mathf.Abs(rb.velocity.magnitude - prevVelocity.magnitude) > hitThreshold)
		{
			Debug.Log(prevVelocity.magnitude / 30f);
			snd.playHitRock(prevVelocity.magnitude / 30f);
			//Debug.Log("Hit" + Time.time);
		}*/

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
		if (!twod) //  && Mathf.Abs(Input.GetAxis("Vertical")) < 1f 
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

		if (crouching && !jumpHeld) //jump
		{
			//Debug.Log(Time.time);
			snd.playDescrunch();
			snd.stopScrunch();
			transform.localScale = initialScale;
			crouching = false;

			coyoteTimer = 0;
		}
		else if (coyoteTimer < coyoteTime)
		{
			coyoteTimer += Time.deltaTime;
		}
		if (grounded && coyoteTimer < coyoteTime) //the actual code that runs to jump
		{
			coyoteTimer = coyoteTime + 1f;
			transform.Translate(Vector3.up * 0.5f, Space.Self);

			
			if (solidGround)
			{
				rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 0, 999), rb.velocity.z);
				if (rb.velocity.y == 0)
				{
					//Debug.Log("cancelled velocity vertically");
				}
			}
			else if(Mathf.Sign(transform.TransformDirection(Vector3.up).z) != Mathf.Sign(rb.velocity.z)) //if the player is facing the opposite direction than they are moving
			{
				rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
				//Debug.Log("cancelled velocity horizontally");
			}


			//if(!(!solidGround && transform.eulerAngles.x)) 
			var wallClipRot = transform.eulerAngles.x; //To stop jumping straight up a wall now that CT is a thing
			if (wallClipRot < 180)
			{
				wallClipRot += 360;
			}
			//Debug.Log(Mathf.Abs(360 - wallClipRot));
			if (!solidGround && Mathf.Abs(360 - wallClipRot) < 3)
			{
				//Debug.LogError("cheating");
			}
			else
			{
				rb.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse); //THIS IS THE JUMP FORCE
				//Debug.Log("added force");
			}
			//Debug.Log("Jump!");


			//gets the material under the worm;
			RaycastHit hit;
			LayerMask mask = 101 << 8;

			Debug.DrawRay(buttPosition, transform.TransformDirection(Vector3.down * 4));
			float distanceBetweenCenterAndButt = Vector3.Distance(buttPosition, transform.position);
			var rayDistance = 1;
			while (rayDistance < 5)
			{
				if (Physics.SphereCast(transform.position, jumpTrigger.transform.lossyScale.x / 2, transform.TransformDirection(Vector3.down), out hit, rayDistance + distanceBetweenCenterAndButt, mask) ||
					Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.forward)), out hit, rayDistance, mask) ||
					Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.back)), out hit, rayDistance, mask))
				{
					rayDistance = 999;
					if (hit.collider.GetComponent<MeshRenderer>() != null)
					{
						Material mat = hit.collider.GetComponent<MeshRenderer>().sharedMaterial;
						//Debug.Log(mat);
						if (mat == null)
						{
						}
						else if (stoneMaterials.Contains(mat))
						{
							snd.playJump(Random.Range(0, 5));
						}
						/*else if (grassMaterials.Contains(mat))
						{
							snd.playGrass();
						}*/
						else if (woodMaterials.Contains(mat))
						{
							snd.playWood();
						}
						else
						{
							snd.playGrass();
						}

						
						if (hit.collider.gameObject.GetComponent<BreakableBlock>() != null)
						{
							GameObject other = hit.collider.gameObject;
							other.GetComponent<MeshRenderer>().enabled = false;
							other.transform.position += Vector3.up * 200;
							if (grappling)
							{
								Physics.SyncTransforms();
								LayerMask lm = 1 << 8;
								if (!Physics.CheckSphere(grappleHit.transform.position, grappleHit.transform.localScale.x / 2, lm))
								{
									itsGrappleTime = false;
									howMuchLongerIsItGrappleTime = 0;
									grappling = false;
									grappleTimer = rsc.getTime();
									if (grappleTimer > maxGrappleTime)
									{
										canGrapple = false;
									}
									rsc.deactivate();
									spring.maxDistance = Mathf.Infinity;
									currentRopeLength = -1;
								}
							}

						}
						
					}
				}
				else
				{
					rayDistance++;
				}
			}


			playJump();
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
			//snd.playSpeedWind(0);
		}

		prevAngVel = rb.angularVelocity;
		prevVelocity = rb.velocity;
		velocityPoint.position = transform.position + rb.velocity;

		//Debug.Log(rb.velocity + " " + (velocityPoint.position - transform.position));

	}

	public void grabObject(GameObject obj)
	{
		if (obj != null) //grabbing a new object
		{
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

	public void playExplosion()
	{
		explosion.Play();
	}

	public void playWinExplosion()
	{
		winExplosion.Play();
	}

	private void playSpeedLines()
	{
		speedLines.Play();
	}

	private void setJumpInput(bool iid)
	{
		if (startingGrapple)
		{
			return;
		}
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

		if (jumpHasBeenSet)
		{
			jumpDown = false;
			//Debug.Log("running on new stuff");
			if (Input.GetKey(KeyCode.Space))
			{
				jumpHasBeenSet = false;
				jumpUp = Input.GetKeyUp(KeyCode.Space);
				jumpDown = Input.GetKeyDown(KeyCode.Space);
				jumpHeld = Input.GetKey(KeyCode.Space);

			}

			if (MiniGame.spaceHeld)
			{
				jumpHeld = true;
			}
			else
			{
				jumpHasBeenSet = false;
				jumpUp = true;
				jumpHeld = false;
				Debug.Log("jump");
			}
		}
		else
		{
			jumpUp = Input.GetKeyUp(KeyCode.Space);
			jumpDown = Input.GetKeyDown(KeyCode.Space);
			jumpHeld = Input.GetKey(KeyCode.Space);
		}
		
	}
	public void setJumpDown()
	{
		if (FindObjectOfType<MiniGame>() != null)
		{
			
			if (MiniGame.spaceHeld)
			{
				jumpHasBeenSet = true;
				jumpDown = true;
				jumpHeld = true;
			}
		}
		else if (Input.GetKey(KeyCode.Space))
		{
			jumpDown = true;
		}
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
			eg.AddComponent<OuterWilds>();
			grabObject(eg);
			
		}
	}

	public bool getCrouching()
	{
		return crouching;
	}

	public void setDead(bool value)
	{
		dead = value;
		animator.SetBool("Dead", value);
		Debug.Log("dying");
	}
	public bool getDead()
	{
		return dead;
	}

	public void setArtificialWindSpeed(float spd)
	{
		artificialWindSpeed = spd;
	}

	public bool getGrappling()
	{
		return grappling;
	}

	public bool getFlippedModel()
	{
		return flippedModel;
	}

	public float getTailColor()
	{
		float val = 1f;
		if (!grappleHit.activeSelf)
		{
			val *= -1;
		}
		val *= (0.01f + (grappleTimer / maxGrappleTime) * 1.1f);
		//Debug.Log(val);
		return val;
	}

	private void grappleToWorm(Transform other, Vector3 position)
	{
		wormGrapplePoint = Instantiate(wormGrapplePointPrefab, position, Quaternion.identity, other);
		wormGrapplePoint.GetComponent<WormGrapplePoint>().setUp(other, transform, tail.sharedMaterial);
		itsGrappleTime = false;
		howMuchLongerIsItGrappleTime = 0;
		Rigidbody wgpRB = wormGrapplePoint.GetComponent<Rigidbody>();
		grappleHit.transform.position = Vector3.one * 9999;
		spring.connectedBody = wgpRB;
		wgpRB.isKinematic = true;
		spring.maxDistance = Vector3.Distance(position, buttPosition);
		grappling = true;
		grapplingToWorm = true;

		float maxPitch = 2;
		float minPitch = 0.4f;
		float distanceScaled = maxPitch - (maxPitch - minPitch) * Vector3.Distance(buttPosition, grappleHit.transform.position) / range;

		if (!startingGrapple)
		{
			snd.playGrapple(distanceScaled);
		}
	}

	public void changeSkin(int skinNumber)
	{
		//transform.parent.GetComponentInChildren<SkinSelector>().changeSkin(skinNumber);
		//Debug.Log(transform.parent.GetComponentInChildren<Animator>().gameObject);
		SkinSelector ss = transform.parent.GetComponentInChildren<SkinSelector>();
		model = Instantiate(ss.skinPrefabs[skinNumber], ss.transform);

		animator = model.GetComponent<Animator>();
		tail = model.transform.Find("Tail").GetComponent<MeshRenderer>();
		TailLightBank tlb = tail.GetComponent<TailLightBank>();
		tailOn = tlb.getFreshOn();
		tailOff = tlb.getFreshOff();
		tailOnExerted = tlb.getExertedOn();
		tailOffExerted = tlb.getExertedOff();
		//tailMat = tailOn;

		ss.updateModel(model, skinNumber);

		if (startingGrapple)
		{
			animator.SetBool("Asleep", true);
		}

		if (game != null)
		{
			game.setSkin(skinNumber);
		}
		PlayerPrefs.SetInt("Skin", skinNumber);

	}

}




