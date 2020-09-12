using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGame : MonoBehaviour
{
	private bool showedLevel = false;
	//private Animator announcementAnim;
	public Material easySkybox;
	public Material mediumSkybox;
	public Material hardSkybox;
	public Material masterSkybox;
	private SoundManager sm;

	//public GameObject musicFairy;
	// Start is called before the first frame update

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	void Awake()
    {
		showedLevel = false;
		//announcementAnim = FindObjectOfType<StartingCountdown>().GetComponent<Animator>();
		bool activate = false;
		foreach (SoundManager sound in FindObjectsOfType<SoundManager>())
		{
			if (sound.GetComponent<MiniGame>() == null)
			{
				activate = true;
			}
		}
		if (activate)
		{
			Destroy(GetComponent<SoundManager>());
			Destroy(GetComponent<AudioListener>());
			Destroy(GetComponent<ArcadeTimer>());
			Destroy(GetComponent<DeathCounter>());
			foreach (AudioSource audio in GetComponentsInChildren<AudioSource>())
			{
				Destroy(audio.gameObject);
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.R) && !PauseMenu.isPaused && !FinishBox.finished && StartingCountdown.goed)
		{
			StartingCamera sc = FindObjectOfType<StartingCamera>();
			if (sc == null || !sc.enabled)
			{
				FindObjectOfType<DeathCounter>().addDeath();
			}
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);

			
		}
		//ready = true;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		
		string path = SceneManager.GetActiveScene().path;
		if (path.Contains("Easy"))
		{
			RenderSettings.skybox = easySkybox;
		}
		else if (path.Contains("Medium"))
		{
			RenderSettings.skybox = mediumSkybox;
		}
		else if (path.Contains("Hard"))
		{
			RenderSettings.skybox = hardSkybox;
		}
		else if (path.Contains("Master"))
		{
			RenderSettings.skybox = masterSkybox;
		}
		if (!showedLevel)
		{
			showedLevel = true;
		}
		else
		{
			FindObjectOfType<StartingCamera>().setState(1);
			//Debug.Log("Skip the cutscene");
		}
	}

	public bool getShowedLevel()
	{
		return showedLevel;
	}
}
