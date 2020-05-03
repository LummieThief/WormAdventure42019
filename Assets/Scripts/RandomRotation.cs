using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
	public Vector3 axis;
    // Start is called before the first frame update
    void Start()
    {
		int rand = Random.Range(0, 6);
		axis *= 60 * rand;
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + axis.x,
												transform.rotation.eulerAngles.y + axis.y,
												transform.rotation.eulerAngles.z + axis.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
