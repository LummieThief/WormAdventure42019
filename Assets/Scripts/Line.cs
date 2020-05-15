using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	private LineRenderer line;
	public Transform target;
    // Start is called before the first frame update
    void Start()
    {
		line = GetComponent<LineRenderer>();
		line.useWorldSpace = true;

    }

    // Update is called once per frame
    void Update()
    {
		line.SetPosition(0, transform.position);
		line.SetPosition(1, target.transform.position);
	}
}
