using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowController : MonoBehaviour
{
	public Material mat;
	private float count = 0;
	public float increment = 0.1f;
    // Update is called once per frame
    void Update()
    {
		count += increment * Time.unscaledDeltaTime;
		mat.SetFloat("_hue", count % 1);
		
    }
}
