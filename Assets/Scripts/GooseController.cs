using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseController : MonoBehaviour
{
	private Animator anim;
	private string state;
	public float chanceToStartPecking = 0.01f;
	public float chanceToStopPecking = 0.05f;
    void Start()
    {
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (state == "Pecking") //pecking
		{
			if (Random.value < chanceToStopPecking)
			{
				state = "NotPecking";
				anim.SetBool("Peck", false);
			}
		}
		else //not pecking
		{
			if (Random.value < chanceToStartPecking)
			{
				state = "Pecking";
				anim.SetBool("Peck", true);
			}
		}

	}

	
}
