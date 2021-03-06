﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseplateDeactivator : MonoBehaviour
{
	public bool deactivated = true;
	private bool prevState = true;
    // Start is called before the first frame update
    void Start()
    {
		foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
		{
			r.enabled = false;
		}
		foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Pause();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			deactivated = false;
		}

	}
	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			deactivated = true;
		}

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			deactivated = !deactivated;
		}
		if (deactivated && !prevState)
		{
			foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
			{
				r.enabled = false;
			}
			foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
			{
				if (p.isPlaying)
				{
					p.Pause();
				}
			}
			//transform.position = new Vector3(transform.position.x, transform.position.y - 10000, transform.position.z);
		}
		else if (!deactivated && prevState)
		{
			foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
			{
				r.enabled = true;
			}
			foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
			{
				if (p.isPaused)
				{
					p.Play();
				}
			}
			//transform.position = new Vector3(transform.position.x, transform.position.y + 10000, transform.position.z);
		}

		prevState = deactivated;
	}
}
