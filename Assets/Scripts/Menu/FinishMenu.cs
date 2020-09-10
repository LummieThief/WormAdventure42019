using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishMenu : MonoBehaviour
{
	public GameObject finishMenuUI;
	public Text congratulations;
	public Text deaths;
	public Text finalTime;
	public Text bestTime;
	public static bool isOpen = false;
	private float rawTime;
	private ArcadeTimer at;
	// Start is called before the first frame update
	void Start()
	{
		ArcadeTimer at = FindObjectOfType<ArcadeTimer>();
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log(at.getRawPlayTime());
	}

	public void Menu()
	{
		deactivate();
		foreach (Persistant p in FindObjectsOfType<Persistant>())
		{
			Destroy(p.gameObject);
		}
		SceneManager.LoadScene("Attempt 2");
	}

	public void activate()
	{
		isOpen = true;
		finishMenuUI.SetActive(true);

		if (at == null)
		{
			at = FindObjectOfType<ArcadeTimer>();
		}
		at.StopTimer();
		finalTime.text = finalTime.text + at.getPlayTime();
		rawTime = at.getRawPlayTime();

		deaths.text += FindObjectOfType<DeathCounter>().getDeaths();
		congratulations.text = getCongratulationsText();
		setNewBest();
		
		Debug.Log("out the loop " + rawTime);

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void deactivate()
	{
		isOpen = false;
		finishMenuUI.SetActive(false);
	}

	private string getCongratulationsText()
	{
		string text = congratulations.text;
		string difficulty = "";
		string unlock = "";



		switch (getDifficulty())
		{
			case 0:
				difficulty = "Beginner";

				if (!PlayerPrefs.HasKey("Arcade") || PlayerPrefs.GetInt("Arcade") < 1)
				{
					PlayerPrefs.SetInt("Arcade", 1);
					unlock = "Experienced mode unlocked";
				}

				PlayerPrefs.SetInt("Arcade", 1);
				break;

			case 1:
				difficulty = "Experienced";

				if (PlayerPrefs.GetInt("Arcade") < 2)
				{
					unlock = "Veteran mode unlocked";
				}

				PlayerPrefs.SetInt("Arcade", 2);
				break;

			case 2:
				difficulty = "Veteran";

				if (PlayerPrefs.GetInt("Arcade") < 3)
				{
					unlock = "Master mode unlocked";
				}

				PlayerPrefs.SetInt("Arcade", 3);
				break;

			case 3:
				difficulty = "Master";
				unlock = "You are a true master of Worm Adventure";
				PlayerPrefs.SetInt("Arcade", 4);
				break;
		}

		text = text.Replace("[difficulty]", difficulty);

		text = text.Replace("[unlock]", unlock);

		return text;
	}

	private void setNewBest()
	{
		float difficulty = getDifficulty();

		string key = "undefined";
		switch (difficulty)
		{
			case 0:
				key = "ArcadeBest0";
				break;
			case 1:
				key = "ArcadeBest1";
				break;
			case 2:
				key = "ArcadeBest2";
				break;
			case 3:
				key = "ArcadeBest3";
				break;
		}

		if (at == null)
		{
			at = FindObjectOfType<ArcadeTimer>();
		}
		if (PlayerPrefs.HasKey(key))
		{
			Debug.Log("key");
			if (PlayerPrefs.GetFloat(key) < rawTime) //if the existing record is better
			{
				Debug.Log("1");
				bestTime.text += at.convertTime(PlayerPrefs.GetFloat(key));
				
			}
			else //if the new time is better
			{
				Debug.Log("2");
				PlayerPrefs.SetFloat(key, rawTime);
				bestTime.text += at.convertTime(rawTime);
				Debug.Log("in the loop " + rawTime);
			}
		}
		else //if theyve never gotten a record before
		{
			PlayerPrefs.SetFloat(key, rawTime);
			bestTime.text += at.convertTime(rawTime);
			Debug.Log("no key");
		}
	}

	private int getDifficulty()
	{
		int levelNumber = int.Parse(SceneManager.GetActiveScene().name.Substring(6));
		if (levelNumber < 100)
		{
			return 0;
		}
		else if (levelNumber < 200)
		{
			return 1;
		}
		else if (levelNumber < 300)
		{
			return 2;
		}
		else
		{
			return 3;
		}
	}
}
