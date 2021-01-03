using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

	private void Start()
	{
		finished = false;
	}
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

	private void Update()
	{
		if (!SteamManager.debugMode)
			return;
		if (Input.GetKeyDown(KeyCode.F12) && !running)
		{
			mg = FindObjectOfType<MiniGame>();
			running = true;
			GameObject.FindObjectOfType<CameraFollow>().setState(1);
			wormRB = GameObject.FindObjectOfType<WormMove>().GetComponent<Rigidbody>();
		}
	}

	private void FixedUpdate()
	{
		//Debug.Log(finished);
		if (running)
		{
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
					finished = true;
					if (timer > state1Transition)
					{
						state = 1;
						FindObjectOfType<WormMove>().setDead(true);

						LoadNextScene lns = FindObjectOfType<LoadNextScene>();
						lns.gameObject.GetComponent<Animator>().SetBool("Closing", true);

						var name = SceneManager.GetActiveScene().name;
						int num = int.Parse(name.Substring(6));
						if (!Application.CanStreamedLevelBeLoaded("Level " + (num + 1)))
						{
							foreach (Image i in lns.GetComponentsInChildren<Image>())
							{
								i.color = Color.black;
							}
						}
					
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
					//finished = false;
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
					//Debug.Log(levelNumber);

					
					if (Application.CanStreamedLevelBeLoaded("Level " + (levelNumber + 1)))
					{
						if (PlayerPrefs.HasKey("ArcadeLevel"))
						{
							if (PlayerPrefs.GetInt("ArcadeLevel") < levelNumber + 1)
							{
								PlayerPrefs.SetInt("ArcadeLevel", levelNumber + 1);
							}	
						}
						else
						{
							PlayerPrefs.SetInt("ArcadeLevel", levelNumber + 1);
						}
						SceneManager.LoadScene("Level " + (levelNumber + 1));
					}
					else
					{
						FindObjectOfType<FinishMenu>().activate();
						state++;
					}
					break;
				case 3:

					break;
			}
		}
	}
}
