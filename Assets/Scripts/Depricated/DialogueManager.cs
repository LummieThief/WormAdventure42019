using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

	public ParticleSystem oldTextAlert;
	public ParticleSystem newTextAlert;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public ParticleSystem getOldTextAlert()
	{
		return oldTextAlert;
	}

	public ParticleSystem getNewTextAlert()
	{
		return newTextAlert;
	}
}
