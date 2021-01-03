using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormGrapplePoint : MonoBehaviour
{
	private LineRenderer line;
	//public Transform worm;
	private GameObject controller;
	private Transform host;

	//private int connectedId = 0;
    // Start is called before the first frame update
    void Awake()
    {
		line = GetComponent<LineRenderer>();
		//worm = FindObjectOfType<WormMove>().transform;
		
		controller = GameObject.FindGameObjectWithTag("MyPlayer");
	}

    // Update is called once per frame
    void Update()
    {
		line.positionCount = 2;
		line.SetPosition(0, transform.position);
		line.SetPosition(1, host.position + host.TransformDirection(Vector3.down) * host.lossyScale.y * 0.9f);
	}

	public void setUp(Transform connected, Transform host, Material lineMat)
	{
		this.host = host;
		int connectedId = connected.GetComponent<PhotonOwner>().id;
		line.sharedMaterial = lineMat;

		controller.GetComponent<PhotonPlayerController>().grappleToWorm(connectedId, transform.localPosition);
	}

	public void destroyLine()
	{
		controller.GetComponent<PhotonPlayerController>().destroyLine();
	}
}
