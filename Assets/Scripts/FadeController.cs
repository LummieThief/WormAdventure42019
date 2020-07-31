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
		if (PlayerPrefs.GetInt("FromWhite") == 1)
		{
			fadeImage.color = Color.white;
		}
		fadeImage.enabled = true;
    }

	public void startFadeIn(float fadePerSecond)
	{
		startFadeIn(fadePerSecond, Color.black);
	}
	public void startFadeIn(float fadePerSecond, Color color)
	{
		fadeImage.color = new Color(color.r, color.g, color.b, fadeImage.color.a);
		StopCoroutine("fadeIn");
		StopCoroutine("fadeOut");
		StartCoroutine(fadeIn(fadePerSecond));
	}

	public void startFadeIn(float fadePerSecond, bool resetAlpha)
	{
		startFadeIn(fadePerSecond, resetAlpha, Color.black);
	}
	public void startFadeIn(float fadePerSecond, bool resetAlpha, Color color)
	{
		if (resetAlpha)
		{
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
		}
		startFadeIn(fadePerSecond, color);
	}


	public void startFadeOut(float fadePerSecond)
	{
		startFadeOut(fadePerSecond, Color.black);
	}
	public void startFadeOut(float fadePerSecond, Color color)
	{
		fadeImage.color = new Color(color.r, color.g, color.b, fadeImage.color.a);
		StopCoroutine("fadeIn");
		StopCoroutine("fadeOut");
		StartCoroutine(fadeOut(fadePerSecond));
	}

	public void startFadeOut(float fadePerSecond, bool resetAlpha)
	{
		startFadeOut(fadePerSecond, resetAlpha, Color.black);
	}
	public void startFadeOut(float fadePerSecond, bool resetAlpha, Color color)
	{
		if (resetAlpha)
		{
			fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
		}
		startFadeOut(fadePerSecond, color);
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
