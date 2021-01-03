using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class PlayMenu : MonoBehaviour
{
	private FadeController fade;
	public GameObject startMenuUI;
	public GameObject playMenuUI;
	public GameObject arcadeMenuUI;
	//public static bool isOpen = true;
	public GameObject playButton;
	public GameObject newGameButton;

	private CanvasGroup cg;
	public float offsetDownWhenStarting;

	//private GameObject continueButton;
	private Text playText;

	private bool fadingToScene;
	private bool goingToScene;
	private string scene;
	private float fadeTimer;
	private float timeToFade = 1;
	private bool fadeObjectRunning = false;


	private float timer;
	private Vector3 startingScale;
	private float scaleSpeed = 7f;
	private float maxScale = 0.1f;

	public GameObject screenBlocker;

	private SoundManager sm;
	// Start is called before the first frame update
	void Start()
	{
		fade = FindObjectOfType<FadeController>();
		cg = GetComponent<CanvasGroup>();
		playText = playButton.GetComponentInChildren<Text>();

		if (System.IO.File.Exists(SaveLoad.path) || (PlayerPrefs.HasKey("Continue") && PlayerPrefs.GetInt("Continue") == 1))
		{
			playText.text = " CONTINUE";
			playText.alignment = TextAnchor.MiddleLeft;
			newGameButton.SetActive(true);
		}
		else
		{
			playText.text = "START";
			playText.alignment = TextAnchor.MiddleCenter;
			playButton.GetComponent<RectTransform>().localPosition += Vector3.down * offsetDownWhenStarting;
			newGameButton.SetActive(false);
		}
		startingScale = playButton.transform.localScale;
	

		sm = FindObjectOfType<SoundManager>();

	}

	private void Update()
	{
		if (playMenuUI.activeSelf && Input.GetKeyDown(KeyCode.P))
		{
			Continue();
		}

		timer += scaleSpeed * Time.deltaTime;
		playButton.transform.localScale = startingScale + Vector3.one * maxScale * (1f + Mathf.Sin(timer) / 2f);


		if (fadingToScene)
		{
			screenBlocker.SetActive(true);
			fadeTimer += Time.deltaTime;

			if (fadeTimer > timeToFade + 1)
			{
				if (scene == "Attempt 2")
				{
					ResetSave.resetSave();
				}
				GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
				foreach (Persistant p in FindObjectsOfType<Persistant>())
				{
					GameObject g = p.gameObject;
					Destroy(g);
				}
				PlayerPrefs.SetInt("Continue", 0);
				SceneManager.LoadScene(scene);
			}
		}
		else if (goingToScene && !fadeObjectRunning)
		{
			GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
			foreach (Persistant p in FindObjectsOfType<Persistant>())
			{
				GameObject g = p.gameObject;
				Destroy(g);
			}
			SceneManager.LoadScene(scene);
		}
	}

	public void Continue()
	{
		StartCoroutine(fadeOutMenu(3f));
		sm.menuMusicFade(-0.5f);
		//sm.caveMusicFade(0.1f);
		//fadeOutMenu();
	}

	public void Back()
	{
		playMenuUI.SetActive(false);
		startMenuUI.SetActive(true);
	}

	public void NewGame()
	{
		fadingToScene = true;
		scene = "Attempt 2";
		fade.startFadeOut(timeToFade, true);
		sm.menuMusicFade(-1f);
	}

	public void PlayArcade()
	{
		arcadeMenuUI.SetActive(true);
		playMenuUI.SetActive(false);
	}

	IEnumerator fadeOutMenu(float fadePerSecond)
	{
		StartMenu.isOpen = false;
		while (cg.alpha > 0)
		{
			//Debug.Log(cg.alpha);
			cg.alpha -= fadePerSecond * Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		cg.alpha = 1;
		playMenuUI.SetActive(false);
	}

	/*
	IEnumerator fadeObject(GameObject obj, float fadePerSecond, float delay1, float delay2)
	{
		fadeObjectRunning = true;
		CanvasGroup cg = obj.GetComponent<CanvasGroup>();
		
		if (fadePerSecond > 0)
		{
			obj.SetActive(true);
			cg.alpha = 0;
			yield return new WaitForSeconds(delay1);

			while (cg.alpha < 1)
			{
				cg.alpha += fadePerSecond * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			StartCoroutine(fadeObject(obj, -fadePerSecond, delay1, delay2));
		}
		else
		{
			cg.alpha = 1;
			yield return new WaitForSeconds(delay2);

			while (cg.alpha > 0)
			{
				cg.alpha += fadePerSecond * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			//obj.SetActive(false);
			yield return new WaitForSeconds(0.5f);
			fadeObjectRunning = false;
		}
		
	}
	*/

}
