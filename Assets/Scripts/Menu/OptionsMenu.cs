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
	public Dropdown targetFrameRateDropdown;
	public Slider soundEffectsSlider;
	public Slider musicSlider;
	public Slider ambienceSlider;
	public Dropdown regionDropdown;
	[Header("Other variables")]
	public GameObject optionsMenuUI;
	public GameObject startMenuUI;
	public GameObject pauseMenuUI;
	public GameObject resetConfirmMenuUI;
	public AudioMixer soundEffectsMixer;
	public AudioMixer musicMixer;
	public AudioMixer ambienceMixer;


	public static bool signsEnabled;
	private bool inFullscreen = true;
	private Vector2[] resolutions;
	private int[] frameRates = {30, 60, 120, 144, 240, -1};
	private FPSDisplay fps;
	private bool achieved = false;
	//private MultiplayerMenu mm;

	void Start()
	{
		//mm = FindObjectOfType<MultiplayerMenu>();
		//Debug.Log(mm);

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
		Application.targetFrameRate = 60;
		Debug.Log("setting target frame rate here");
		load();
	}

	private void Update()
	{
		if (sensitivitySlider.value == sensitivitySlider.maxValue && !PauseMenu.isPaused && !StartMenu.isOpen && !achieved)
		{
			achieved = true;
			AchievementManager.Achieve("ACH_SENSITIVITY");
		}
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

	public void ResetSave()
	{
		resetConfirmMenuUI.SetActive(true);
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

	public void TargetFrameRate(int value)
	{
		Application.targetFrameRate = frameRates[value];
		PlayerPrefs.SetInt("TargetFrameRate", value);
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
		//Debug.Log("show frame rate");
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
		float halfOfMax = sensitivitySlider.maxValue / 2;
		if (newSens > halfOfMax)
		{

			newSens *= 2 - (sensitivitySlider.maxValue - newSens) / halfOfMax;
		}

		Debug.Log(newSens);
		FindObjectOfType<CameraFollow>().setSensitivity(newSens / 2.0f);
		PlayerPrefs.SetInt("Sensitivity", (int)newSens);
	}

	public void soundEffectsVolume(float newVol)
	{
		soundEffectsMixer.SetFloat("Volume", Mathf.Log10(newVol) * 20);
		PlayerPrefs.SetFloat("SoundEffectsVolume", newVol);
		//Debug.Log("New volume is " + newVol);
	}
	public void musicVolume(float newVol)
	{
		musicMixer.SetFloat("Volume", Mathf.Log10(newVol) * 20);
		PlayerPrefs.SetFloat("MusicVolume", newVol);
		//Debug.Log("New volume is " + newVol);
	}
	public void ambienceVolume(float newVol)
	{
		ambienceMixer.SetFloat("Volume", Mathf.Log10(newVol) * 20);
		PlayerPrefs.SetFloat("AmbienceVolume", newVol);
		//Debug.Log("New volume is " + newVol);
	}

	public void regionSelector(int value)
	{
		
		PlayerPrefs.SetInt("Region", value);
		string token = "";
		switch (value)
		{
			case 1:
				token = "asia";
				break;
			case 2:
				token = "au";
				break;
			case 3:
				token = "cae";
				break;
			case 4:
				token = "eu";
				break;
			case 5:
				token = "in";
				break;
			case 6:
				token = "jp";
				break;
			case 7:
				token = "ru";
				break;
			case 8:
				token = "rue";
				break;
			case 9:
				token = "za";
				break;
			case 10:
				token = "sa";
				break;
			case 11:
				token = "kr";
				break;
			case 12:
				token = "us";
				break;
			case 13:
				token = "usw";
				break;
			default:
				token = "";
				break;

		}

		PlayerPrefs.SetString("Region Token", token);
		
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
		//if (PlayerPrefs.HasKey("TextureQuality"))
			//textureQualityDropdown.value = PlayerPrefs.GetInt("TextureQuality");
		if (PlayerPrefs.HasKey("TargetFrameRate"))
			targetFrameRateDropdown.value = PlayerPrefs.GetInt("TargetFrameRate");
		if (PlayerPrefs.HasKey("Resolution"))
			resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
		if (PlayerPrefs.HasKey("SoundEffectsVolume"))
			soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");
		if (PlayerPrefs.HasKey("MusicVolume"))
			musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		if (PlayerPrefs.HasKey("AmbienceVolume"))
			ambienceSlider.value = PlayerPrefs.GetFloat("AmbienceVolume");
		if (PlayerPrefs.HasKey("Region"))
			regionDropdown.value = PlayerPrefs.GetInt("Region");
		//Debug.Log(PlayerPrefs.GetInt("Resolution"));
	}

	private bool toBool(string s)
	{
		return s == "True";
	}


}
