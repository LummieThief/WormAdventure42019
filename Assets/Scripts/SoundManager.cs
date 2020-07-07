using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public AudioSource scrunch;
	public AudioSource descrunch;
	public AudioSource grass;
	public AudioSource[] jumps;

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
}
