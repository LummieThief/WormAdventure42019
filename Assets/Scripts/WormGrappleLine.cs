using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormGrappleLine : MonoBehaviour
{
	private Transform target1;
	private Material mat;
	//private Transform target2;
	private int mainId;
	public LineRenderer line;
	private GameObject localHit;
	public void setUp(Transform t1, Transform t2, int id, Vector3 localPos)
	{
		mat = t1.GetComponentInChildren<MainBodyController>().tail.material;
		line.material = mat;
		target1 = t1.GetComponentInChildren<MainBodyController>().mainBodyCollider.transform;
		Transform target2 = t2.GetComponentInChildren<MainBodyController>().mainBodyCollider.transform;
		mainId = id;

		localHit = new GameObject();
		localHit.transform.parent = target2;
		localHit.transform.localPosition = localPos;
	}
	private void Update()
	{
		if (target1 == null || localHit == null)
		{
			Destroy(this.gameObject);
			return;
		}
		Vector3 pos1 = target1.position + target1.TransformDirection(Vector3.down) * target1.lossyScale.y * 0.9f;
		Vector3 pos2 = localHit.transform.position;
		Vector3[] arr = { pos1, pos2 };
		line.SetPositions(arr);
		//line.material = target1.GetComponentInParent<>
	}
	private void OnDestroy()
	{
		Destroy(localHit);
	}
	public int getMainId()
	{
		return mainId;
	}

	
}
