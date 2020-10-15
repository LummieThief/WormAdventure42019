using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour
{
	// A player unit is the thing that is controlled by the player
	private Transform worm;
	private Transform world;

	private Vector3 origin = new Vector3(500, 0, 500);

	private Vector3 velocity;

	private Vector3 lastOfficialPosition;

	private float minVelocity = 0.1f;
	private float minDisplacement = 0.1f;
	private float lastTime;

    // Start is called before the first frame update
    void Start()
    {
		/*
		
		*/
	}

    // Update is called once per frame
    void Update()
    {
		//This runs on all player units, not just the one I own.

		// OK SO BASICALLY, this object needs to handle PU positions and ONLY PU positions!
		// The clients can use this information LOCALLY! We dont need to do that for them.
		// Having all the positions in one spot will be very nice.

		// In order to send this object to the PU, I have to first set it to the worms position,
		// (so I will need a reference to the worm) and then I have to move it to the PU
		// based on the offset of the main world from the PU origin (so I will need a reference
		// to the world). Finally I need to rotate this object around the PU origin by the 
		// opposite of the worlds rotation. This objet will now be in the correct PU position.



		// Because I need references to the worm and the world, I will make sure I have them now.
		resetReferences();


		// Before we do any server stuff, lets do some local stuff.
		// Primarilly, we need to predict where we are supposed to be.
		// Lets just assume that we will keep moving in a straight line, so we will just
		// translate along our last known velocity.
		transform.Translate(velocity * Time.unscaledDeltaTime);
		//Debug.DrawRay(transform.position, velocity);
		
		


		// Now I need to check whether the player owns this object. If they dont I dont need to do
		// anything anymore.
		if (!hasAuthority) 
		{ 
			return; 
		}

		// Now I have to begin the process copying the worms position to the PU
		// The first step in this is moving this object to the position of the worm.
		transform.position = worm.position;
		transform.rotation = worm.rotation;

		// The next step is to move this object to the PU. We will do this through
		// the help of a method.
		sendToPU(transform);
		// This object is now in its correct PU position. Step one is done.

		// So far we have just updated the position on this local client,
		// so we have to tell the server to tell ALL the clients that this object is
		// in a different position now. We can do that in just one Command call.
		// Since we cant pass a transform as parameters, we will just pass the position
		// and rotation, which will be rebuilt into a transform (we dont care about scale).
		CmdSyncTransform(transform.position, transform.rotation);

		// Congrats! We have now sent this object to its correct PU position, and in just
		// a few milliseconds, all of the clients will be updated with our new position.
	}



	void resetReferences()
	{
		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if (world == null)
		{
			world = GameObject.FindGameObjectWithTag("OuterWildsWorld").transform;
		}
	}


	private void sendToPU(Transform trans)
	{
		// To send a transform to the PU, we first need to figure out how
		// far away from the PU origin the world is. We do this by subtracting
		// the world position from the PU origins position.
		Vector3 toPU = world.position - origin;

		// Now if we take the transform and subtract it by the offset from the PU,
		// the transform will go to its PU position IF THE MAIN WORLD HADNT BEEN ROTATED!
		trans.position -= toPU;

		// Since 99.9% of the time, the world will be rotated, we need to reverse this rotation,
		// because the PU is stationary. All we have to do is rotate the transform around the 
		// PU origin by the opposite of the amount the world is rotated.
		trans.RotateAround(origin, Vector3.up, -world.eulerAngles.y);

		// CONGRATS! In only 3 calculations, we have sent the transform to its PU position!
	}
	
	[Command]
	void CmdSyncTransform(Vector3 position, Quaternion rotation)
	{
		RpcSyncTransform(position, rotation);
	}

	[ClientRpc]
	void RpcSyncTransform(Vector3 position, Quaternion rotation)
	{
		// Because the client that owns the object has a better idea of where it is than
		// the server does, there is no need to tell it to move.
		if (hasAuthority)
		{
			return;
		}

		// This object should move to the position the server says it is in.
		transform.position = position;
		transform.rotation = rotation;


		// Now we should update our last known velocity so that we can predict our position.
		// To find velocity, we need a displacement and a delta time, and for each of those,
		// you need the current value and the last value. We store the last value whenever this
		// function is called so that we know we have accurate, server side positions to work with.
		// Lets calculate displacement and delta time.
		// (when working with "last" variables, you should update them after you use them)
		Vector3 displacement = position - lastOfficialPosition;
		lastOfficialPosition = position;
		float deltaTime = Time.time - lastTime;
		lastTime = Time.time;

		// Now since we are going to be dividing, we need to make sure we dont divide by zero.
		if (deltaTime > 0)
		{
			velocity = displacement / deltaTime;
		}
		// We now have our velocity, but if its really small, we can just say its zero to save cpu.
		if (velocity.magnitude < minVelocity)
		{
			velocity = Vector3.zero;
		}
	}
}





/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour
{
	// A player unit is the thing that is controlled by the player
	private float lerpValue = 0.8f;

	public GameObject mainBodyPrefab;
	private Transform mainBody;
	private Rigidbody mainBodyRB;
	//private MainBodyController mbc;
	private Transform worm;
	private Rigidbody wormRB;
	private Transform world;
	private bool meshDisabled = false;

	private Vector3 velocity = Vector3.zero;
	private float lastTime = 0.1f;

	private Vector3 lastPosition;
	private Vector3 mainBodyLastPosition;
	private Quaternion lastRotation;
	private float distanceDelta = 0.05f;
	private float rotationDelta = 0.1f;


	private Vector3 origin = new Vector3(500, 0, 500);

    // Start is called before the first frame update
    void Start()
    {
		mainBody = Instantiate(mainBodyPrefab, GameObject.FindGameObjectWithTag("OuterWildsWorld").transform).transform;
		mainBodyRB = mainBody.GetComponent<Rigidbody>();
		//mbc = mainBody.GetComponent<MainBodyController>();
	}

    // Update is called once per frame
    void Update()
    {
		//This runs on all player units, not just the one I own.
		resetReferences();

		if (hasAuthority)
		{
			//first sets this objects position to the worms position
			transform.position = worm.position;
			transform.rotation = worm.rotation;
			//then sends this position to the PU, which mimics sending the worm to the PU
			sendToPU(transform);
		}



		//mainBody.Translate(velocity * Time.unscaledDeltaTime);
		if (!hasAuthority)
		{

			if (Vector3.Distance(lastPosition, transform.position) < distanceDelta
				|| Quaternion.Angle(lastRotation, transform.rotation) < rotationDelta)
			{
				mainBodyRB.velocity = Vector3.zero;
				mainBodyRB.angularVelocity = Vector3.zero;
			}

			lastPosition = transform.position;
			lastRotation = transform.rotation;

			return;
		}

		

		if (!meshDisabled) //disables the mesh of the clients object
		{
			foreach (MeshRenderer mr in mainBody.GetComponentsInChildren<MeshRenderer>())
			{
				mr.enabled = false;
			}
			meshDisabled = true;
		}



		if (//Vector3.Distance(lastPosition, transform.position) > distanceDelta
			//|| Quaternion.Angle(lastRotation, transform.rotation) > rotationDelta)
			true)
		{
			CmdSyncTransform(transform.position, transform.rotation);
		}
		
		lastPosition = transform.position;
		lastRotation = transform.rotation;

		//RpcSyncTransform(transform.position, transform.rotation);
	}



	void resetReferences()
	{
		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if (world == null)
		{
			world = GameObject.FindGameObjectWithTag("OuterWildsWorld").transform;
		}
	}


	private void sendToPU(Transform trans)
	{
		Vector3 worldPos = world.position;
		Vector3 toPU = worldPos - origin;

		worldPos -= toPU;

		trans.position -= toPU;
		trans.RotateAround(worldPos, Vector3.up, -world.eulerAngles.y);
	}

	private void bringFromPU(Transform trans)
	{
		Vector3 worldPos = world.position;
		Vector3 toPU = worldPos - origin;

		worldPos -= toPU;

		trans.RotateAround(worldPos, Vector3.up, world.eulerAngles.y);
		trans.position += toPU;
	}

	
	[Command]
	void CmdSyncTransform(Vector3 position, Quaternion rotation)
	{
		RpcSyncTransform(position, rotation);
	}

	[ClientRpc]
	void RpcSyncTransform(Vector3 position, Quaternion rotation)
	{
		mainBody.position = transform.position;
		mainBody.rotation = transform.rotation;

		bringFromPU(mainBody);

		if (hasAuthority)
		{
			return;
		}

		transform.position = position;
		transform.rotation = rotation;
	}


	[Command]
	void CmdNegateVelocity()
	{
		RpcNegateVelocity();
	}

	[ClientRpc]
	void RpcNegateVelocity()
	{
		velocity = Vector3.zero;
	}
	


	private void OnDestroy()
	{
		if (mainBody != null)
		{
			Destroy(mainBody.gameObject);
		}
	}
}

 * 
 * */ //this is the old code before I cleaned it
