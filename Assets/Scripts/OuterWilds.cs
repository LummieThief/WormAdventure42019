using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Any objects with this script will rotate around the worm and be effected by Recenter
public class OuterWilds : MonoBehaviour
{
	private float rotSpeed = 120f;
	private float airRotSpeed = 40f;
	private Transform worm;
	private JumpTrigger jumpTrigger;
	private bool grounded;
    // Start is called before the first frame update
    void Start()
    {
		worm = GameObject.FindGameObjectWithTag("Player").transform;
		jumpTrigger = worm.gameObject.GetComponentInChildren<JumpTrigger>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (Input.GetAxis("Horizontal") != 0)
		{
			grounded = jumpTrigger.getGrounded();

			var rot = Input.GetAxis("Horizontal") * Time.deltaTime;
			if (grounded)
			{
				rot *= rotSpeed;
				transform.RotateAround(worm.position, Vector3.up, -rot);
			}
			else
			{
				rot *= airRotSpeed;
				transform.RotateAround(worm.position, Vector3.up, -rot);
			}
		}
	}
}
