using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBox : MonoBehaviour
{
	public bool fadeIn = true;
	public bool directional;
	public Color color = Color.black;

	public float fadeSpeed;

	private FadeController fade;
	// Start is called before the first frame update
	private void Awake()
	{
		fade = FindObjectOfType<FadeController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
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
}
