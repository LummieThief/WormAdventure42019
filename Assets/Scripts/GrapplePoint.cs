using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
	private GameObject lastPoint;
	private GameObject nextPoint;
	private bool current;
	private int tick;

	public void setLastPoint(GameObject obj)
	{
		lastPoint = obj;
	}

	public void setNextPoint(GameObject obj)
	{
		nextPoint = obj;
	}

	public GameObject getLastPoint()
	{
		return lastPoint;
	}

	public void destroySelf()
	{
		if (lastPoint != null)
		{
			lastPoint.GetComponent<GrapplePoint>().destroySelf();
		}
		GameObject.Destroy(this.gameObject);
	}


	private void LateUpdate()
	{
		if (nextPoint != null)
		{
			LineRenderer line = GetComponent<LineRenderer>();
			line.SetPosition(0, transform.position);
			line.SetPosition(1, nextPoint.transform.position);
		}
		if (current)
		{
			gameObject.layer = 0;
			if (lastPoint != null)
			{
				lastPoint.gameObject.layer = 0;
				GameObject lastLastPoint = lastPoint.GetComponent<GrapplePoint>().lastPoint;
				if (lastLastPoint != null)
				{
					lastLastPoint.layer = 2;
				}
				//lastPoint.GetComponent<GrapplePoint>().gameObject.layer = 2;
			}
		}
	}


	private void FixedUpdate()
	{
		tick++;
		if (tick == 2)
		{
			GetComponent<Rigidbody>().isKinematic = true;
			gameObject.layer = 0;
			if (current)
			{
				GetComponent<SphereCollider>().enabled = false;
			}
			
		}
		transform.rotation = Quaternion.identity;
	}

	public float getTotalDistance()
	{
		if (lastPoint != null)
		{
			float dist = Vector3.Distance(transform.position, lastPoint.transform.position);
			return dist + lastPoint.GetComponent<GrapplePoint>().getTotalDistance();
		}
		else
		{
			return 0;
		}
	}

	public void setCurrent(bool value)
	{
		if (value)
		{
			current = true;
			if (lastPoint != null)
			{
				lastPoint.GetComponent<GrapplePoint>().setCurrent(false);
			}
			if (tick < 2)
			{
				gameObject.layer = 2;
			}
			else
			{
				GetComponent<SphereCollider>().enabled = false;
			}
		}
		else
		{
			current = false;
			if (tick < 2)
			{
				//gameObject.layer = 0;
			}
			else
			{
				GetComponent<SphereCollider>().enabled = true;
			}
		}
	}
}
