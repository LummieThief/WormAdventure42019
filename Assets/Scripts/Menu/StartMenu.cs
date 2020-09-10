using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class StartMenu : MonoBehaviour
{
	private FadeController fade;
	public GameObject startMenuUI;
	public GameObject playMenuUI;
	public GameObject mediumButton;
	public GameObject hardButton;
	public GameObject masterButton;
	public GameObject controlsMenuUI;
	public static bool isOpen = true;
	public GameObject playButton;
	public GameObject newGameButton;
	public float offsetDownWhenStarting;

	public GameObject optionsMenuUI;
	public GameObject creditsMenuUI;

	private Color purple = new Color(0.04764745f, 0, 0.1981132f);
	//private GameObject continueButton;

	private float timeBeforeFirstFade = 1;
	private float timeBeforeFirstFadeTimer = 0;
	// Start is called before the first frame update
	void Start()
	{
		isOpen = true;
		fade = FindObjectOfType<FadeController>();
		if (PlayerPrefs.HasKey("Arcade"))
		{
			int progress = PlayerPrefs.GetInt("Arcade");

			mediumButton.SetActive(progress >= 1);
			hardButton.SetActive(progress >= 2);
			masterButton.SetActive(progress >= 3);
			
		}
	}

	private void Update()
	{
		if (timeBeforeFirstFadeTimer > timeBeforeFirstFade && timeBeforeFirstFade > 0)
		{
			if (PlayerPrefs.GetInt("FromWhite") == 1)
			{
				fade.startFadeIn(1f, true, Color.white);
				Debug.Log("starting from white");
				PlayerPrefs.SetInt("FromWhite", 0);
			}
			else
			{
				fade.startFadeIn(1f, true);
			}
			timeBeforeFirstFade = -1;
			//Debug.Log("trying to fade");
		}
		else
		{
			timeBeforeFirstFadeTimer += Time.deltaTime;
		}
	}

	public void Continue()
	{
		playMenuUI.SetActive(true);
		startMenuUI.SetActive(false);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void OpenOptions()
	{
		optionsMenuUI.SetActive(true);
		startMenuUI.SetActive(false);
	}

	public void OpenCredits()
	{
		creditsMenuUI.SetActive(true);
		startMenuUI.SetActive(false);
	}

	public void OpenControls()
	{
		controlsMenuUI.SetActive(true);
		startMenuUI.SetActive(false);
	}
}
