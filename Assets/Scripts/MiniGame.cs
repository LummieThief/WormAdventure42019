using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGame : MonoBehaviour
{
	private bool showedLevel = false;
	private Animator announcementAnim;
	public Material easySkybox;
	public Material mediumSkybox;
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
		announcementAnim = FindObjectOfType<StartingCountdown>().GetComponent<Animator>();
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
			foreach (AudioSource audio in GetComponentsInChildren<AudioSource>())
			{
				Destroy(audio.gameObject);
			}
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.R) && !PauseMenu.isPaused && !FinishBox.finished)
		{
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
		if (!showedLevel)
		{
			showedLevel = true;
		}
		else
		{
			FindObjectOfType<StartingCamera>().setState(1);
			Debug.Log("Skip the cutscene");
		}
	}

	public bool getShowedLevel()
	{
		return showedLevel;
	}
}
