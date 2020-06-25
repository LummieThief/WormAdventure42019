using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public static bool isPaused;
	public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
		if (SaveLoad.initializing)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
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

	public void resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1;
		isPaused = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void pause()
	{
		pauseMenuUI.SetActive(true);
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
}
