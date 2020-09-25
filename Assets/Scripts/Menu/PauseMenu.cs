using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static bool isPaused;
	public GameObject pauseMenuUI;
	public GameObject optionsMenuUI;
	public GameObject controlsMenuUI;
	//public GameObject startMenuUI;
	public GameObject mask;
	public Game game;
	private SoundManager sm;

	// Update is called once per frame
	private void Start()
	{
		sm = FindObjectOfType<SoundManager>();
	}
	void Update()
    {
		if (game == null)
		{
			game = FindObjectOfType<Game>();
		}

		if (SaveLoad.initializing)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape) && !(game != null && game.getStartingGrapple()) 
			&& !GooseChooserMenu.isActive && !DetectWin.hasWon && !FinishBox.finished)
		{
			if (optionsMenuUI.activeSelf)
			{
				optionsMenuUI.SetActive(false);
				pauseMenuUI.SetActive(true);
			}
			else if (controlsMenuUI.activeSelf)
			{
				controlsMenuUI.SetActive(false);
				pauseMenuUI.SetActive(true);
			}
			else
			{
				if (isPaused)
				{
					
					resume();
				}
				else
				{
					pause();
				}
			}
			
		}
    }

	public void resume()
	{
		pauseMenuUI.SetActive(false);
		mask.SetActive(false);
		Time.timeScale = 1;
		isPaused = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void pause()
	{
		pauseMenuUI.SetActive(true);
		mask.SetActive(true);
		Time.timeScale = 0;
		isPaused = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void quitGame()
	{
		resume();
		Application.Quit();
	}

	public void options()
	{
		optionsMenuUI.SetActive(true);
		pauseMenuUI.SetActive(false);
	}

	public void MainMenu()
	{
		resume();
		GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
		foreach (Persistant p in FindObjectsOfType<Persistant>())
		{
			GameObject g = p.gameObject;
			Destroy(g);
		}
		SceneManager.LoadScene("Attempt 2");
	}

	public void controls()
	{
		controlsMenuUI.SetActive(true);
		pauseMenuUI.SetActive(false);
	}

	public void feedback()
	{
		Application.OpenURL("https://forms.gle/y7TVsU1x9XRG1z276");
		AchievementManager.Achieve("ACH_FEEDBACK");
	}
}
