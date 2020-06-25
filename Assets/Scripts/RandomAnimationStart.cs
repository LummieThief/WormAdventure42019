using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomAnimationStart : MonoBehaviour
{
	private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
		anim.Play("Idle", -1, Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
