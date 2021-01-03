using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PhotonPlayerController : MonoBehaviour, IPunObservable
{
	// A player unit is the thing that is controlled by the player
	private PhotonView pv;

	public GameObject mainBody;

	private Transform worm;
	private Transform wormVelocityPoint;
	private Transform velocityPoint;
	private Transform world;

	private Quaternion lastOfficialRotation;
	public Vector3 velocity;
	private float minVelocity = 0.5f;
	private float rotationSpeed = 180f;

	private Vector3 origin = new Vector3(500, 0, 500);

	private int scene = 0;
	private bool rendering = true;
	private bool dontRender = false;
	private bool stopped = false;
	private bool crouching = false;
	private WormMove wormMove;

	private int sendRate = 10;
	private int serializationRate = 10;

	private MainBodyController mbc;

	private float deci = 0.01f;
	private Stack<Vector3> grapplePoints;

	public LineRenderer line;
	public PhotonOwner po;
	public GameObject wormGrappleLinePrefab;

	private int skin = 0;
	private SkinSelector ss;
	private bool modelFlipped;

	// Start is called before the first frame update
	void Awake()
	{
		pv = GetComponent<PhotonView>();
		Debug.Log("my view is " + pv.ViewID);
		po.id = pv.ViewID;
		mbc = GetComponentInChildren<MainBodyController>();
		if (pv.IsMine)
		{
			setRenderers(false);
			dontRender = true;
			scene = SceneManager.GetActiveScene().buildIndex;
			pv.RPC("RPC_sceneUpdate", RpcTarget.AllBuffered, scene);
			gameObject.tag = "MyPlayer";
		}

		velocityPoint = GetComponentInChildren<VelocityPoint>().transform;
		DontDestroyOnLoad(gameObject);

		PhotonNetwork.SendRate = sendRate;
		PhotonNetwork.SerializationRate = serializationRate;


		line.transform.parent = null;
		DontDestroyOnLoad(line.gameObject);
		grapplePoints = new Stack<Vector3>();
	}

	// Update is called once per frame
	void Update()
	{
		/*
		foreach (PhotonView _pv in PhotonNetwork.PhotonViewCollection)
		{
			Debug.Log(_pv.ViewID);
		}
		*/

		resetReferences();
		checkSceneUpdate();
		checkCrouchUpdate();
		checkPaused();
		checkSkinChange();
		checkModelFlip();
		//line.transform.position = origin;
		//line.transform.rotation = Quaternion.identity;



		if (!crouching)
		{
			//mainBody.transform.localScale = new Vector3(1, 1, 1);
			mbc.setCrouching(false);
		}
		else
		{
			//mainBody.transform.localScale = new Vector3(1, (1f / 1.777f) * 1.33f, 1);
			mbc.setCrouching(true);
		}


		if (rendering && !dontRender && SceneManager.GetActiveScene().buildIndex != scene)
		{
			setRenderers(false);
		}
		else if (!rendering && !dontRender && SceneManager.GetActiveScene().buildIndex == scene)
		{
			setRenderers(true);
		}

		//Debug.Log(Truncate(1.23456789f, 0.01f));

		if (pv.IsMine)
		{
			transform.position = worm.position;
			transform.rotation = worm.rotation;
			velocityPoint.localPosition = wormVelocityPoint.localPosition;
			sendToPU(transform);

			/*
			if (!stopped && velocity.magnitude < minVelocity)
			{
				stopped = true;
			}
			else if (stopped && velocity.magnitude > minVelocity)
			{
				stopped = false;
			}
			*/

		}
		else
		{

			transform.position += velocity * Time.deltaTime * 0.75f;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, lastOfficialRotation, rotationSpeed * Time.deltaTime);
		}

		updateRopePositions();
	}

	private void OnDestroy()
	{
		if (line != null)
		{
			Destroy(line.gameObject);
		}
	}



	void resetReferences()
	{
		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if (wormVelocityPoint == null)
		{
			wormVelocityPoint = worm.GetComponentInChildren<VelocityPoint>().transform;
		}
		if (world == null)
		{
			world = GameObject.FindGameObjectWithTag("OuterWildsWorld").transform;
		}
		if (wormMove == null)
		{
			wormMove = FindObjectOfType<WormMove>();
		}
		if (ss == null)
		{
			ss = FindObjectOfType<SkinSelector>();
		}
	}


	private void setRenderers(bool enabled)
	{
		foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
		{
			mr.enabled = enabled;
		}
		line.enabled = enabled;
		mbc.setColliderActive(enabled);
		rendering = enabled;
		//Debug.Log("Rendering is now set to " + enabled);
	}

	void checkSceneUpdate()
	{
		if (pv.IsMine)
		{
			int newScene = SceneManager.GetActiveScene().buildIndex;
			if (newScene != scene)
			{
				scene = newScene;
				pv.RPC("RPC_sceneUpdate", RpcTarget.AllBuffered, scene);
			}
		}
	}

	[PunRPC]
	void RPC_sceneUpdate(int s)
	{
		scene = s;
	}

	void checkModelFlip()
	{
		if (pv.IsMine)
		{
			if (modelFlipped && !wormMove.getFlippedModel())
			{
				modelFlipped = false;
				pv.RPC("RPC_modelFlip", RpcTarget.AllBuffered, modelFlipped);
			}
			else if (!modelFlipped && wormMove.getFlippedModel())
			{
				modelFlipped = true;
				pv.RPC("RPC_modelFlip", RpcTarget.AllBuffered, modelFlipped);
			}
		}

	}

	[PunRPC]
	void RPC_modelFlip(bool flipped)
	{
		mbc.setFlippedModel(flipped);
	}


	void checkCrouchUpdate()
	{
		if (pv.IsMine)
		{
			if (!crouching && wormMove.getCrouching())
			{
				crouching = true;
				pv.RPC("RPC_crouchUpdate", RpcTarget.AllBuffered, true);
			}
			else if (crouching && !wormMove.getCrouching())
			{
				crouching = false;
				pv.RPC("RPC_crouchUpdate", RpcTarget.AllBuffered, false);
			}
		}
	}
	[PunRPC]
	void RPC_crouchUpdate(bool c)
	{
		crouching = c;
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


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{

			Vector3 shortPosition = Truncate(transform.position, deci);
			Quaternion shortRotation = Truncate(transform.rotation, deci);
			Vector3 shortVelocityPoint = Truncate(velocityPoint.position * Time.timeScale, deci);
			float shortColor = Truncate(wormMove.getTailColor(), deci);

			stream.SendNext(shortPosition);
			stream.SendNext(shortRotation);
			stream.SendNext(shortVelocityPoint);
			stream.SendNext(shortColor);
			//stream.SendNext(Truncate(yScale, deci));

		}
		else
		{
			Vector3 newOfficialPosition = (Vector3)stream.ReceiveNext();
			Quaternion newOfficialRotation = (Quaternion)stream.ReceiveNext();
			Vector3 newVelocityPoint = (Vector3)stream.ReceiveNext();
			float newColor = (float)stream.ReceiveNext();

			if (newVelocityPoint == Vector3.zero)
			{
				velocity = Vector3.zero;
			}
			else
			{
				velocity = newVelocityPoint - newOfficialPosition;
				if (velocity.magnitude < minVelocity)
				{
					velocity = Vector3.zero;
				}
			}



			transform.position = newOfficialPosition;
			transform.rotation = lastOfficialRotation;
			lastOfficialRotation = newOfficialRotation;
			interpretColor(newColor);
			//mainBody.transform.localScale = new Vector3(1, (1f/1.777f) * newYScale, 1);




		}
	}

	private float Truncate(float value, float dec)
	{
		return value - (value % dec);
	}
	private Vector3 Truncate(Vector3 value, float dec)
	{
		return new Vector3(Truncate(value.x, dec),
							Truncate(value.y, dec),
							Truncate(value.z, dec));
	}
	private Quaternion Truncate(Quaternion value, float dec)
	{
		return new Quaternion(Truncate(value.x, dec),
							Truncate(value.y, dec),
							Truncate(value.z, dec),
							Truncate(value.w, dec));
	}


	public void addGrapplePoint(Transform point)
	{
		Debug.Log("new grapple point");
		sendToPU(point);

		Vector3 truncatedPoint = Truncate(point.position, deci);
		Destroy(point.gameObject);
		pv.RPC("RPC_addGrapplePoint", RpcTarget.AllBuffered, truncatedPoint);
	}
	[PunRPC]
	void RPC_addGrapplePoint(Vector3 point)
	{
		if (pv.IsMine)
		{
			return;
		}



		//mbc.addGrapplePoint(point);
		if (line.positionCount == 0)
		{
			line.positionCount++;
		}

		line.positionCount++;
		line.SetPosition(line.positionCount - 2, point - origin);
		Vector3 buttPosition = transform.position + transform.TransformDirection(Vector3.down) * mbc.getModelScale().y * 0.9f;
		line.SetPosition(line.positionCount - 1,  buttPosition - origin);

	}

	public void removeGrapplePoint()
	{
		//Debug.Log("removed grapple point");

		pv.RPC("RPC_removeGrapplePoint", RpcTarget.AllBuffered);
	}
	[PunRPC]
	void RPC_removeGrapplePoint()
	{
		if (pv.IsMine)
		{
			return;
		}

		//mbc.removeGrapplePoint();

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
			Vector3 buttPosition = transform.position + transform.TransformDirection(Vector3.down) * mbc.getModelScale().y * 0.9f;
			line.SetPosition(line.positionCount - 1, buttPosition - origin); //sets the end of the rope to this objects position
		}

		if (world != null)
		{
			line.transform.position = world.position;
			line.transform.rotation = world.rotation;
		}
	}

	private void interpretColor(float col)
	{
		if (mbc != null)
		{
			mbc.setTailColor(col);
		}
	}

	public void grappleToWorm(int connectedId, Vector3 localPos)
	{
		if (scene == SceneManager.GetActiveScene().buildIndex)
		{
			pv.RPC("RPC_grappleToWorm", RpcTarget.AllBuffered, pv.ViewID, connectedId, Truncate(localPos, deci));
		}
	}
	[PunRPC]
	private void RPC_grappleToWorm(int id1, int id2, Vector3 localPos)
	{
		if (pv.IsMine)
		{
			return;
		}

		//id1 is the worm that shot the grapple
		//id2 is the worm that got hit by the grapple
		GameObject wormLine = Instantiate(wormGrappleLinePrefab);
		wormLine.GetComponent<WormGrappleLine>().setUp(PhotonNetwork.GetPhotonView(id1).transform, PhotonNetwork.GetPhotonView(id2).transform, id1, localPos);
		//Debug.Log(id1 + " " + id2);
	}
	public void destroyLine()
	{
		pv.RPC("RPC_destroyLine", RpcTarget.AllBuffered, pv.ViewID);
	}
	[PunRPC]
	private void RPC_destroyLine(int id)
	{
		foreach (WormGrappleLine wgl in FindObjectsOfType<WormGrappleLine>())
		{
			if (wgl.getMainId() == id)
			{
				Destroy(wgl.gameObject);
			}
		}
	}

	private void checkPaused()
	{
		if (pv.IsMine)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				//pv.RPC("RPC_checkPaused", RpcTarget.AllBuffered);
			}
		}
	}
	[PunRPC]
	private void RPC_checkPaused()
	{
		velocity = Vector3.zero;
		Debug.Log("paused");
	}


	public void setSkin(int skinIndex)
	{

		pv.RPC("RPC_setSkin", RpcTarget.AllBuffered, skinIndex);
	}
	[PunRPC]
	private void RPC_setSkin(int index)
	{
		SkinSelector ss = FindObjectOfType<SkinSelector>();
		skin = ss.selectedSkin;
		mbc.updateModel(ss.skinPrefabs[index]);

		if (pv.IsMine || scene != SceneManager.GetActiveScene().buildIndex)
		{
			setRenderers(false);
		}
	}

	private void checkSkinChange()
	{
		if (!pv.IsMine)
			return;

		if (skin != ss.selectedSkin)
		{
			setSkin(ss.selectedSkin);

		}
	}




	/*
	
	*/

}
