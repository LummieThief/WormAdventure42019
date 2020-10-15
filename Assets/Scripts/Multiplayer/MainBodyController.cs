using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBodyController : MonoBehaviour
{
	private Transform worm;
	private Transform world;

	private bool crouching;
	private Vector3 origin = new Vector3(500, 0, 500);

	private Animator animator;

	public LineRenderer line;
	public Transform model;
	private Vector3 modelButtPosition;

	public Material tailOn;
	public Material tailOnExerted;
	public Material tailOff;
	public Material tailOffExerted;
	public MeshRenderer tail;
	public GameObject mainBodyColliderPrefab;
	private GameObject mainBodyCollider;
	private bool canCollide = true;

	private void Start()
	{
		mainBodyCollider = Instantiate(mainBodyColliderPrefab);
		mainBodyCollider.GetComponent<CapsuleCollider>().enabled = canCollide;
		DontDestroyOnLoad(mainBodyCollider);
		animator = GetComponentInChildren<Animator>();
		animator.SetBool("Multiplayer", true);
		//line = GetComponentInChildren<LineRenderer>();

		/*WormMove wormMove = FindObjectOfType<WormMove>();
		tailOn = wormMove.tailOn;
		tailOnExerted = wormMove.tailOnExerted;
		tailOff = wormMove.tailOff;
		tailOffExerted = wormMove.tailOffExerted;*/
		tail.sharedMaterial = new Material(tail.sharedMaterial);
	}
	private void LateUpdate()
	{
		// The main body is the one that shows up in the actual game, and our
		// job is to get it there!
		// To get the main body to where it's supposed to be, we will read
		// from its PU position and convert it to main world coordinates

		// To send it to the main world, we will need a reference to the worm
		// and the world.
		resetReferences();

		// Because we are in LateUpdate(), we know that our parent is already
		// In the PU. So if we go to (0, 0, 0) locally, we will be in our PU
		// position.
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		bringFromPU(transform);
		// Congrats! We are now in the main world!


		modelButtPosition = model.position + model.TransformDirection(Vector3.down) * model.lossyScale.y * 0.9f;
		//updateRopePositions();

		animator.SetBool("Crouching", crouching);

		mainBodyCollider.transform.position = model.position;
		mainBodyCollider.transform.rotation = model.rotation;
		mainBodyCollider.transform.localScale = model.localScale;

	}

	private void OnDestroy()
	{
		Destroy(mainBodyCollider);
	}

	private void bringFromPU(Transform trans)
	{
		// Now we need to rotate to match the main worlds rotation.
		trans.RotateAround(origin, Vector3.up, world.eulerAngles.y);

		// Then we have to move from the PU back to the main world
		Vector3 toPU = world.position - origin;
		trans.position += toPU;
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

	public void setCrouching(bool c)
	{
		crouching = c;
	}
	/*
	public void addGrapplePoint(Vector3 point)
	{
		GameObject emptyGO = new GameObject();
		emptyGO.transform.position = point;
		//bringFromPU(emptyGO.transform);

		if (line.positionCount == 0)
		{
			line.positionCount++;
		}

		line.positionCount++;
		line.SetPosition(line.positionCount - 2, emptyGO.transform.position);
		line.SetPosition(line.positionCount - 1, modelButtPosition); //sets the end of the rope to this objects position

		Destroy(emptyGO);
	}

	public void removeGrapplePoint()
	{
		line.positionCount--;
		if (line.positionCount == 1)
		{
			line.positionCount = 0;
		}
	}

	private void updateRopePositions()
	{
		if (line.positionCount > 0)
		{
			line.SetPosition(line.positionCount - 1, modelButtPosition); //sets the end of the rope to this objects position
		}
	}
	*/

	public void setTailColor(float col)
	{
		if (col > 0)
		{
			
			tail.sharedMaterial.Lerp(tailOn, tailOnExerted, col);
		}
		else
		{
			col *= -1;
			tail.sharedMaterial.Lerp(tailOff, tailOffExerted, col);
		}
		//Debug.Log(col);
		line.sharedMaterial = tail.sharedMaterial;
	}

	public void setColliderActive(bool active)
	{
		if (mainBodyCollider != null)
		{
			mainBodyCollider.GetComponent<CapsuleCollider>().enabled = active;
		}
		else
		{
			canCollide = active;
		}
		//Debug.Log("turned off colliders");
	}

	

}

