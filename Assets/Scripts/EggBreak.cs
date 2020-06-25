using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBreak : MonoBehaviour
{
	public Transform home;
	private bool primed;
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<LineRenderer>().useWorldSpace = true;
    }

	// Update is called once per frame
	private void OnTriggerStay(Collider other)
	{
		if (primed && other.gameObject.tag == "Solid")
		{
			if (home == null)
			{
				Destroy(gameObject);
			}
			else
			{
				GetComponent<Rigidbody>().isKinematic = true;
				transform.position = home.position;
				transform.rotation = Quaternion.identity;
				primed = false;
			}
		}
	}

	public void prime()
	{
		primed = true;
	}
}
