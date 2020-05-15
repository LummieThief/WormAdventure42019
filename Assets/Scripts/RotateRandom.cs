using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		transform.rotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
