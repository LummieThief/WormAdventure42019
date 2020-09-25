using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWin : MonoBehaviour
{
	public static bool hasWon;
	private Game game;
	private WormMove worm;
	private bool grappleHitIn = false;
	private GameObject grappleHit;
	public float radius;
	public GameObject finishMenu;

	// Start is called before the first frame update
	private void Start()
	{
		game = FindObjectOfType<Game>();
		worm = FindObjectOfType<WormMove>();
	}
	private void Update()
	{
		if (worm == null)
		{
			worm = FindObjectOfType<WormMove>();
		}


		if (!hasWon && !game.getStartingGrapple() && grappleHit != null && grappleHit.activeSelf)
		{
			float distance = Vector3.Distance(transform.position, grappleHit.transform.position);
			if (distance < radius && worm.getGrappling())
			{
				win();
			}
		}
		

	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "GrappleHit")
		{
			grappleHit = other.gameObject;
			Debug.Log("grapple in");
		}
	}

	private void win()
	{
		hasWon = true;
		StartCoroutine(fadeInAFew(0.5f, 0.5f, true, Color.black));
		Debug.Log("Winners");
		FindObjectOfType<SoundManager>().playIdle();
		FinishMenuWormhole.active = true;
	}

	public void toggleWin()
	{
		hasWon = !hasWon;
	}

	private IEnumerator fadeInAFew(float seconds, float fadePerSecond, bool resetAlpha, Color color)
	{
		yield return new WaitForSeconds(seconds);
		FindObjectOfType<FadeController>().startFadeOut(0.5f, true, Color.black);
		yield return new WaitForSeconds(1 / fadePerSecond);
		finishMenu.SetActive(true);
		
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
