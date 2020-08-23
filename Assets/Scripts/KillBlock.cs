using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KillBlock : MonoBehaviour
{
	public int value;
	private float indent = 0.1f;
	private GameObject transition;
	private Animator animator;
	private bool active = true;

	private void Start()
	{
		transition = FindObjectOfType<LoadNextScene>().gameObject;
		animator = transition.GetComponent<Animator>();

		Vector3 newSize = transform.localScale - Vector3.one * indent;
		newSize = new Vector3(newSize.x / transform.localScale.x, newSize.y / transform.localScale.y, newSize.z / transform.localScale.z);
		GetComponent<BoxCollider>().size = newSize;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (active)
		{
			if (other.gameObject.tag == "Player" || other.gameObject.tag == "Friction Point")
			{
				
				WormMove worm = other.gameObject.GetComponentInParent<WormMove>();
				
				if (!worm.getDead())
				{
					SoundManager sm = FindObjectOfType<SoundManager>();
					sm.playDeath();
					FindObjectOfType<CameraFollow>().setState(2);
					worm.playExplosion();
					animator.SetBool("Closing", true);
					worm.setDead(true);
					active = false;
				}
			}
		}
	}
}
