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

	private string nullString = "N/A";
	private bool skipped;
	// Start is called before the first frame update
	void Start()
	{
		
		
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
		ArcadeTimer at = FindObjectOfType<ArcadeTimer>();
		skipped = at.getSkipped();
		Debug.Log("finish menu skipped " + skipped);

		if (at == null)
		{
			at = FindObjectOfType<ArcadeTimer>();
		}
		at.StopTimer();

		if (skipped)
		{
			finalTime.text = finalTime.text + nullString;
		}
		else
		{
			finalTime.text = finalTime.text + at.getPlayTime();
		}
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
				break;

			case 1:
				difficulty = "Experienced";

				if (PlayerPrefs.GetInt("Arcade") < 2)
				{
					PlayerPrefs.SetInt("Arcade", 2);
					unlock = "Veteran mode unlocked";
				}

				break;

			case 2:
				difficulty = "Veteran";

				if (PlayerPrefs.GetInt("Arcade") < 3)
				{
					PlayerPrefs.SetInt("Arcade", 3);
					unlock = "Master mode unlocked";
				}

				
				break;

			case 3:
				difficulty = "Master";
				unlock = "You are a true master of Worm Adventure";
				//PlayerPrefs.SetInt("Arcade", 4);
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

		if (skipped)
		{
			if (PlayerPrefs.HasKey(key))
			{
				var time = PlayerPrefs.GetFloat(key);
				if (time == 99999) //their record got reset
				{
					bestTime.text += nullString;
				}
				else
				{
					bestTime.text += at.convertTime(time);
				}
			}
			else //if theyve never gotten a record before
			{
				bestTime.text += nullString;
			}
		}
		else
		{


			if (PlayerPrefs.HasKey(key))
			{
				if (PlayerPrefs.GetFloat(key) < rawTime) //if the existing record is better
				{
					bestTime.text += at.convertTime(PlayerPrefs.GetFloat(key));

				}
				else //if the new time is better
				{
					PlayerPrefs.SetFloat(key, rawTime);
					bestTime.text += at.convertTime(rawTime);
				}
			}
			else //if theyve never gotten a record before
			{
				PlayerPrefs.SetFloat(key, rawTime);
				bestTime.text += at.convertTime(rawTime);
			}
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
