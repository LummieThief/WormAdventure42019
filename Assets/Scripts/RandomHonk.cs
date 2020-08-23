using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHonk : MonoBehaviour
{
	public float probability = 0.01f;
	public float delay = 5f;
	private float timer = 0f;

	private SoundManager sm;
	private void Start()
	{
		sm = FindObjectOfType<SoundManager>();
	}
	void FixedUpdate()
    {
		if (!StartMenu.isOpen)
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else if (Random.Range(0f, 1f) < probability)
			{
				sm.playHonk(Random.Range(0, sm.honks.Length));
				timer = delay;
			}
		}
		
    }
}
