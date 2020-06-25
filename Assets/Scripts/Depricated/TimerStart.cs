using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class TimerStart : MonoBehaviour
{
	private bool wormInside = false;
	public Text t;
	private float timer = 0;
	private float record = 99999;
	public bool running = false;
	private string s;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (wormInside)
		{
			t.text = "";
		}
		else if (running)
		{
			timer += Time.deltaTime;
		}
		else
		{
			timer = (int)(timer * 100) / 100.0f;
			if (timer < record)
			{
				record = timer;
			}
			s = ("Time: " + timer.ToString() + "   Record: " + record.ToString());
			t.text = s;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			wormInside = true;
			running = false;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			wormInside = false;
			running = true;
			timer = 0;
		}
	}
}
