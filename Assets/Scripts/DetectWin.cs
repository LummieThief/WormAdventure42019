using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWin : MonoBehaviour
{
	public static bool hasWon;
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "GP")
		{
			win();
		}
	}

	private void win()
	{
		hasWon = true;
	}
}
