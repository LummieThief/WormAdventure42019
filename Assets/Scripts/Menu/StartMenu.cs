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
	public static bool isOpen = true;
	public GameObject playButton;
	public GameObject newGameButton;
	private CanvasGroup cg;
	public float offsetDownWhenStarting;

	public GameObject optionsMenuUI;
	//private GameObject continueButton;
	private Text playText;

	private bool fadingToScene;
	private float fadeTimer;
	private float timeToFade = 1;

	private float timeBeforeFirstFade = 1;
	private float timeBeforeFirstFadeTimer = 0;
	// Start is called before the first frame update
	void Start()
	{
		isOpen = true;
		fade = FindObjectOfType<FadeController>();
		cg = GetComponent<CanvasGroup>();
		playText = playButton.GetComponentInChildren<Text>();
		
		if (System.IO.File.Exists(SaveLoad.path))
		{
			playText.text = "CONTINUE";
			newGameButton.SetActive(true);
		}
		else
		{
			playText.text = "START";
			playButton.GetComponent<RectTransform>().localPosition += Vector3.down * offsetDownWhenStarting;
			newGameButton.SetActive(false);
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

		if (fadingToScene)
		{
			fadeTimer += Time.deltaTime;

			if (fadeTimer > timeToFade + 1)
			{
				ResetSave.resetSave();
				GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
				foreach (Persistant p in FindObjectsOfType<Persistant>())
				{
					GameObject g = p.gameObject;
					Destroy(g);
				}
				SceneManager.LoadScene("Attempt 2");
			}
		}
	}

	public void Continue()
	{
		StartCoroutine(fadeOutMenu(3f));
		//fadeOutMenu();
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void NewGame()
	{
		fadingToScene = true;
		fade.startFadeOut(timeToFade, true);
	}

	public void OpenOptions()
	{
		optionsMenuUI.SetActive(true);
		startMenuUI.SetActive(false);
	}

	IEnumerator fadeOutMenu(float fadePerSecond)
	{
		isOpen = false;
		while (cg.alpha > 0)
		{
			cg.alpha -= fadePerSecond * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		cg.alpha = 1;
		startMenuUI.SetActive(false);
	}
}
