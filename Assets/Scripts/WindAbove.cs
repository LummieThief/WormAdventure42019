using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAbove : MonoBehaviour
{
	private Transform worm;
	private WormMove wormMove;
	private SoundManager sm;
	private float maxVolume = 0.4f;
	private float rampUpSpeed = 0.6f;
	private float vol;
	private Game game;
    // Start is called before the first frame update
    void Start()
    {
		wormMove = FindObjectOfType<WormMove>();
		worm = wormMove.transform;
		sm = FindObjectOfType<SoundManager>();
		game = FindObjectOfType<Game>();
		if (worm.position.y > transform.position.y && !game.getStartingGrapple())
		{
			sm.setWindVolume(maxVolume);
			vol = maxVolume;
			//Debug.LogError("max");
			//Debug.LogError("Worm position: " + worm.position.y + "  My position: " + transform.position.y);
		}
		else
		{
			sm.setWindVolume(0);
			vol = 0;
			//Debug.LogError("Worm position: " + worm.position.y + "  My position: " + transform.position.y);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (worm == null)
		{
			wormMove = FindObjectOfType<WormMove>();
			worm = wormMove.transform;

		}

		if (Time.timeScale != 1)
		{
			sm.setWindVolume(0);
		}
		else
		{
			if (worm.position.y > transform.position.y)
			{
				vol = Mathf.Lerp(vol, maxVolume, Time.deltaTime * rampUpSpeed);
				sm.setWindVolume(vol);
				wormMove.setArtificialWindSpeed(1);
			}
			else
			{
				vol = Mathf.Lerp(vol, 0, Time.deltaTime * rampUpSpeed * 7);
				sm.setWindVolume(vol);
				wormMove.setArtificialWindSpeed(0);
			}
		}
		//Debug.Log(vol);
    }
}
