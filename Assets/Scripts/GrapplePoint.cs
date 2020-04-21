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
			if (lastPoint != null)
			{
				GameObject lastLastPoint = lastPoint.GetComponent<GrapplePoint>().lastPoint;
			}
		}
	}


	private void FixedUpdate()
	{
		tick++;
		if (tick == 2)
		{
			GetComponent<Rigidbody>().isKinematic = true;
			if (current)
			{
				GetComponent<SphereCollider>().enabled = false;
			}
			
		}
		transform.rotation = Quaternion.identity;
		
		
	}
	private void Update()
	{
		readjust();
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
			else
			{
				GetComponent<SphereCollider>().enabled = false;
			}
		}
		else
		{
			current = false;
			GetComponent<SphereCollider>().enabled = true;
		}
	}

	private void readjust()
	{
		float scaleFactor = 0.1f;
		float iterations = 15;
		int pointsPerSphere = 32;
		Vector3[] sphere = PointsOnSphere(pointsPerSphere);
		bool _break = false;
		if (lastPoint != null && current)
		{
			//Debug.Log("1");
			RaycastHit hit;
			if (Physics.Linecast(transform.position, lastPoint.transform.position, out hit))
			{
				//Debug.Log("2");
				if (hit.collider.gameObject.tag != "GP")
				{
					//Debug.Log(hit.collider.gameObject);
					//ok now you can actuall run the MEAt of the meathod
					for (int i = 0; i < iterations; i++)
					{
						
						foreach (Vector3 p in sphere)
						{
							Vector3 point = transform.position + p * i * scaleFactor;
							if (!Physics.CheckSphere(point, 0.2f))
							{

								RaycastHit hit2;
								if (Physics.Linecast(point, lastPoint.transform.position, out hit2))
								{
									//Debug.Log(hit2.collider.gameObject.tag);
									if (hit2.collider.gameObject.tag == "GP")
									{
										//point = Vector3.MoveTowards(point, lastPoint.transform.position, Vector3.Distance(point, lastPoint.transform.position) / 2);
										transform.position = point;
										//transform.position = point;
										//Debug.Log("bb");
										//Debug.DrawLine(point, lastPoint.transform.position);
										_break = true;
										if (_break)
											break;
									}

								}
							}
							if (_break)
								break;
						}
						if (_break)
							break;
					}

				}
			}
		}
	}

	private Vector3[] PointsOnSphere(int n)
	{
		List<Vector3> upts = new List<Vector3>();
		float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
		float off = 2.0f / n;
		float x = 0;
		float y = 0;
		float z = 0;
		float r = 0;
		float phi = 0;

		for (var k = 0; k < n; k++)
		{
			y = k * off - 1 + (off / 2);
			r = Mathf.Sqrt(1 - y * y);
			phi = k * inc;
			x = Mathf.Cos(phi) * r;
			z = Mathf.Sin(phi) * r;

			upts.Add(new Vector3(x, y, z));
		}
		Vector3[] pts = upts.ToArray();
		return pts;
	}

	public bool getCurrent()
	{
		return current;
	}
}


