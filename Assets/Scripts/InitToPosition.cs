using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitToPosition : MonoBehaviour
{
	public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
		transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
