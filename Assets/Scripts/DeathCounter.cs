using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
	private int deaths = 0;

	public void addDeath()
	{
		deaths++;
		Debug.Log("Died: " +deaths);
		
	}

	public int getDeaths()
	{
		return deaths;
	}

	public void removeDeath()
	{
		deaths--;
		Debug.Log("Removed a death: " + deaths);
	}
}
