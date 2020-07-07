using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityBox : MonoBehaviour
{
	public float force = 100f;
	private Rigidbody wormRB;
	private void OnTriggerStay(Collider other)
	{
		
		if (other.GetComponent<Rigidbody>() != null && Game.gravBoxEnabled && other.gameObject.tag == "Player")
		{
			//freezeWorm = true;
			//other.transform.eulerAngles = Vector3.zero;
			wormRB = other.GetComponent<Rigidbody>();
			//wormRB.angularVelocity = Vector3.zero;
			//wormRB.velocity = new Vector3(wormRB.velocity.x / 1.05f, wormRB.velocity.y, wormRB.velocity.z / 1.05f);
			wormRB.AddForce(Vector3.up * force * Time.deltaTime);
		}
	}


}
