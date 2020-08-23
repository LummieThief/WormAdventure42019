using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	private float pauseVolumeScale = 0.1f;
	private bool wasPaused;

	public AudioSource scrunch;
	public AudioSource descrunch;
	public AudioSource grass;
	public AudioSource wood;
	public AudioSource[] jumps;
	public AudioSource death;
	public AudioSource speedWind;
	public AudioSource grapple;
	public AudioSource win;
	public AudioSource wormholeWind;
	public AudioSource ready;
	public AudioSource go;
	public AudioSource woodCrash;

	public AudioSource[] honks;

	public AudioSource mouseDown;
	public AudioSource mouseUp;
	public AudioSource mouseEnter;

	public AudioSource caveAmbience;
	public AudioSource wormholeAmbience;


	public AudioSource menuMusic;
	private float menuMax;

	public AudioSource caveMusic;
	private float caveMax;
	public AudioSource wormholeMusic;
	private float wormholeMax;

	public AudioSource levelMusic;
	private float levelMax;

	private IEnumerator levelCoroutine;

	public AudioSource winnersMusic;
	private float winnersMax;
	private IEnumerator winnersCoroutine;

	public AudioSource ranchMusic;
	private float ranchMax;
	private IEnumerator ranchCoroutine;

	public AudioSource arcadeOneMusic;
	private float arcadeOneMax;

	private float speedWindMaxVolume;

	private void Start()
	{
		speedWindMaxVolume = speedWind.volume;
		speedWind.volume = 0;


		//menuMusic.Play();
		if (!SceneManager.GetActiveScene().name.Contains("Level"))
		{
			menuMax = menuMusic.volume;
			menuMusic.volume = 0;

			caveMax = caveMusic.volume;
			caveMusic.volume = 0;

			wormholeMax = wormholeMusic.volume;
			wormholeMusic.volume = 0;

			winnersMax = winnersMusic.volume;
			winnersMusic.volume = 0;

			ranchMax = ranchMusic.volume;
			ranchMusic.volume = 0;



			menuMusicFade(0.15f);
		}
		else
		{
			arcadeOneMax = arcadeOneMusic.volume;
		}
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "Attempt 2")
		{
			if (!caveAmbience.isPlaying && !StartMenu.isOpen)
			{
				wormholeAmbience.Stop();
				caveAmbience.Play();
			}
			levelMusic = caveMusic;
			levelMax = caveMax;
		}
		else if (SceneManager.GetActiveScene().name == "Attempt 3")
		{
			if (!wormholeAmbience.isPlaying && !StartMenu.isOpen)
			{
				wormholeAmbience.Play();
				caveAmbience.Stop();
			}
			levelMusic = wormholeMusic;
			levelMax = wormholeMax;
		}
		else if (SceneManager.GetActiveScene().name == "Winners")
		{
			wormholeAmbience.Stop();
			if (!winnersMusic.isPlaying && !StartMenu.isOpen)
			{
				winnersMusicFade(0.1f);
			}
		}
		else if (SceneManager.GetActiveScene().name == "Ranch")
		{
			wormholeAmbience.Stop();
			if (!ranchMusic.isPlaying && !StartMenu.isOpen)
			{
				ranchMusicFade(0.1f);
			}
		}



		if (PauseMenu.isPaused)
		{
			wasPaused = true;

			if (!SceneManager.GetActiveScene().name.Contains("Level"))
			{
				if (winnersMusic.isPlaying)
				{
					StopCoroutine(winnersCoroutine);
					winnersMusic.volume = winnersMax * pauseVolumeScale;
				}
				if (menuMusic.isPlaying)
				{
					menuMusic.Stop();
					menuMusic.volume = 0;
					StopCoroutine("menuMusicFade");
				}
				if (ranchMusic.isPlaying)
				{
					StopCoroutine(ranchCoroutine);
					ranchMusic.volume = ranchMax * pauseVolumeScale;
				}

			}
			else
			{
				if (arcadeOneMusic.isPlaying)
				{
					arcadeOneMusic.volume = arcadeOneMax * pauseVolumeScale;
				}

			}
			

		}
		else if (wasPaused)
		{
			wasPaused = false;
			
			if (!SceneManager.GetActiveScene().name.Contains("Level"))
			{
				if(winnersMusic.isPlaying)
					winnersMusic.volume = winnersMax;
				if(ranchMusic.isPlaying)
					ranchMusic.volume = ranchMax;
				
			}
			else
			{
				arcadeOneMusic.volume = arcadeOneMax;
			}
		}
	}

	public void playScrunch()
	{
		scrunch.Play();
	}
	public void stopScrunch()
	{
		scrunch.Stop();
	}
	public void playDescrunch()
	{
		descrunch.Play();
	}
	public void stopDescrunch()
	{
		descrunch.Stop();
	}
	public void playJump(int index)
	{
		jumps[index].Play();
	}
	public void playGrass()
	{
		grass.Play();
	}
	public void playDeath()
	{
		death.Play();
	}

	public void playSpeedWind()
	{
		speedWind.Play();
	}
	public void playSpeedWind(float vol)
	{
		speedWind.volume = speedWindMaxVolume * vol;
	}
	public void playGrapple()
	{
		playGrapple(1);
	}
	public void playGrapple(float pitch)
	{
		grapple.Play();
		grapple.pitch = pitch;
	}
	public void playWood()
	{
		wood.Play();
	}

	public void playWin()
	{
		win.Play();
	}

	public void setWindVolume(float volume)
	{
		if (wormholeWind != null)
		{
			wormholeWind.volume = volume;
		}
	}
	public float getWindVolume()
	{
		return wormholeWind.volume;
	}

	public void playMouseDown()
	{
		mouseDown.Play();
	}

	public void playMouseUp()
	{
		mouseUp.Play();
	}

	public void playMouseEnter()
	{
		mouseEnter.Play();
	}

	public void playReady()
	{
		ready.Play();
	}

	public void playGo()
	{
		go.Play();
	}

	public void playHonk(int index)
	{
		honks[index].Play();
	}

	public void playWoodCrash()
	{
		woodCrash.pitch = Random.Range(0.8f, 1.2f);
		woodCrash.Play();
	}

	public void menuMusicFade(float volPerSec)
	{
		menuMusicFade(volPerSec, false);
	}
	public void menuMusicFade(float volPerSec, bool startFromZero)
	{
		if (!menuMusic.isPlaying && startFromZero)
			menuMusic.volume = 0;
		StopCoroutine("menuMusicFade");
		StartCoroutine(menuMusicFade(menuMusic, menuMax, menuMax * volPerSec));
	}

	IEnumerator menuMusicFade(AudioSource source, float maxVolume, float volPerSec)
	{
		if(!source.isPlaying)
			source.Play();
		while (volPerSec >= 0 && source.volume < maxVolume
			|| volPerSec < 0 && source.volume > 0)
		{
			source.volume += volPerSec * Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame();
		}
		if (source.volume <= 0)
		{
			source.Stop();
		}
	}

	public void levelMusicFade(float volPerSec)
	{
		if (levelCoroutine != null)
			StopCoroutine(levelCoroutine);
		levelCoroutine = levelMusicFade(levelMusic, levelMax, levelMax * volPerSec);
		StartCoroutine(levelCoroutine);
	}

	IEnumerator levelMusicFade(AudioSource source, float maxVolume, float volPerSec)
	{
		if (!source.isPlaying)
			source.Play();
		while ((volPerSec >= 0 && source.volume < maxVolume)
			|| (volPerSec < 0 && source.volume > 0))
		{
			source.volume += volPerSec * Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame();
		}

		if (source.volume <= 0)
		{
			source.Stop();
		}

	}

	public void winnersMusicFade(float volPerSec)
	{
		if (winnersCoroutine != null)
			StopCoroutine(winnersCoroutine);
		winnersCoroutine = winnersMusicFade(winnersMusic, winnersMax, winnersMax * volPerSec);
		StartCoroutine(winnersCoroutine);
	}

	IEnumerator winnersMusicFade(AudioSource source, float maxVolume, float volPerSec)
	{
		if (!source.isPlaying)
			source.Play();
		while ((volPerSec >= 0 && source.volume < maxVolume)
			|| (volPerSec < 0 && source.volume > 0))
		{
			source.volume += volPerSec * Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame();
		}

		if (source.volume <= 0)
		{
			source.Stop();
		}

	}

	public void ranchMusicFade(float volPerSec)
	{
		if (ranchCoroutine != null)
			StopCoroutine(ranchCoroutine);
		ranchCoroutine = ranchMusicFade(ranchMusic, ranchMax, ranchMax * volPerSec);
		StartCoroutine(ranchCoroutine);
	}

	IEnumerator ranchMusicFade(AudioSource source, float maxVolume, float volPerSec)
	{
		if (!source.isPlaying)
			source.Play();
		while ((volPerSec >= 0 && source.volume < maxVolume)
			|| (volPerSec < 0 && source.volume > 0))
		{
			source.volume += volPerSec * Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame();
		}

		if (source.volume <= 0)
		{
			source.Stop();
		}

	}


	/*
	public void caveMusicFade(float volPerSec)
	{
		if(caveCoroutine != null)
			StopCoroutine(caveCoroutine);
		caveCoroutine = caveMusicFade(caveMusic, caveMax, caveMax * volPerSec);
		StartCoroutine(caveCoroutine);
	}

	IEnumerator caveMusicFade(AudioSource source, float maxVolume, float volPerSec)
	{
		if (!source.isPlaying)
			source.Play();
		while ((volPerSec >= 0 && source.volume < maxVolume)
			|| (volPerSec < 0 && source.volume > 0))
		{
			source.volume += volPerSec * Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame();
		}
		
		if (source.volume <= 0)
		{
			source.Stop();
		}
		
	}
	*/

}
