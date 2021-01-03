using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTP : MonoBehaviour
{
	public Transform worm;
	public Transform[] teleporters;
	public Transform[] moreTeleporters;
	
	private int current = 0;
	private int current2 = 0;
    // Start is called before the first frame update
    void Start()
    {

		foreach (Transform t in teleporters)
		{
			t.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}

		foreach (Transform t in moreTeleporters)
		{
			t.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (!SteamManager.debugMode)
			return;

		if (Input.GetKeyDown(KeyCode.F12))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				worm.transform.position = moreTeleporters[current2].position;
				current2++;
				if (current2 >= moreTeleporters.Length)
				{
					current2 = 0;
				}
			}
			else
			{
				worm.transform.position = teleporters[current].position;
				current++;
				if (current >= teleporters.Length)
				{
					current = 0;
				}
			}
		}
	}
}
