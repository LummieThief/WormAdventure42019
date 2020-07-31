using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInside : MonoBehaviour
{
	public Collider other;
    // Update is called once per frame
    void Update()
    {
		Debug.Log(other.ClosestPoint(transform.position) == transform.position);
	}
}
