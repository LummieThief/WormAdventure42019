using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTP : MonoBehaviour
{
	public Transform worm;
	public Transform[] teleporters;
	
	private int current = 0;
    // Start is called before the first frame update
    void Start()
    {
		foreach (Transform t in teleporters)
		{
			t.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F12))
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
