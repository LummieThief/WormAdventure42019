using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public static bool isPaused;
	public GameObject pauseMenuUI;
	public GameObject optionsMenuUI;
	public GameObject mask;
	public Game game;

    // Update is called once per frame
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
		if (Input.GetKeyDown(KeyCode.Escape) && !(game != null && game.getStartingGrapple()) && !GooseChooserMenu.isActive)
		{
			if (optionsMenuUI.activeSelf)
			{
				optionsMenuUI.SetActive(false);
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
}
