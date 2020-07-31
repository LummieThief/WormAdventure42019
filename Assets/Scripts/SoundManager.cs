using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public AudioSource scrunch;
	public AudioSource descrunch;
	public AudioSource grass;
	public AudioSource[] jumps;
	public AudioSource death;
	public AudioSource fall;
	public AudioSource speedWind;
	public AudioSource grapple;

	private float speedWindMaxVolume;

	private void Start()
	{
		speedWindMaxVolume = speedWind.volume;
		speedWind.volume = 0;
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
	public void playFall()
	{
		fall.Play();
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
}
