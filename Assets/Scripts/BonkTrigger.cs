using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkTrigger : MonoBehaviour
{
	private bool bonking;
	public Rigidbody worm;
	public float bonkThreshold;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	/*
	private void OnTriggerEnter(Collider other)
	{
		
		if (other.tag == "Solid" && Vector3.Project(worm.velocity, worm.transform.up).magnitude > bonkThreshold)
		{
			bonking = true;
			Debug.Log("Bonk");
			worm.velocity = Vector3.zero;
			//Debug.Log(bonking);
		}
	}
	*/
}
