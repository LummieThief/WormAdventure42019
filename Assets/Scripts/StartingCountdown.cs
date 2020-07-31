using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingCountdown : MonoBehaviour
{
	public StartingCamera startingCam;
	public Animator anim;

	public void Go()
	{
		startingCam.startGame();
	}

	public void startCountdown()
	{
		//Debug.Log("playing");
		anim.SetBool("Counting", true);
	}
}
