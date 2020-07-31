using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
	[Header("UI elements")]
	public Dropdown resolutionDropdown;
	public Slider sensitivitySlider;
	public Toggle showSignsToggle;
	public Toggle vsyncToggle;
	public Toggle playInBackgroundToggle;
	public Toggle framerateToggle;
	public Dropdown textureQualityDropdown;
	public Slider soundEffectsSlider;
	[Header("Other variables")]
	public GameObject optionsMenuUI;
	public GameObject startMenuUI;
	public GameObject pauseMenuUI;
	public AudioMixer soundEffectsMixer;

	public static bool signsEnabled;
	private bool inFullscreen = true;
	private Vector2[] resolutions;
	private FPSDisplay fps;

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
		resolutionDropdown.RefreshShownValue();

		
		if (!PlayerPrefs.HasKey("Resolution"))
		{
			resolutionDropdown.value = currentResolutionIndex;
			PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
		}
		

		load();
	}




	public void Back()
	{
		optionsMenuUI.SetActive(false);
		if (FindObjectOfType<Game>() != null && FindObjectOfType<Game>().getStartingGrapple())
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
		PlayerPrefs.SetInt("Resolution", value);
		//Debug.Log(value);
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
		PlayerPrefs.SetString("VSync", "" + value);
	}

	public void TextureQuality(int value)
	{
		QualitySettings.masterTextureLimit = value;
		PlayerPrefs.SetInt("TextureQuality", value);
	}

	public void PlayInBackground(bool value)
	{
		Application.runInBackground = value;
		PlayerPrefs.SetString("PlayInBackground", "" + value);
	}
	/*
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
	*/

	public void ShowFrameRate(bool value)
	{
		if (fps == null)
		{
			fps = FindObjectOfType<FPSDisplay>();
		}
		if (value)
		{
			fps.enabled = true;
		}
		else
		{
			fps.enabled = false;
		}
		PlayerPrefs.SetString("ShowFrameRate", "" + value);
	}

	public void ShowHintSigns(bool value)
	{
		signsEnabled = value;
		PlayerPrefs.SetString("ShowHintSigns", "" + value);
	}

	public void sensitivityChange(float newSens)
	{
		FindObjectOfType<CameraFollow>().setSensitivity(newSens / 2.0f);
		PlayerPrefs.SetInt("Sensitivity", (int)newSens);
	}

	public void soundEffectsVolume(float newVol)
	{
		soundEffectsMixer.SetFloat("Volume", Mathf.Log10(newVol) * 20);
		PlayerPrefs.SetFloat("SoundEffectsVolume", newVol);
		Debug.Log("New volume is " + newVol);
	}

	private void load()
	{
		/*
		public Dropdown resolutionDropdown;
		public Slider sensitivitySlider;
		public Toggle showSignsToggle;
		public Toggle vsyncToggle;
		public Toggle playInBackgroundToggle;
		public Toggle framerateToggle;
		*/

		if (PlayerPrefs.HasKey("Sensitivity"))
			sensitivitySlider.value = PlayerPrefs.GetInt("Sensitivity");
		if (PlayerPrefs.HasKey("ShowHintSigns"))
			showSignsToggle.isOn = toBool(PlayerPrefs.GetString("ShowHintSigns"));
		if (PlayerPrefs.HasKey("VSync"))
			vsyncToggle.isOn = toBool(PlayerPrefs.GetString("VSync"));
		if (PlayerPrefs.HasKey("PlayInBackground"))
			playInBackgroundToggle.isOn = toBool(PlayerPrefs.GetString("PlayInBackground"));
		if (PlayerPrefs.HasKey("ShowFrameRate"))
			framerateToggle.isOn = toBool(PlayerPrefs.GetString("ShowFrameRate"));
		if (PlayerPrefs.HasKey("TextureQuality"))
			textureQualityDropdown.value = PlayerPrefs.GetInt("TextureQuality");
		if (PlayerPrefs.HasKey("Resolution"))
			resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
		if (PlayerPrefs.HasKey("SoundEffectsVolume"))
		{
			soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");
			Debug.Log("Has");
		}
		else
		{
			Debug.Log("doesnt have");
		}
		//Debug.Log(PlayerPrefs.GetInt("Resolution"));
	}

	private bool toBool(string s)
	{
		return s == "True";
	}

}
