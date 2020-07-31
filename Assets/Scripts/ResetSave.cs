﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResetSave : MonoBehaviour
{
	public bool reset;
	public bool resetSubmit;
	public bool hardResetBeCareful;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		if (reset)
		{
			resetSave();
			reset = false;
		}

		if (resetSubmit)
		{
			resetSubmit = false;
			PlayerPrefs.SetString("Submitted", "False");
		}
		
		if (hardResetBeCareful)
		{
			fullResetSave();
			hardResetBeCareful = false;
		}
    }

	public static void resetSave()
	{
		System.IO.File.Delete(SaveLoad.path);
		PlayerPrefs.SetInt("unity.player_session_log", Random.Range(0, 499999) * 2 + 1);
		PlayerPrefs.SetFloat("PlayTime", 0);

		Debug.Log("Reset");
	}

	public static void fullResetSave()
	{
		Debug.Log("Hard Reset");
		PlayerPrefs.DeleteAll();
	}
}
