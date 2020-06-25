using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerEvent : MonoBehaviour
{
	private bool trigger1;
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			trigger1 = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			trigger1 = false;
		}
	}

	public bool isTriggered(int num)
	{
		switch (num)
		{
			case 1:
				return trigger1;
			default:
				return false;
		}
	}
}
