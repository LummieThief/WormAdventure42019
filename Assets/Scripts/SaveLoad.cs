using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveLoad : MonoBehaviour
{
	public static bool initializing;
	public GameObject worm;
	public Transform ow;
	private Game game;
	private WormMove wm;

	private float timeBetweenSaves = 60;
	private float timer;

	private bool goNext;
	public bool resetOnPlay;

	private int numSaves;
	private bool loadNextFrame;
	private bool gamePaused;

	public static string path;
	private int marginOfError = 5;
	// Start is called before the first frame update
	void Awake()
    {
		path = Application.dataPath + "/Saves.txt";
		game = FindObjectOfType<Game>();
		wm = worm.GetComponent<WormMove>();

		if (resetOnPlay)
		{
			game.setLoaded(true);
			return;
		}

		if (!game.getLoaded())
		{
			startInitialization();

			if (File.Exists(path))
			{
				loadNextFrame = true;
			}
			else
			{
				endInitialization();
			}

			
		}
    }

    // Update is called once per frame
    void Update()
    {
		
		if (worm == null)
		{
			worm = FindObjectOfType<WormMove>().gameObject;
		}
		if (ow == null)
		{
			ow = GameObject.FindWithTag("OuterWildsWorld").transform;
		}

		if (PauseMenu.isPaused)
		{
			if (!gamePaused)
			{
				save();
			}
			gamePaused = true;
		}
		else
		{
			gamePaused = false;
		}

		timer += Time.deltaTime;
		if (timer > timeBetweenSaves)
		{
			save();
			timer = 0;
		}

	}


	private void OnApplicationQuit()
	{
		if (resetOnPlay)
		{
			ResetSave.resetSave();
		}
		else if (!StartMenu.isOpen)
		{
			save();
		}
	}

	private void LateUpdate()
	{
		if (loadNextFrame)
		{
			load();
			loadNextFrame = false;
		}


		if (goNext)
		{
			Debug.Log("SL new scene");
			SceneManager.LoadScene(getValueOf(path, "Scene"));
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
		game.setLoaded(true);
	}

	void save()
	{
		Rigidbody rb = worm.GetComponent<Rigidbody>();

		File.WriteAllText(path, "\n\n\nMODIFYING THIS DOCUMENT WILL ERASE YOUR SAVE DATA!\n\n\n");
		

		List<string> contents = new List<string>();
		contents.Add("\n" + "PositionX" + "," + worm.transform.position.x);
		contents.Add("\n" + "PositionY" + "," + worm.transform.position.y);
		contents.Add("\n" + "PositionZ" + "," + worm.transform.position.z);

		contents.Add("\n" + "RotationX" + "," + worm.transform.rotation.x);
		contents.Add("\n" + "RotationY" + "," + worm.transform.rotation.y);
		contents.Add("\n" + "RotationZ" + "," + worm.transform.rotation.z);
		contents.Add("\n" + "RotationW" + "," + worm.transform.rotation.w);

		

		contents.Add("\n" + "VelocityX" + "," + rb.velocity.x);
		contents.Add("\n" + "VelocityY" + "," + rb.velocity.y);
		contents.Add("\n" + "VelocityZ" + "," + rb.velocity.z);


		contents.Add("\n" + "WorldPositionX" + "," + ow.transform.position.x);
		contents.Add("\n" + "WorldPositionY" + "," + ow.transform.position.y);
		contents.Add("\n" + "WorldPositionZ" + "," + ow.transform.position.z);

		contents.Add("\n" + "WorldRotationX" + "," + ow.transform.rotation.x);
		contents.Add("\n" + "WorldRotationY" + "," + ow.transform.rotation.y);
		contents.Add("\n" + "WorldRotationZ" + "," + ow.transform.rotation.z);
		contents.Add("\n" + "WorldRotationW" + "," + ow.transform.rotation.w);

		contents.Add("\n" + "FogR" + "," + RenderSettings.fogColor.r);
		contents.Add("\n" + "FogG" + "," + RenderSettings.fogColor.g);
		contents.Add("\n" + "FogB" + "," + RenderSettings.fogColor.b);
		contents.Add("\n" + "FogA" + "," + RenderSettings.fogColor.a);
		contents.Add("\n" + "FogD" + "," + RenderSettings.fogDensity);

		contents.Add("\n" + "Egg" + "," + game.getHoldingObject());
		contents.Add("\n" + "Scene" + "," + SceneManager.GetActiveScene().name);
		contents.Add("\n" + "Time1" + "," + System.DateTime.Now.ToString("yyyyMMdd"));

		foreach (string s in contents)
		{
			File.AppendAllText(path, s);
		}

		var margin = marginOfError + 1;
		var l = 0;
		while (margin > marginOfError && l < 3)
		{
			File.AppendAllText(path, "\n" + "Time2" + "," + System.DateTime.Now.ToString("hhmmss.fff"));

			double time1 = double.Parse(getValueOf(path, "Time2"));
			double time2 = double.Parse(File.GetLastWriteTime(path).ToString("hhmmss.fff"));
			margin = (int)Mathf.Round(1000 * Mathf.Abs((float)(time1 - time2)));
			l++;
		}

		numSaves++;

		Debug.Log("Saved: save number " + numSaves);
		Debug.Log(margin);
		//Debug.Log("Margin of " + margin + ". " + time1 + " " + time2);
	}
	void load()
	{
		#region babycatcher
		float t1 = float.Parse(getValueOf(path, "Time2"));
		float t2 = float.Parse(File.GetLastWriteTime(path).ToString("hhmmss.fff"));
		int mar = (int)Mathf.Round(Mathf.Abs((1000 * (t1 - t2))));
		Debug.Log(mar);
		if (mar > marginOfError) //they might be cheating
		{
			//we have to check to make sure their computer isnt boonk
			var margin = marginOfError + 1;
			var l = 0;
			string newPath = Application.dataPath + "Tester.txt";
			while (margin > marginOfError && l < 10)
			{
				File.WriteAllText(newPath, "\n" + "Time2" + "," + System.DateTime.Now.ToString("hhmmss.fff"));

				float time1 = float.Parse(getValueOf(newPath, "Time2"));
				float time2 = float.Parse(File.GetLastWriteTime(newPath).ToString("hhmmss.fff"));
				margin = (int)Mathf.Round(Mathf.Abs((1000 * (time1 - time2))));
				Debug.Log(margin);
				l++;
			}
			File.Delete(newPath);
			if (l >= 5) //their computer is boonk
			{
				Debug.Log("computer is boonk");
			}
			else //their computer isnt boonk
			{

				Debug.Log("CAUGHT ONE!");
				ResetSave.resetSave();
				GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
				foreach (Persistant p in FindObjectsOfType<Persistant>())
				{
					GameObject g = p.gameObject;
					Destroy(g);
				}
				SceneManager.LoadScene("Attempt 2");
				return;
			}
		}
		#endregion
		string scene = getValueOf(path, "Scene");

		if (SceneManager.GetActiveScene().name != scene)
		{
			goNext = true;
			return;
		}

		Vector3 pos = new Vector3(float.Parse(getValueOf(path, "PositionX")),
									float.Parse(getValueOf(path, "PositionY")),
									float.Parse(getValueOf(path, "PositionZ")));

		Quaternion rot = new Quaternion(float.Parse(getValueOf(path, "RotationX")),
										float.Parse(getValueOf(path, "RotationY")),
										float.Parse(getValueOf(path, "RotationZ")),
										float.Parse(getValueOf(path, "RotationW")));

		Vector3 vel = new Vector3(float.Parse(getValueOf(path, "VelocityX")),
									float.Parse(getValueOf(path, "VelocityY")),
									float.Parse(getValueOf(path, "VelocityZ")));

		Vector3 worldPos = new Vector3(float.Parse(getValueOf(path, "WorldPositionX")),
										float.Parse(getValueOf(path, "WorldPositionY")),
										float.Parse(getValueOf(path, "WorldPositionZ")));

		Quaternion worldRot = new Quaternion(float.Parse(getValueOf(path, "WorldRotationX")),
											float.Parse(getValueOf(path, "WorldRotationY")),
											float.Parse(getValueOf(path, "WorldRotationZ")),
											float.Parse(getValueOf(path, "WorldRotationW")));

		Color fogColor = new Color(float.Parse(getValueOf(path, "FogR")),
									float.Parse(getValueOf(path, "FogG")),
									float.Parse(getValueOf(path, "FogB")),
									float.Parse(getValueOf(path, "FogA")));

		float fogDensity = float.Parse(getValueOf(path, "FogD"));

		bool hasEgg = getValueOf(path, "Egg") == "True";

		

		RenderSettings.fogColor = fogColor;
		RenderSettings.fogDensity = fogDensity;
		wm.load(pos, rot, vel, hasEgg);
		ow.position = worldPos;
		ow.rotation = worldRot;

		game.setLoaded(true);
		endInitialization();

		Debug.Log("Loaded");

	}

	void SetBool(string key, bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt(key, 1);
		}
		else
		{
			PlayerPrefs.SetInt(key, 0);
		}
	}

	string getValueOf(string path, string key)
	{
		string source = File.ReadAllText(path);

		int startIndex = source.IndexOf(",", source.IndexOf(key));
		if (startIndex == -1)
		{
			return "";
		}

		
		int endIndex = source.IndexOf("\n", startIndex);
		string data;
		if (endIndex == -1)
		{
			data = source.Substring(startIndex + 1);
		}
		else
		{
			data = source.Substring(startIndex + 1, endIndex - (startIndex + 1));
		}

		if (data.Contains("E-"))
		{
			data = "0";
		}
		//string data = source.Substring(startIndex);

		//Debug.Log("Key: " + key + " Data: {" + data + "}");

		return data;
	}
}
