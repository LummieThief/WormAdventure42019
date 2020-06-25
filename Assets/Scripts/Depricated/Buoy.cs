using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoy : MonoBehaviour
{
	public Rigidbody connectedInstance;
	public float buoyancy = 1;
	private float maxVelocity = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void applyForce(float force, Vector3 direction)
	{
		connectedInstance.AddForceAtPosition(force * buoyancy * direction * Time.deltaTime, transform.position);
		
		if (connectedInstance.velocity.magnitude > maxVelocity)
		{
			Debug.Log(connectedInstance.velocity.magnitude);
			connectedInstance.velocity = connectedInstance.velocity.normalized * maxVelocity;
		}
		
	}

	public void enterWater()
	{
		//connectedInstance.AddForceAtPosition(-connectedInstance.GetPointVelocity(transform.position), transform.position, ForceMode.Impulse);
	}
}
