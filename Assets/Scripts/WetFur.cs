using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WetFur : MonoBehaviour
{
	/*public GameObject wetFur;

	private bool wormIn;
	private bool wasPaused;
	private Game game;
	private void Start()
	{
		game = FindObjectOfType<Game>();
	}
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			wormIn = true;
			Debug.Log("worm in");
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

		if (wetFur == null)
		{
			wetFur = GameObject.FindGameObjectWithTag("WetFur");
		}
		else
		{
			//Debug.Log(wormIn);
			if (wormIn && !PauseMenu.isPaused && !game.getStartingGrapple())
			{
				if (!wetFur.activeSelf)
				{
					if (!wasPaused)
					{
						FindObjectOfType<SoundManager>().playSecret();
					}
					wetFur.SetActive(true);
				}

			}
			else
			{
				wetFur.SetActive(false);
			}

			wasPaused = PauseMenu.isPaused;
		}
	}*/
}
