using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartTrigger : MonoBehaviour
{
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			ResetSave.resetSave();
			GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
			foreach (Persistant p in FindObjectsOfType<Persistant>())
			{
				GameObject g = p.gameObject;
				Destroy(g);
			}
			SceneManager.LoadScene("Attempt 2");
			PlayerPrefs.SetInt("FromWhite", 1);
		}
	}
}
