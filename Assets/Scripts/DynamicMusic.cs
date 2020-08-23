using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicMusic : MonoBehaviour
{
	private Transform worm;
	private Rigidbody wormRB;
	private Transform windAbove;

	private SoundManager sm;

	private Queue<Vector3> velocities;

	private float timer;
	private float timeBetweenSamples = 1f;

	private int velCapacity = 20;

	private float threshold = 13f;

	private float speedToTurnOn = 6f;
	private float speedToTurnOff = 8f;

	private void Start()
	{
		velocities = new Queue<Vector3>();
		worm = FindObjectOfType<WormMove>().transform;
		wormRB = worm.GetComponent<Rigidbody>();
		sm = FindObjectOfType<SoundManager>();
		windAbove = FindObjectOfType<WindAbove>().transform;
	}

	private void Update()
	{
		if (StartMenu.isOpen)
			return;
		if (PauseMenu.isPaused)
		{
			if (sm.levelMusic.isPlaying)
			{
				sm.levelMusic.Stop();
				sm.levelMusic.volume = 0;
				velocities.Clear();
			}
			return;
		}

		timer += Time.deltaTime;
		if (timer > timeBetweenSamples)
		{
			timer = 0;


			
			Vector3 velToEnqueue = new Vector3(Mathf.Abs(wormRB.velocity.x), wormRB.velocity.y, Mathf.Abs(wormRB.velocity.z));

			velocities.Enqueue(velToEnqueue);
			if (velocities.Count > velCapacity)
			{
				velocities.Dequeue();
			}


			Vector3 averageVelocity = Vector3.zero;
			for (int i = 0; i < velocities.Count; i++)
			{
				Vector3 vel = velocities.Dequeue();
				averageVelocity += vel;
				velocities.Enqueue(vel);
			}

			averageVelocity /= velCapacity;

			Debug.Log(averageVelocity.y + averageVelocity.z / 6);

			if (windAbove != null && worm.position.y > windAbove.position.y)
			{
				if (sm.levelMusic.isPlaying)
				{
					sm.levelMusicFade(-0.125f);
				}
			}
			else if (averageVelocity.y + averageVelocity.z / 6 > speedToTurnOn)
			{
				
				if (!sm.levelMusic.isPlaying)
				{
					sm.levelMusicFade(0.075f);
				}
			}
			else if(averageVelocity.y + averageVelocity.z < speedToTurnOff)
			{
				
				if (sm.levelMusic.isPlaying)
				{
					sm.levelMusicFade(-0.2f);
				}
			}
		}


		
	}

}
