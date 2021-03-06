﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeTimer : MonoBehaviour
{
	private float playTime;
	private bool running;
	private bool skipped = false;
    // Start is called before the first frame update
    void Start()
    {

		skipped = checkSkipped();
		if (skipped)
		{
			Debug.Log("Skipped");
		}
		else
		{
			Debug.Log("Played from the start");
		}
		StartTimer();

    }

    // Update is called once per frame
    void Update()
    {
		if (running)
		{
			playTime += Time.unscaledDeltaTime;
		}
    }

	public string convertTime(float time)
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
			finalTime += hours + ":";
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
		if (milli < 100)
		{
			if (milli < 10)
			{
				finalTime += "00";
			}
			else
			{
				finalTime += "0";
			}
		}
		finalTime += milli;


		return finalTime;
	}

	public string getPlayTime()
	{
		return convertTime(playTime);
	}

	public float getRawPlayTime()
	{
		return playTime;
	}

	public void StopTimer()
	{
		running = false;
	}
	public void StartTimer()
	{
		running = true;
	}

	private bool checkSkipped()
	{
		string sceneName = SceneManager.GetActiveScene().name;
		int levelNumber = int.Parse(sceneName.Substring(6));

		return levelNumber % 100 != 1;
	}

	public bool getSkipped()
	{
		return skipped;
	}
}
