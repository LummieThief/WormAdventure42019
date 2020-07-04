using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

	public GameObject optionsMenuUI;
	public GameObject startMenuUI;
	public GameObject pauseMenuUI;
	private bool inFullscreen = true;
	private Vector2[] resolutions;
	public Dropdown resolutionDropdown;

	void Start()
	{
		//Gets rid of refresh rate
		List<Vector2> condensedResolutions = new List<Vector2>();
		foreach (Resolution r in Screen.resolutions)
		{
			Vector2 res = new Vector2(r.width, r.height);
			if (!condensedResolutions.Contains(res))
			{
				condensedResolutions.Add(res);
			}
		}
		resolutions = condensedResolutions.ToArray();

		//Fills out the dropdown
		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].x + " x " + resolutions[i].y;
			options.Add(option);

			if (resolutions[i].x == Screen.currentResolution.width &&
				resolutions[i].y == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();

	}




	public void Back()
	{
		optionsMenuUI.SetActive(false);
		if (FindObjectOfType<Game>().getStartingGrapple())
		{
			startMenuUI.SetActive(true);
		}
		else
		{
			pauseMenuUI.SetActive(true);
		}
	}

	public void Fullscreen()
	{

		Screen.fullScreen = !Screen.fullScreen;
	}

	public void Resolution(int value)
	{
		Vector2 res = resolutions[value];
		Screen.SetResolution((int)res.x, (int)res.y, Screen.fullScreen);
	}

	public void VSync(bool value)
	{
		if (value)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
	}

	public void TextureQuality(int value)
	{
		QualitySettings.masterTextureLimit = value;
	}

	public void PlayInBackground(bool value)
	{
		Application.runInBackground = value;
	}

	public void Particles(bool value)
	{
		if (value)
		{
			foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
			{
				var emission = ps.emission;
				emission.enabled = true;
			}
		}
		else
		{
			foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
			{
				ps.Clear();
				var emission = ps.emission;
				emission.enabled = false;
			}
		}
	}
}
