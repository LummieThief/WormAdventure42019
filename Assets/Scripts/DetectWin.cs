using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWin : MonoBehaviour
{
	public static bool hasWon;
	private Game game;
	// Start is called before the first frame update
	private void Start()
	{
		game = FindObjectOfType<Game>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "GP")
		{
			if (!game.getStartingGrapple())
			{
				win();
			}
		}
	}

	private void win()
	{
		hasWon = true;
		Debug.Log("Winners");
		Debug.Log(game.getStartingGrapple());
	}

	public void toggleWin()
	{
		hasWon = !hasWon;
	}
}
