using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityBox : MonoBehaviour
{
	public float force = 100f;
	//public static bool freezeWorm;
	public string scene;
	//public Camera backupCam;
	private Rigidbody wormRB;
	public bool fallOut = false;
	// Start is called before the first frame update

	private void OnTriggerStay(Collider other)
	{
		
		if (other.GetComponent<Rigidbody>() != null && Game.gravBoxEnabled && other.gameObject.tag == "Player")
		{
			//freezeWorm = true;
			//other.transform.eulerAngles = Vector3.zero;
			wormRB = other.GetComponent<Rigidbody>();
			//wormRB.angularVelocity = Vector3.zero;
			wormRB.velocity = new Vector3(wormRB.velocity.x / 1.05f, wormRB.velocity.y, wormRB.velocity.z / 1.05f);
			wormRB.AddForce(Vector3.up * force * Time.deltaTime);
		}
	}

	private void Update()
	{
		if (wormRB != null)
		{
			if ((!fallOut && wormRB.velocity.y < 0) || (fallOut && wormRB.velocity.y > 0))
			{
				//freezeWorm = false;
				Game.gravBoxEnabled = false;
				goToScene(scene);
				//Debug.Log("fading");
				
			}

		}
		
	}
	/*	private void OnTriggerExit(Collider other)
		{
			if (other.gameObject.tag == "Player")
			{
				if (other.GetComponent<Rigidbody>() != null && other.GetComponent<Rigidbody>().velocity.y > 0 && Game.gravBoxEnabled)
				{
					fader.toScene("Attempt 3");
				}

			}
		}*/

	private void goToScene(string scene)
	{
		Debug.Log("left scene");
		//backupCam.enabled = true;
		SceneManager.LoadScene(scene);
	}
}
