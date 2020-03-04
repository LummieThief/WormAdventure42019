using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelFollow : MonoBehaviour
{
	//private float offsetX;
	//private float offsetY;
	//private float offsetZ;
	private Vector3 previousPosition;
	private Quaternion previousRotation;




	public Transform worm;
    // Start is called before the first frame update
    void Start()
    {
		//offsetX = transform.position.x - worm.position.x;
		//offsetY = transform.position.y - worm.position.y;
		//offsetZ = transform.position.z - worm.position.z;
		previousPosition = worm.position;
		previousRotation = worm.rotation;

	}

    // Update is called once per frame
    void LateUpdate()
    {
		float newXpos =  worm.position.x - previousPosition.x;
		float newYpos =  worm.position.y - previousPosition.y;
		float newZpos = worm.position.z - previousPosition.z;
		Vector3 posDisplacement = new Vector3(newXpos, newYpos, newZpos);

		float newXrot = worm.eulerAngles.x - previousRotation.eulerAngles.x;
		float newYrot = worm.eulerAngles.y - previousRotation.eulerAngles.y;
		float newZrot = worm.eulerAngles.z - previousRotation.eulerAngles.z;
		Vector3 rotDisplacement = new Vector3(newXrot, newYrot, newZrot);

		transform.position += posDisplacement;
		transform.eulerAngles += rotDisplacement;

		previousPosition = worm.position;
		previousRotation = worm.rotation;
	}
}
