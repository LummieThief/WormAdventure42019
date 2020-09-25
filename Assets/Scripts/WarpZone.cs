using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpZone : MonoBehaviour
{
	public string scene;
	public bool deletePersistants;
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			//Debug.Log("going to scene");
			goToScene(scene);
		}
	}


	private void goToScene(string scene)
	{
		switch (scene)
		{
			case "Winners":
				AchievementManager.Achieve("ACH_WIN");
				break;
			case "Attempt 2":
				AchievementManager.Achieve("ACH_FALL");
				break;
			case "Attempt 3":
				AchievementManager.Achieve("ACH_WORMHOLE");
				break;
			default:
				break;

		}


		if (deletePersistants)
		{
			foreach (Persistant p in FindObjectsOfType<Persistant>())
			{
				Destroy(p.gameObject);
			}
			SceneManager.LoadScene(scene);
		}

		if (scene == "Winners" && FindObjectOfType<WormMove>().getHolding())
		{
			scene = "Ranch";
			AchievementManager.Achieve("ACH_GOOSE");
		}
		//Debug.Log("left scene");
		//backupCam.enabled = true;
		SceneManager.LoadScene(scene);
	}
}
