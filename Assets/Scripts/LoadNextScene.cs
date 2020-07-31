using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
	public void loadNextScene(int value)
	{
		if (!FinishBox.finished)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		/*
		string sceneName = SceneManager.GetActiveScene().name;
		int levelNumber = int.Parse(sceneName.Substring(6));
		Debug.Log(levelNumber);
		if (Application.CanStreamedLevelBeLoaded("Level " + (levelNumber - value)))
		{
			SceneManager.LoadScene("Level " + (levelNumber - value));
		}
		else
		{
			SceneManager.LoadScene("Level 1");
		}
		*/
	}
}
