using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
	private Image fadeImage;
    // Start is called before the first frame update
    void Awake()
    {
		fadeImage = GetComponent<Image>();
		fadeImage.enabled = true;
    }

	public void startFadeIn(float fadePerSecond)
	{
		StopCoroutine("fadeIn");
		StopCoroutine("fadeOut");
		StartCoroutine(fadeIn(fadePerSecond));
	}
	public void startFadeIn(float fadePerSecond, bool resetAlpha)
	{
		StopCoroutine("fadeIn");
		StopCoroutine("fadeOut");
		if (resetAlpha)
		{
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
		}
		startFadeIn(fadePerSecond);
	}

	public void startFadeOut(float fadePerSecond)
	{
		StopCoroutine("fadeIn");
		StopCoroutine("fadeOut");
		StartCoroutine(fadeOut(fadePerSecond));
	}
	public void startFadeOut(float fadePerSecond, bool resetAlpha)
	{
		StopCoroutine("fadeIn");
		StopCoroutine("fadeOut");
		if (resetAlpha)
		{
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
		}
		startFadeOut(fadePerSecond);
	}

	IEnumerator fadeIn(float fadePerSecond)
	{
		while (fadeImage.color.a > 0)
		{
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a - fadePerSecond * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator fadeOut(float fadePerSecond)
	{
		while (fadeImage.color.a < 1)
		{
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a + fadePerSecond * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}

}
