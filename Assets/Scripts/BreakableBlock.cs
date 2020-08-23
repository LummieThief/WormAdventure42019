using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
	private BoxCollider bc;
	private ParticleSystem[] pss;
	private bool broken = false;
	private float particleScaleFactor = 0.2f;
	private float startY;
    // Start is called before the first frame update
    void Start()
    {
		startY = transform.position.y;
		pss = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
		if (transform.position.y != startY)
		{
			if (!broken)
			{
				FindObjectOfType<SoundManager>().playWoodCrash();
				transform.position -= Vector3.up * 200;
				foreach (ParticleSystem ps in pss)
				{
					ps.startSize *= (transform.localScale.x + transform.localScale.z) / 2f * particleScaleFactor;
					ps.Play();
					
				}
				GetComponent<BoxCollider>().enabled = false;
				broken = true;
			}
		}
    }
}
