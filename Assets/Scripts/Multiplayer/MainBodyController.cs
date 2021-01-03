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
	private Transform model;
	private Vector3 modelButtPosition;

	private Material tailOn;
	private Material tailOnExerted;
	private Material tailOff;
	private Material tailOffExerted;
	public MeshRenderer tail;

	private Material currentTailMaterial;
	//public GameObject mainBodyColliderPrefab;
	public GameObject mainBodyCollider;
	private bool canCollide = true;
	private bool flippedModel;

	private Transform modelGoesHere;

	private void Awake()
	{
		//mainBodyCollider = Instantiate(mainBodyColliderPrefab);
		//mainBodyCollider.GetComponent<PhotonOwner>().id = 
		modelGoesHere = transform.Find("Model Goes Here");
		model = modelGoesHere.GetChild(0);
		mainBodyCollider.GetComponent<CapsuleCollider>().enabled = canCollide;
		mainBodyCollider.transform.parent = null;
		DontDestroyOnLoad(mainBodyCollider);
		animator = GetComponentInChildren<Animator>();
		animator.SetBool("Multiplayer", true);
		//line = GetComponentInChildren<LineRenderer>();

		TailLightBank tlb = model.GetComponentInChildren<TailLightBank>();
		tailOn = tlb.getFreshOn();
		tailOnExerted = tlb.getExertedOn();
		tailOff = tlb.getFreshOff();
		tailOffExerted = tlb.getExertedOff();
		tail = tlb.GetComponent<MeshRenderer>();
		currentTailMaterial = new Material(tail.sharedMaterial);
		tail.sharedMaterial = currentTailMaterial;
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
		Destroy(currentTailMaterial);
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
			Debug.Log("main body collider is now set to " + active);
		}
		else
		{
			canCollide = active;
		}
		//Debug.Log("turned off colliders");
	}

	public Vector3 getModelScale()
	{
		return model.transform.lossyScale;
	}

	public void updateModel(GameObject newModel)
	{
		GameObject oldModel = model.gameObject;
		oldModel.transform.parent = null;
		Destroy(currentTailMaterial);
		model = Instantiate(newModel, modelGoesHere).transform;
		model.localRotation = Quaternion.identity;

		animator = GetComponentInChildren<Animator>();
		animator.SetBool("Multiplayer", true);

		TailLightBank tlb = model.GetComponentInChildren<TailLightBank>();
		tailOn = tlb.getFreshOn();
		tailOnExerted = tlb.getExertedOn();
		tailOff = tlb.getFreshOff();
		tailOffExerted = tlb.getExertedOff();
		tail = tlb.GetComponent<MeshRenderer>();
		currentTailMaterial = new Material(tail.sharedMaterial);
		tail.sharedMaterial = currentTailMaterial;

		Destroy(oldModel);
	}

	public void setFlippedModel(bool flipped)
	{
		flippedModel = flipped;
		if (flipped)
		{
			modelGoesHere.localRotation = Quaternion.Euler(0, 180, 0);
		}
		else
		{
			modelGoesHere.localRotation = Quaternion.Euler(0, 0, 0);
		}
	}



}

