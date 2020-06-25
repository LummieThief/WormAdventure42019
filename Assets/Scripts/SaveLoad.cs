using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
	public static bool initializing;
	public GameObject worm;
	public Transform ow;
	private Game game;
	private WormMove wm;

	private float timeBetweenSaves = 1;
	private float timer;

	public bool resetOnPlay;
	// Start is called before the first frame update
	void Start()
    {

		game = FindObjectOfType<Game>();
		wm = worm.GetComponent<WormMove>();

		if (resetOnPlay)
		{
			return;
		}
		startInitialization();

		if (PlayerPrefs.HasKey("PositionX"))
		{
			load();
		}
		else
		{
			endInitialization();
		}
    }

    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime;
		if (timer > timeBetweenSaves)
		{
			save();
			timer = 0;
		}
    }

	void startInitialization()
	{
		//Time.timeScale = 0;
		initializing = true;
	}

	void endInitialization()
	{
		//Time.timeScale = 1;
		initializing = false;
	}

	void save()
	{


		PlayerPrefs.SetFloat("PositionX", worm.transform.position.x);
		PlayerPrefs.SetFloat("PositionY", worm.transform.position.y);
		PlayerPrefs.SetFloat("PositionZ", worm.transform.position.z);

		PlayerPrefs.SetFloat("RotationX", worm.transform.rotation.x);
		PlayerPrefs.SetFloat("RotationY", worm.transform.rotation.y);
		PlayerPrefs.SetFloat("RotationZ", worm.transform.rotation.z);
		PlayerPrefs.SetFloat("RotationW", worm.transform.rotation.w);

		Rigidbody rb = worm.GetComponent<Rigidbody>();
		PlayerPrefs.SetFloat("VelocityX", rb.velocity.x);
		PlayerPrefs.SetFloat("VelocityY", rb.velocity.y);
		PlayerPrefs.SetFloat("VelocityZ", rb.velocity.z);


		PlayerPrefs.SetFloat("WorldPositionX", ow.transform.position.x);
		PlayerPrefs.SetFloat("WorldPositionY", ow.transform.position.y);
		PlayerPrefs.SetFloat("WorldPositionZ", ow.transform.position.z);

		PlayerPrefs.SetFloat("WorldRotationX", ow.transform.rotation.x);
		PlayerPrefs.SetFloat("WorldRotationY", ow.transform.rotation.y);
		PlayerPrefs.SetFloat("WorldRotationZ", ow.transform.rotation.z);
		PlayerPrefs.SetFloat("WorldRotationW", ow.transform.rotation.w);


	}

	void load()
	{
		Vector3 pos = new Vector3(PlayerPrefs.GetFloat("PositionX"),
									PlayerPrefs.GetFloat("PositionY"),
									PlayerPrefs.GetFloat("PositionZ"));

		Quaternion rot = new Quaternion(PlayerPrefs.GetFloat("RotationX"),
										PlayerPrefs.GetFloat("RotationY"),
										PlayerPrefs.GetFloat("RotationZ"),
										PlayerPrefs.GetFloat("RotationW"));

		Vector3 vel = new Vector3(PlayerPrefs.GetFloat("VelocityX"),
								PlayerPrefs.GetFloat("VelocityY"),
								PlayerPrefs.GetFloat("VelocityZ"));


		Vector3 worldPos = new Vector3(PlayerPrefs.GetFloat("WorldPositionX"),
									PlayerPrefs.GetFloat("WorldPositionY"),
									PlayerPrefs.GetFloat("WorldPositionZ"));

		Quaternion worldRot = new Quaternion(PlayerPrefs.GetFloat("WorldRotationX"),
										PlayerPrefs.GetFloat("WorldRotationY"),
										PlayerPrefs.GetFloat("WorldRotationZ"),
										PlayerPrefs.GetFloat("WorldRotationW"));

		wm.load(pos, rot, vel);
		ow.position = worldPos;
		ow.rotation = worldRot;
	
		
		endInitialization();

	}
}
