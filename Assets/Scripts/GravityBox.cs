using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityBox : MonoBehaviour
{
	public float force = 100f;
	private float height = 100;
	private Rigidbody wormRB;
	private void OnTriggerStay(Collider other)
	{
		
		if (other.GetComponent<Rigidbody>() != null && Game.gravBoxEnabled && other.gameObject.tag == "Player")
		{
			wormRB = other.GetComponent<Rigidbody>();

			Vector3 top = new Vector3(transform.position.x, wormRB.transform.position.y + height, transform.position.z);
			Vector3 bot = new Vector3(wormRB.transform.position.x, wormRB.transform.position.y, wormRB.transform.position.z);
			Vector3 direction = top - bot;

			//Debug.DrawRay(wormRB.transform.position, direction);
			Debug.DrawLine(top, bot);
			
			//wormRB.AddForce(Vector3.up * force * Time.deltaTime);
			wormRB.AddForce(direction.normalized * force * Time.deltaTime);
		}
	}


}
