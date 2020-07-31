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

	private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		//offsetX = transform.position.x - worm.position.x;
		//offsetY = transform.position.y - worm.position.y;
		//offsetZ = transform.position.z - worm.position.z;
		previousPosition = worm.position;
		previousRotation = worm.rotation;

	}

    // Update is called once per frame
    void FixedUpdate()
    {
		float newXpos =  worm.position.x - previousPosition.x;
		float newYpos =  worm.position.y - previousPosition.y;
		float newZpos = worm.position.z - previousPosition.z;
		Vector3 posDisplacement = new Vector3(newXpos, newYpos, newZpos);


		rb.MovePosition(transform.position + posDisplacement);

		previousPosition = worm.position;
	}
}
