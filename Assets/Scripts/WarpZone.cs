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
		}
		//Debug.Log("left scene");
		//backupCam.enabled = true;
		SceneManager.LoadScene(scene);
	}
}
