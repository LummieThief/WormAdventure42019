using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
	public GameObject controlsMenuUI;
	public GameObject startMenuUI;
	public GameObject pauseMenuUI;

	public void Back()
	{

		controlsMenuUI.SetActive(false);
		if (!PauseMenu.isPaused)
		{
			startMenuUI.SetActive(true);
		}
		else
		{
			pauseMenuUI.SetActive(true);
		}
	}
}
