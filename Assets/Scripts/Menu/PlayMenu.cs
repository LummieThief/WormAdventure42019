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
	public GameObject arcadeIntroUI;
	public GameObject[] arcadeIntroText;
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

	private SoundManager sm;
	// Start is called before the first frame update
	void Start()
	{
		fade = FindObjectOfType<FadeController>();
		cg = GetComponent<CanvasGroup>();
		playText = playButton.GetComponentInChildren<Text>();

		if (System.IO.File.Exists(SaveLoad.path))
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
		timer += scaleSpeed * Time.deltaTime;
		playButton.transform.localScale = startingScale + Vector3.one * maxScale * (1f + Mathf.Sin(timer) / 2f);


		if (fadingToScene)
		{
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
		if (fadingToScene)
			return;
		StartCoroutine(fadeOutMenu(3f));
		sm.menuMusicFade(-0.5f);
		//sm.caveMusicFade(0.1f);
		//fadeOutMenu();
	}

	public void Back()
	{
		if (fadingToScene)
			return;
		playMenuUI.SetActive(false);
		startMenuUI.SetActive(true);
	}

	public void NewGame()
	{
		if (fadingToScene)
			return;
		fadingToScene = true;
		scene = "Attempt 2";
		fade.startFadeOut(timeToFade, true);
		sm.menuMusicFade(-1f);
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

	IEnumerator fadeObject(GameObject obj, float fadePerSecond, float delay)
	{
		fadeObjectRunning = true;
		CanvasGroup cg = obj.GetComponent<CanvasGroup>();
		
		if (fadePerSecond > 0)
		{
			obj.SetActive(true);
			cg.alpha = 0;
			yield return new WaitForSeconds(delay);

			while (cg.alpha < 1)
			{
				cg.alpha += fadePerSecond * Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			StartCoroutine(fadeObject(obj, -fadePerSecond, delay));
		}
		else
		{
			cg.alpha = 1;
			yield return new WaitForSeconds(delay);

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

	public void GoToArcade(int level)
	{
		if (goingToScene || fadeObjectRunning)
			return;
		sm.menuMusicFade(-0.5f);
		//arcadeIntroUI.SetActive(true);
		goingToScene = true;
		StartCoroutine(fadeObject(arcadeIntroUI, 1, 1));
		arcadeIntroText[level].SetActive(true);
		switch (level)
		{
			case 0:
				scene = "Level 1";
				break;
			case 1:
				scene = "Level 101";
				break;
			default:
				scene = "Level 1";
				break;
		}
		
		fade.startFadeOut(timeToFade, true, new Color(0.04764745f, 0, 0.1981132f, 1));
	}
}
