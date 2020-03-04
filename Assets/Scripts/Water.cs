using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Underwater");
		if (other.tag == "Player")
		{
			other.GetComponent<WormMove>().underwater = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		Debug.Log("Not Underwater");
		if (other.tag == "Player")
		{
			other.GetComponent<WormMove>().underwater = false;
		}
	}
}
