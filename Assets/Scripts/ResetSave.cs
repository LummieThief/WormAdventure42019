using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResetSave : MonoBehaviour
{
	public bool reset;
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
		if (hardResetBeCareful)
		{
			fullResetSave();
			hardResetBeCareful = false;
		}
    }

	public static void resetSave()
	{
		/*
		PlayerPrefs.DeleteKey("PositionX");
		PlayerPrefs.DeleteKey("PositionY");
		PlayerPrefs.DeleteKey("PositionZ");

		PlayerPrefs.DeleteKey("RotationX");
		PlayerPrefs.DeleteKey("RotationY");
		PlayerPrefs.DeleteKey("RotationZ");
		PlayerPrefs.DeleteKey("RotationW");

		PlayerPrefs.DeleteKey("VelocityX");
		PlayerPrefs.DeleteKey("VelocityY");
		PlayerPrefs.DeleteKey("VelocityZ");


		PlayerPrefs.DeleteKey("WorldPositionX");
		PlayerPrefs.DeleteKey("WorldPositionY");
		PlayerPrefs.DeleteKey("WorldPositionZ");

		PlayerPrefs.DeleteKey("WorldRotationX");
		PlayerPrefs.DeleteKey("WorldRotationY");
		PlayerPrefs.DeleteKey("WorldRotationZ");
		PlayerPrefs.DeleteKey("WorldRotationW");

		PlayerPrefs.DeleteKey("FogR");
		PlayerPrefs.DeleteKey("FogG");
		PlayerPrefs.DeleteKey("FogB");
		PlayerPrefs.DeleteKey("FogA");
		PlayerPrefs.DeleteKey("FogD");

		PlayerPrefs.DeleteKey("Scene");
		*/
		System.IO.File.Delete(SaveLoad.path);

		Debug.Log("Reset");
	}

	public static void fullResetSave()
	{
		Debug.Log("Hard Reset");
		PlayerPrefs.DeleteAll();
	}
}
