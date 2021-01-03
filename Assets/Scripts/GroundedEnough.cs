using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnough : MonoBehaviour
{
	private bool groundedEnough;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Solid" || other.tag == "Solid Excluded")
		{
			groundedEnough = true;
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Solid" || other.tag == "Solid Excluded")
		{
			groundedEnough = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Solid" || other.tag == "Solid Excluded")
		{
			groundedEnough = false;
		}
	}

	public bool getGroundedEnough()
	{
		return groundedEnough;
	}

	public void setGroundedEnough(bool value)
	{
		groundedEnough = value;
	}
}
