using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceStopper : MonoBehaviour
{
	public Rigidbody wormRB;
	public int instanceID;

	public static bool zeroIn;
	public static bool oneIn;

	private float bounceSpeed = 5f;

	private float flatTolerance = 15;
	private float flatBelly = 90;
	private float flatBack = 270;

	private bool canBounce = false;
	private float timer;
	private float timeBetweenHits = 0.1f;

	private void Update()
	{
		Vector3 rot = wormRB.transform.eulerAngles;
		//Debug.Log(rot.x);
		bool wormIsFlat = (rot.x > flatBelly - flatTolerance && rot.x < flatBelly + flatTolerance) || 
							(rot.x > flatBack - flatTolerance && rot.x < flatBack + flatTolerance);
		bool wormIsFast = Mathf.Abs(wormRB.velocity.y) > bounceSpeed;

		//Debug.Log(wormIsFast);
		canBounce = wormIsFlat && wormIsFast;

		switch (instanceID)
		{
			case 0:
				if (zeroIn)
				{
					timer += Time.deltaTime;
					if (timer > timeBetweenHits)
					{
						timer = 0;
						zeroIn = false;
					}
				}
				break;
			case 1:
				if (oneIn)
				{
					timer += Time.deltaTime;
					if (timer > timeBetweenHits)
					{
						timer = 0;
						oneIn = false;
					}
				}
				break;
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 8)
		{
			if (instanceID == 0)
			{
				zeroIn = true;
				if (oneIn && canBounce)
				{
					wormRB.velocity = new Vector3(wormRB.velocity.x, 0, wormRB.velocity.z);
					wormRB.angularVelocity = Vector3.zero;
					//wormRB.angularVelocity /= 3;
					zeroIn = false;
					oneIn = false;
					Debug.Log("stopping bounce");
				}
			}
			else
			{
				oneIn = true;
				if (zeroIn && canBounce)
				{
					wormRB.velocity = new Vector3(wormRB.velocity.x, 0, wormRB.velocity.z);
					wormRB.angularVelocity = Vector3.zero;
					//wormRB.angularVelocity /= 3;
					zeroIn = false;
					oneIn = false;
					Debug.Log("stopping bounce");
				}
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 8)
		{
			if (instanceID == 0)
			{
				zeroIn = false;
			}
			else
			{
				oneIn = false;
			}
		}
	}
}
