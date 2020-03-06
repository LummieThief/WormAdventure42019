using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeClipStopper : MonoBehaviour
{
	//public string type = "rope"; //drop
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Solid")
		{
			GetComponentInParent<Collider>().isTrigger = true;
		}
	}
}
