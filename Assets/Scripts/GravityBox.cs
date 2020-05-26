using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityBox : MonoBehaviour
{
	public float force = 100f;
	public Camera backupCam;
	// Start is called before the first frame update
	private void OnTriggerStay(Collider other)
	{
		if (other.GetComponent<Rigidbody>() != null)
		{
			//Debug.Log("grav box");
			other.GetComponent<Rigidbody>().AddForce(Vector3.up * force * Time.deltaTime);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (other.GetComponent<Rigidbody>() != null && other.GetComponent<Rigidbody>().velocity.y > 0)
			{
				goToScene();
			}
			
		}
	}

	private void goToScene()
	{
		Debug.Log("left scene");
		backupCam.enabled = true;
		SceneManager.LoadScene("Intermediate");
	}
}
