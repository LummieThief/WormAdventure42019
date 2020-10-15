using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePointPhotonCommunicator : MonoBehaviour
{
	// Start is called before the first frame update
	GameObject controller;
    void Start()
    {
		controller = GameObject.FindGameObjectWithTag("MyPlayer");
		if (controller != null)
		{
			GameObject emptyGO = new GameObject();
			emptyGO.transform.position = transform.position;
			controller.GetComponent<PhotonPlayerController>().addGrapplePoint(emptyGO.transform);
			Debug.Log(controller);
		}
		else
		{
			Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (controller != null)
		{
			controller.GetComponent<PhotonPlayerController>().removeGrapplePoint();
		}
	}

}
