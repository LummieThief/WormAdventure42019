using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static bool gravBoxEnabled = false;

	private float timer = 0;
	private float gravBoxDelay = 7;
	private Transform worm;
	private Vector3 wormVel;
	private Quaternion wormRot;
	private Vector3 wormAngVel;

	private Transform rig;
	private Vector3 rigAngle;

	public Vector3 wormRespawnLocation;
	private Color fogColor;
	private float fogDensity;

	private string currentScene = "Attempt 2";

	public GameObject egg;
	private bool holdingObject;

	private bool startingGrapple = true;

	private float worldRotation;

	private bool loaded = false;

	// Start is called before the first frame update
	void Start()
    {
		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
		}
		if (rig == null)
		{
			rig = GameObject.FindGameObjectWithTag("MainCamera").transform;
		}
	}

	private void Update()
	{
		//Debug.Log(timer);
		//Debug.Log(gravBoxEnabled);
		timer += Time.deltaTime;
		if (timer > gravBoxDelay && timer < gravBoxDelay + 2)
		{
			gravBoxEnabled = true;
		}

		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
			worm.GetComponent<Rigidbody>().velocity = new Vector3(0, wormVel.y, 0);
			worm.rotation = wormRot;
			worm.GetComponent<Rigidbody>().angularVelocity = wormAngVel;

			if (SceneManager.GetActiveScene().name == "Attempt 2")
			{
				worm.position = wormRespawnLocation;
			}

			if (holdingObject)
			{
				var eg = Instantiate(egg, worm.position, Quaternion.identity);
				worm.GetComponent<WormMove>().grabObject(eg);
				//worm.
			}
		}
		else
		{
			wormVel = worm.GetComponent<Rigidbody>().velocity;
			wormRot = worm.rotation;
			wormAngVel = worm.GetComponent<Rigidbody>().angularVelocity;
			holdingObject = worm.GetComponent<WormMove>().getHolding();
		}

		if (rig == null)
		{
			rig = GameObject.FindGameObjectWithTag("MainCamera").transform;
			var mouseY = rigAngle.x;
			if (mouseY > 180)
			{
				mouseY -= 360;
			}

			rig.GetComponent<CameraFollow>().setY(mouseY);
			rig.eulerAngles = rigAngle;
			//Debug.Log(rigAngle);
		}
		else
		{
			rigAngle = rig.eulerAngles;
		}


		if (currentScene != SceneManager.GetActiveScene().name)
		{
			newScene();
		}
		else
		{
			fogColor = RenderSettings.fogColor;
			fogDensity = RenderSettings.fogDensity;
		}
		//Debug.Log(fogDensity);
		if (startingGrapple)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		worldRotation = transform.eulerAngles.y;
		//Debug.Log(loaded);
	}

	public void newScene()
	{
		Debug.Log("new scene");
		timer = 0;
		gravBoxEnabled = false;
		currentScene = SceneManager.GetActiveScene().name;
		RenderSettings.fog = true;
		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = fogDensity;
		Debug.Log(fogDensity);
		
	}


	public bool getStartingGrapple()
	{
		return startingGrapple;
	}
	public bool setStartingGrapple(bool value)
	{
		//Debug.Log("set grapple");
		startingGrapple = value;
		return value;
	}

	public float getWorldRotation()
	{
		return worldRotation;
	}

	public void setLoaded(bool val)
	{
		loaded = val;
	}

	public bool getLoaded()
	{
		return loaded;
	}

	public bool getHoldingObject()
	{
		return holdingObject;
	}

	public void resetGameData()
	{
		worm = GameObject.FindGameObjectWithTag("Player").transform;
		rig = GameObject.FindGameObjectWithTag("MainCamera").transform;
		currentScene = "Attempt 2";
		startingGrapple = true;
		loaded = false;
		timer = 0;
		wormVel = Vector3.zero;
		wormRot = Quaternion.identity;
		wormAngVel = Vector3.zero;
		rigAngle = Vector3.zero;
	}
}
