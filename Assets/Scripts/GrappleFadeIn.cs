using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleFadeIn : MonoBehaviour
{
	private float timer = -1;
	private float incrementSpeed = 120f;
	private float maxSpeed = 100f;
    // Start is called before the first frame update
    void Awake()
    {
		GetComponent<MeshRenderer>().enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (timer > 0)
		{
			timer -= incrementSpeed * Time.deltaTime;
			GetComponent<MeshRenderer>().enabled = false;
		}
		else if (timer != -1)
		{
			GetComponent<MeshRenderer>().enabled = true;
		}
		Rigidbody rb = GetComponent<Rigidbody>();
		if (rb.velocity.magnitude > maxSpeed)
		{
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}
    }

	public void setTimer(float t)
	{
		timer = t;
	}
}
