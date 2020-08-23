using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingCountdown : MonoBehaviour
{
	public StartingCamera startingCam;
	public Animator anim;
	private SoundManager sm;

	public void Go()
	{
		sm = FindObjectOfType<SoundManager>();
		startingCam.startGame();
		sm.playGo();
	}

	public void startCountdown()
	{
		//Debug.Log("playing");
		anim.SetBool("Counting", true);
	}

	public void Ready()
	{
		sm = FindObjectOfType<SoundManager>();
		sm.playReady();
	}
}
