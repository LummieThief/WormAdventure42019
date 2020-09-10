using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetConfirmMenu : MonoBehaviour
{
	public GameObject resetConfirmMenuUI;
	//public GameObject optionsMenuUI;

	public void confirm()
	{
		PauseMenu.isPaused = false;
		Time.timeScale = 1;

		GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
		ResetSave.resetSave();
		PlayerPrefs.SetFloat("Arcade", 0);
		PlayerPrefs.SetFloat("ArcadeBest0", 99999);
		PlayerPrefs.SetFloat("ArcadeBest1", 99999);
		PlayerPrefs.SetFloat("ArcadeBest2", 99999);
		PlayerPrefs.SetFloat("ArcadeBest3", 99999);
		//GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
		foreach (Persistant p in FindObjectsOfType<Persistant>())
		{
			GameObject g = p.gameObject;
			Destroy(g);
		}
		SceneManager.LoadScene("Attempt 2");
	}
	public void back()
	{
		resetConfirmMenuUI.SetActive(false);
		//optionsMenuUI.SetActive(true);
	}
}
