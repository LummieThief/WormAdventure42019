using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingCountdown : MonoBehaviour
{
	public StartingCamera startingCam;
	public Animator anim;
	private SoundManager sm;

	public static bool goed = true;

	private void Awake()
	{
		goed = true;
	}
	public void Go()
	{
		sm = FindObjectOfType<SoundManager>();
		startingCam.startGame();
		sm.playGo();
		goed = true;
	}

	public void startCountdown()
	{
		//Debug.Log("playing");
		anim.SetBool("Counting", true);
		goed = false;
	}

	public void Ready()
	{
		sm = FindObjectOfType<SoundManager>();
		sm.playReady();
	}
}
