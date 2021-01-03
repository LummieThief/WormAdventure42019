using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class SaveLoad : MonoBehaviour
{
	public static bool initializing;
	private GameObject worm;
	public Transform ow;
	private Game game;
	private WormMove wm;
	private Timer playTimer;

	private float timeBetweenSaves = 60;
	private float timer;

	private bool goNext;
	public bool resetOnPlay;

	private int numSaves;
	private bool loadNextFrame;
	private bool gamePaused;

	public static string path;
	public static string gifterBuffer;
	// Start is called before the first frame update
	void Awake()
    {
		worm = FindObjectOfType<WormMove>().gameObject;
		playTimer = FindObjectOfType<Timer>();
		path = Application.dataPath + "/saves.txt";
		game = FindObjectOfType<Game>();
		wm = worm.GetComponent<WormMove>();

		if (game != null)
		{
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
		if (timer > timeBetweenSaves && !StartMenu.isOpen)
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

		string skins = getValueOf(path, "Skins");
		File.WriteAllText(path, "Saves");
		

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



		if (game == null)
		{
			contents.Add("\n" + "Egg" + "," + "False");
		}
		else
		{
			contents.Add("\n" + "Egg" + "," + game.getHoldingObject());
		}
		contents.Add("\n" + "Scene" + "," + SceneManager.GetActiveScene().name);
		contents.Add("\n" + "Skins" + "," + skins + gifterBuffer);
		gifterBuffer = "";
		contents.Add("\n" + "Time" + "," + toSeconds(System.DateTime.Now));


		foreach (string s in contents)
		{
			File.AppendAllText(path, s);
		}
		numSaves++;

		PlayerPrefs.SetFloat("PlayTime", playTimer.getTime());

		//Debug.Log("Saved: save number " + numSaves);
		
	}
	void load()
	{
		#region babycatcher
		int lastWrite = toSeconds(File.GetLastWriteTime(path));
		int lastLog = int.Parse(getValueOf(path, "Time"));
		int elapsed = lastWrite - lastLog;

		if (!(elapsed == 0 || elapsed == 1 || elapsed == -83999))
		{
			PlayerPrefs.SetInt("unity.player_session_log", Random.Range(0, 499999) * 2);
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

		playTimer.setTime(PlayerPrefs.GetFloat("PlayTime"));
		endInitialization();

		Debug.Log("Loaded");

	}
	public string getValueOf(string key)
	{
		return getValueOf(path, key);
	}
	string getValueOf(string path, string key)
	{
		if (!File.Exists(path))
			return "";

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
	private int toSeconds(System.DateTime date)
	{
		int secInMinute = 60;
		int secInHour = secInMinute * 60;

		return date.Hour * secInHour + date.Minute * secInMinute + date.Second;
	}


}
