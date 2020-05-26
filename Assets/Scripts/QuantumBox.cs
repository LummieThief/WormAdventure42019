using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumBox : MonoBehaviour
{
	private bool wormIn = false;
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			wormIn = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			wormIn = false;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		Quantum q = other.gameObject.GetComponent<Quantum>();
		if (q != null)
		{
			q.setActive(wormIn);
		}
	}
}
