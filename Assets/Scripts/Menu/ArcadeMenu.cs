using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcadeMenu : MonoBehaviour
{
	private FadeController fade;
	public GameObject playMenuUI;
	public GameObject arcadeMenuUI;
	public GameObject arcadeIntroUI;
	public GameObject[] arcadeIntroText;
	//public static bool isOpen = true;

	private int selection = 0;
	public Text difficultyText;
	public string[] difficultyNames;
	public GameObject toggleRightButton;
	public GameObject toggleLeftButton;

	public GameObject togglePlusButton;
	public GameObject toggleMinusButton;
	private int levelNumber = 1;
	public Text levelNumberText;


	private bool fadingToScene;
	private bool goingToScene;
	private string scene;
	private float fadeTimer;
	private float timeToFade = 1;
	private bool fadeObjectRunning = false;


	private SoundManager sm;
	// Start is called before the first frame update
	void Start()
	{
		fade = FindObjectOfType<FadeController>();


		if (!PlayerPrefs.HasKey("Arcade") || (PlayerPrefs.HasKey("Arcade") && PlayerPrefs.GetInt("Arcade") == 0))
		{
			toggleRightButton.SetActive(false);
		}
		if (!PlayerPrefs.HasKey("ArcadeLevel") || (PlayerPrefs.HasKey("ArcadeLevel") && PlayerPrefs.GetInt("ArcadeLevel") == 1))
		{
			togglePlusButton.SetActive(false);
		}

		difficultyText.text = difficultyNames[0];

		sm = FindObjectOfType<SoundManager>();

	}

	private void Update()
	{


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

	public void Back()
	{
		if (fadingToScene)
			return;
		arcadeMenuUI.SetActive(false);
		playMenuUI.SetActive(true);
	}


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

	public void GoToArcade()
	{
		if (goingToScene || fadeObjectRunning)
			return;
		sm.menuMusicFade(-0.5f);
		//arcadeIntroUI.SetActive(true);
		goingToScene = true;

		
		scene = "Level ";
		switch (selection)
		{
			case 0:
				scene += levelNumber;
				break;
			case 1:
				scene += levelNumber + 100;
				break;
			case 2:
				scene += levelNumber + 200;
				break;
			case 3:
				scene += levelNumber + 300;
				break;
		}

		if (levelNumber == 1)
		{
			arcadeIntroText[selection].SetActive(true);
			switch (selection)
			{
				case 3:
					StartCoroutine(fadeObject(arcadeIntroUI, 1, 1, 8));
					break;
				default:
					StartCoroutine(fadeObject(arcadeIntroUI, 1, 1, 1));
					break;
			}
		}
		else
		{
			StartCoroutine(fadeObject(arcadeIntroUI, 1f, 0, 0));
		}

		fade.startFadeOut(timeToFade, true, new Color(0.04764745f, 0, 0.1981132f, 1));
	}

	public void toggleRight()
	{
		
		if (selection < difficultyNames.Length)
		{
			var dif = PlayerPrefs.GetInt("Arcade");
			if (dif > selection)
			{
				difficultyText.text = difficultyNames[selection + 1];
				selection++;

				if (selection == dif)
				{
					toggleRightButton.SetActive(false);
				}
			}
			toggleLeftButton.SetActive(true);
		}
		Debug.Log(PlayerPrefs.GetInt("Arcade"));
		levelNumber = 0;
		togglePlus();
	}

	public void toggleLeft()
	{

		if (selection > 0)
		{

			difficultyText.text = difficultyNames[selection - 1];
			selection--;
			if (selection == 0)
			{
				toggleLeftButton.SetActive(false);
			}
			toggleRightButton.SetActive(true);

		}

		levelNumber = 0;
		togglePlus();
	}

	public void togglePlus()
	{
		
		var levelMax = 1;
		var furthestLevel = PlayerPrefs.GetInt("ArcadeLevel");
		//Debug.Log("furthest level " + furthestLevel);
		switch (selection)
		{
			case 0:
				furthestLevel -= 0;
				furthestLevel = Mathf.Clamp(furthestLevel, 1, 10);
				break;
			case 1:
				furthestLevel -= 100;
				furthestLevel = Mathf.Clamp(furthestLevel, 1, 15);
				break;
			case 2:
				furthestLevel -= 200;
				furthestLevel = Mathf.Clamp(furthestLevel, 1, 20);
				break;
			case 3:
				furthestLevel -= 300;
				furthestLevel = Mathf.Clamp(furthestLevel, 1, 7);
				break;
		}
		levelMax = furthestLevel;
		//Debug.Log("Level max " + levelMax);
		if (levelNumber == 0) //it was called from changing the difficulty
		{
			levelNumber = 1;
			levelNumberText.text = "" + 1;
			toggleMinusButton.SetActive(false);
			if (levelMax <= 1)
			{
				togglePlusButton.SetActive(false);
			}
			else
			{
				togglePlusButton.SetActive(true);
			}
		}
		else if (levelNumber < levelMax)
		{

			levelNumber++;
			levelNumberText.text = "" + levelNumber;
			toggleMinusButton.SetActive(true);
			if (levelNumber == levelMax)
			{
				togglePlusButton.SetActive(false);
			}
		}

	}

	public void toggleMinus()
	{
		if (levelNumber > 1)
		{
			levelNumber--;
			levelNumberText.text = "" + levelNumber;
			togglePlusButton.SetActive(true);
			if (levelNumber == 1)
			{
				toggleMinusButton.SetActive(false);
			}
		}
	}
}
