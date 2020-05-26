using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathbox : MonoBehaviour
{
	public GameObject lastPlatform;
	public GameObject universe;
	public Transform worm;
	public Transform cam;

	private bool primed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (primed)
		{
			primed = !relocate();
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			primed = true;
		}
	}

	private bool relocate()
	{
		bool works = true;
		foreach (Quantum quan in FindObjectsOfType<Quantum>())
		{
			if (quan.getVisible())
			{
				works = false;
			}
		}
		if (cam.eulerAngles.x > 300 && cam.eulerAngles.x < 350 && works)
		{
			worm.transform.position = new Vector3(-0.69f, 7f, -0.27f);
			Rigidbody rb = worm.GetComponent<Rigidbody>();
			rb.velocity = new Vector3(rb.velocity.x / 3, rb.velocity.y, rb.velocity.z / 3);
			universe.transform.position = Vector3.zero;

			Quantum q = lastPlatform.GetComponent<Quantum>();
			if (q != null)
			{
				q.setActive(true);
				q.setVisible(true);
			}
			return true;
		}
		else
		{
			return false;
		}
	}
}
