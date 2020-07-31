using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FalloutBox : MonoBehaviour
{
	private bool running;
	private int state = 0;
	private float timer = 0;

	private float state1Transition = 1.5f;
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("starting");
		if (other.gameObject.tag == "Player" && !running)
		{
			
			running = true;
			GameObject.FindObjectOfType<CameraFollow>().setState(1);
			SoundManager sm = FindObjectOfType<SoundManager>();
			//sm.playFall();
		}
	}

	private void FixedUpdate()
	{
		if (running)
		{
			timer += Time.deltaTime;
			switch (state)
			{
				case 0:
					if (timer > state1Transition)
					{
						state = 1;
					}
					break;
				case 1:
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					break;
			}
		}
	}
}
