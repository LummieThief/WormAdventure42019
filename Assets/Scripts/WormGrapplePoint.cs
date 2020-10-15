using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormGrapplePoint : MonoBehaviour
{
	private LineRenderer line;
	private Transform worm;
    // Start is called before the first frame update
    void Start()
    {
		line = GetComponent<LineRenderer>();
		worm = FindObjectOfType<WormMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
		line.positionCount = 2;
		line.SetPosition(0, transform.position);
		line.SetPosition(1, worm.position + worm.TransformDirection(Vector3.down) * worm.lossyScale.y * 0.9f);
	}
}
