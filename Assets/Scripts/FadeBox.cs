using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeBox : MonoBehaviour
{
	public bool fadeIn = true;
	public bool directional;
	public Color color = Color.black;
	private WormMove wm;

	public float fadeSpeed;

	private FadeController fade;
	private bool running;
	private bool orderSixtySix;
	private SoundManager sm;
	// Start is called before the first frame update
	private void Awake()
	{
		fade = FindObjectOfType<FadeController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (color == Color.white && SceneManager.GetActiveScene().name == "Winners")
			{
				running = true;
			}
			if (fade == null)
			{
				fade = FindObjectOfType<FadeController>();
			}
			if (directional)
			{
				if (other.GetComponent<Rigidbody>().velocity.y < 0)
				{
					fade.startFadeIn(fadeSpeed, color);
				}
				else
				{
					fade.startFadeOut(fadeSpeed, color);
				}
			}
			else
			{
				if (fadeIn)
				{
					fade.startFadeIn(fadeSpeed, color);
				}
				else
				{
					fade.startFadeOut(fadeSpeed, color);
				}
			}
		}
	}

	private void Update()
	{
		if (running)
		{
			if (!orderSixtySix)
			{
				foreach (WindAbove wa in FindObjectsOfType<WindAbove>())
				{
					wa.enabled = false;
				}
				orderSixtySix = true;
			}
			if (wm == null)
			{
				wm = FindObjectOfType<WormMove>();
			}
			if (sm == null)
			{
				sm = FindObjectOfType<SoundManager>();
				if (sm != null)
				{
					sm.winnersMusicFade(-0.3f);
				}
			}
			wm.setArtificialWindSpeed(-1);
			Debug.Log("set a new wind speed");
		}
	}
}
