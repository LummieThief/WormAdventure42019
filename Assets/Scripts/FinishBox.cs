using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBox : MonoBehaviour
{
	public static bool finished = false;

	private bool running;
	private int state = 0;
	private float timer = 0;

	private float playSound = 0.3f;
	private float state1Transition = 0.5f;
	private float state2Transition = 1.5f;
	private Rigidbody wormRB;
	private float upForce = 10000;
	private MiniGame mg;
	private bool playedSound = false;

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player" && !running)
		{
			mg = FindObjectOfType<MiniGame>();
			running = true;
			GameObject.FindObjectOfType<CameraFollow>().setState(1);
			wormRB = GameObject.FindObjectOfType<WormMove>().GetComponent<Rigidbody>();
		}
	}

	private void FixedUpdate()
	{
		Debug.Log(finished);
		if (running)
		{
			finished = true;
			timer += Time.deltaTime;

			if (!playedSound && timer > playSound)
			{
				playedSound = true;
				SoundManager sm = FindObjectOfType<SoundManager>();
				sm.playWin();
				
			}
			switch (state)
			{
				case 0: //delay
					if (timer > state1Transition)
					{
						state = 1;
						FindObjectOfType<WormMove>().setDead(true);
						
						FindObjectOfType<LoadNextScene>().gameObject.GetComponent<Animator>().SetBool("Closing", true);
						FindObjectOfType<WormMove>().playWinExplosion();

					}
					break;
				case 1: 
					if (timer > state2Transition)
					{
						state = 2;
					}
					break;
				case 2:
					finished = false;
					if (mg != null)
					{
						if (mg.GetComponent<SoundManager>() == null)
						{
							Destroy(mg.gameObject);
						}
						else
						{
							Destroy(mg.GetComponent<PersistantProtection>());
							Destroy(mg);
						}
					}
					
					string sceneName = SceneManager.GetActiveScene().name;
					int levelNumber = int.Parse(sceneName.Substring(6));
					Debug.Log(levelNumber);
				
					
					if (Application.CanStreamedLevelBeLoaded("Level " + (levelNumber + 1)))
					{
						SceneManager.LoadScene("Level " + (levelNumber + 1));
					}
					else
					{
						SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					}
					break;
			}
		}
	}
}
