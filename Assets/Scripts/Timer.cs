using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
	private string finalScene = "Winners";
	private float playTime;
	public TextMeshPro timeText;


	private bool running;


    void Update()
    {
		resetRunning();
		if (SceneManager.GetActiveScene().name == finalScene)
		{
			running = false;
			if (timeText == null && playTime != 0)
			{
				timeText = GameObject.FindGameObjectWithTag("Time Text").GetComponent<TextMeshPro>();
				timeText.SetText(timeText.text + convertTime(playTime));
			}
		}
		if (running)
		{
			playTime += Time.deltaTime;
		}
		
		//Debug.Log(playTime);
    }

	public void setTime(float time)
	{
		playTime = time;
	}

	public float getTime()
	{
		return playTime;
	}

	private void resetRunning()
	{
		if (!StartMenu.isOpen)
		{
			running = true;
		}
	}

	private string convertTime(float time)
	{
		int milli = (int)(((time % 1) - (time % 0.001)) * 1000);
		int timeLeft = (int)(time - (time % 1));

		int hours = timeLeft / 3600;
		timeLeft -= hours * 3600;

		int minutes = timeLeft / 60;
		timeLeft -= minutes * 60;

		int seconds = timeLeft;

		string finalTime = "";
		if (hours > 0)
		{
			finalTime += +hours + ":";
		}
		if (minutes > 0)
		{
			if (minutes < 10)
				finalTime += "0";
			finalTime += minutes + ":";
		}
		if (seconds > 0)
		{
			if (seconds < 10)
				finalTime += "0";
			finalTime += seconds + ".";
		}
		finalTime += milli;

		return finalTime;
	}
}
