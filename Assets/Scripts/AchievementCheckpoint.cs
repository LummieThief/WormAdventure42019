using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCheckpoint : MonoBehaviour
{
	private bool achieved;
	public string achievement;

	private bool wormIn = false;

	private JumpTrigger jumpTrigger;
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			wormIn = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			wormIn = false;
		}
	}

	private void Update()
	{
		if (jumpTrigger == null)
		{
			jumpTrigger = FindObjectOfType<JumpTrigger>();
		}
		else if (wormIn && !achieved && jumpTrigger.getSolidGround())
		{
			achieved = true;
			AchievementManager.Achieve(achievement);
		}
	}
}
