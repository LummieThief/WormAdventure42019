using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableFog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		RenderSettings.fog = true;
    }

	private void FixedUpdate()
	{
		RenderSettings.fog = true;
	}

}
