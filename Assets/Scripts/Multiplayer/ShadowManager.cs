using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
	private Vector3 origin = new Vector3(500, 0, 500);

	public Transform worm;
	public Transform world;

	public Transform body;
	public Transform PUBody;

    // Start is called before the first frame update
    void Start()
    {
		//transform.position = origin;
    }

    // Update is called once per frame
    void Update()
    {
		body.position = PUBody.position;
		body.rotation = PUBody.rotation;
		bringFromPU(body);

		/*
		Vector3 wormPosition = worm.position;
		Vector3 worldPosition = world.position;

		Vector3 toPU = worldPosition - origin;

		worldPosition -= toPU;
		wormPosition -= toPU;


		transform.position = wormPosition;
		transform.rotation = worm.rotation;
		transform.RotateAround(worldPosition, Vector3.up, -world.eulerAngles.y);
		*/
    }


	private void sendToPU(Transform trans)
	{
		Vector3 worldPos = world.position;
		Vector3 toPU = worldPos - origin;

		worldPos -= toPU;


		trans.position -= toPU;
		trans.RotateAround(worldPos, Vector3.up, -world.eulerAngles.y);
	}

	private void bringFromPU(Transform trans)
	{
		Vector3 worldPos = world.position;
		Vector3 toPU = worldPos - origin;

		worldPos -= toPU;


		trans.RotateAround(worldPos, Vector3.up, world.eulerAngles.y);
		trans.position += toPU;
		
	}
}
