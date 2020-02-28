using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEnd : MonoBehaviour
{
	public TimerStart start;
    // Start is called before the first frame update

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			start.running = false;
		}
	}
}
