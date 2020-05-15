using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtPosition : MonoBehaviour
{
	public Transform worm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		transform.position = worm.position + worm.TransformDirection(Vector3.down) * worm.localScale.y * 0.9f;
	}
}
