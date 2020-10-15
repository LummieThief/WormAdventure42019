using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegmentController : MonoBehaviour
{
	public Transform end;

	private Transform[] ropePoints;
	public GameObject ropePointPrefab;
	private LineRenderer line;
	public Transform parent;
	private int pointIndex;
	private Vector3 lastEnd;
	private int maxPoints = 100;

	private float length;

	private bool active;
	public bool manualActivate;

	private int iterations = 4;

	private float timer;

	private void Start()
	{
		ropePoints = new Transform[maxPoints + 5];


		lastEnd = end.position;
		line = GetComponent<LineRenderer>();
		line.useWorldSpace = true;
	}
	private void Update()
	{
		if (manualActivate)
		{
			manualActivate = false;
			activate(end.position);
		}

		if (active) //the below code will only run if the object is active
		{
			
			timer += Time.deltaTime;
			//Debug.Log(timer);

			Physics.SyncTransforms();
			Transform currentPoint = ropePoints[pointIndex];
			RaycastHit hit;
			Collider block;

			LayerMask blockMask = 1 << 8;

			//first we have to check that the end of the worm is not clipped into the wall.
			var proceed = true;
			if (Physics.Linecast(currentPoint.position, end.position, out hit, blockMask)) //tests if the worms butt is clipped in the wall
			{
				if (CollidesWith(end.position, hit.collider))
				{
					proceed = false;
					Debug.Log("butt was clipped");
				}
			}

			if (pointIndex < maxPoints && proceed) //if its safe to make more points
			{
				if (Physics.Linecast(end.position, currentPoint.position, out hit, blockMask)) //fires a ray from the last point of the rope to the worm
				{
					block = hit.collider;
					Vector3 finalPoint = hit.point;
					for (int i = 0; i <= iterations; i++)
					{
						float distance = Vector3.Distance(end.position, lastEnd);
						Vector3 newPoint = Vector3.MoveTowards(end.position, lastEnd, (distance / iterations) * i);
						Vector3 highestHit = hit.point;

						if (Physics.Linecast(newPoint, currentPoint.position, out hit, blockMask))
						{
							highestHit = hit.point;
							continue;
						}
						distance = Vector3.Distance(newPoint, highestHit);
						finalPoint = Vector3.MoveTowards(newPoint, currentPoint.position, distance);
						break;
					}

					if (!CollidesWith(finalPoint, block) && Vector3.Distance(finalPoint, ropePoints[pointIndex].position) > 0.1f)
					{
						ropePoints[pointIndex + 1] = Instantiate(ropePointPrefab, finalPoint, Quaternion.identity).transform;
						pointIndex++;
						length += Vector3.Distance(ropePoints[pointIndex].position, ropePoints[pointIndex - 1].position);
						transform.position = finalPoint;
					}

				}
			}

			if (pointIndex > 0)
			{
				int oldLayer = gameObject.layer;
				gameObject.layer = 2; //ignore raycast
				LayerMask mask = 11 << 8;
				if (Physics.Linecast(ropePoints[pointIndex - 1].position, end.position, out hit, mask))
				{
					
					if (hit.collider.gameObject.layer == 9)
					{
						
						if (!sweepArea(end.position, ropePoints[pointIndex].position, ropePoints[pointIndex - 1].position, 15))
						{
							transform.position = ropePoints[pointIndex].position;
							length -= Vector3.Distance(ropePoints[pointIndex].position, ropePoints[pointIndex - 1].position);
							Destroy(ropePoints[pointIndex].gameObject);
							pointIndex--;
							
						}
					}

				}
				gameObject.layer = oldLayer;
			}
		}
		lastEnd = end.position;
	}

	private void LateUpdate()
	{
		updateLineRenderer();
	}

	private void updateLineRenderer()
	{
		if (active)
		{
			Vector3[] vertices = new Vector3[pointIndex + 2];
			vertices[0] = end.position;
			for (int i = 0; i <= pointIndex; i++)
			{
				vertices[i + 1] = ropePoints[pointIndex - i].position;
			}
			line.positionCount = vertices.Length;
			line.SetPositions(vertices);
		}
		else
		{
			line.positionCount = 0;
		}
	}


	private bool CollidesWith(Vector3 point, Collider other)
	{
		return (other.ClosestPoint(point) - point).sqrMagnitude < Mathf.Epsilon * Mathf.Epsilon;
	}

	public void activate(Vector3 startingPosition)
	{
		timer = 0;
		transform.position = startingPosition;
		ropePoints[0] = Instantiate(ropePointPrefab, transform.position, Quaternion.identity).transform;
		pointIndex = 0;
		active = true;
	}

	public void deactivate()
	{
		destroyPoints();
		length = 0;
		active = false;
	}

	private void destroyPoints()
	{
		foreach (Transform t in ropePoints)
		{
			if (t != null)
			{
				Destroy(t.gameObject);
			}
		}
	}


	private bool sweepArea(Vector3 origin, Vector3 start, Vector3 end, int iterations) //returns whether there is a collider in the triangle formed by the three vectors.
	{
		start = Vector3.MoveTowards(start, origin, 0.2f);
		end = Vector3.MoveTowards(end, origin, 0.2f);
		Vector3 raycastLocation = start;
		float itrDelta = Vector3.Distance(start, end) / iterations;

		RaycastHit hitt;
		for (int i = 0; i < iterations; i++)
		{
			raycastLocation = Vector3.MoveTowards(raycastLocation, end, itrDelta);
			raycastLocation = Vector3.MoveTowards(raycastLocation, origin, Mathf.Sin(Mathf.PI * i / iterations));
			int layerMask = 1 << 8; //only blocks
			Debug.DrawLine(origin, raycastLocation);
			if (Physics.Linecast(origin, raycastLocation, out hitt, layerMask))
			{
				return true;
			}

		}
		return false;
	}


	public float getLength()
	{
		return length;
	}

	public Transform getCurrentPoint()
	{
		return ropePoints[pointIndex];
	}

	public float getTime()
	{
		return timer;
	}

	public void setTime(float time)
	{
		timer = time;
	}
}
