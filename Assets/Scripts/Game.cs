using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static bool fallFromRoof = true;
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
		timer += Time.deltaTime;
		if (timer > gravBoxDelay)
		{
			gravBoxEnabled = true;
		}

		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
			worm.GetComponent<Rigidbody>().velocity = new Vector3(0, wormVel.y, 0);
			worm.rotation = wormRot;
			worm.GetComponent<Rigidbody>().angularVelocity = wormAngVel;
		}
		else
		{
			wormVel = worm.GetComponent<Rigidbody>().velocity;
			wormRot = worm.rotation;
			wormAngVel = worm.GetComponent<Rigidbody>().angularVelocity;
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
			Debug.Log(rigAngle);
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

	}

	public void newScene()
	{
		
		timer = 0;
		gravBoxEnabled = false;
		currentScene = SceneManager.GetActiveScene().name;
		if (currentScene == "Attempt 2" && fallFromRoof)
		{
			worm.position = wormRespawnLocation;
		}
		fallFromRoof = true;
		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = fogDensity;
	}

}
