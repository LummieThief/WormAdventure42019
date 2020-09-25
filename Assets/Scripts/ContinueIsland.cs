using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueIsland : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		if (PlayerPrefs.HasKey("Continue") && PlayerPrefs.GetInt("Continue") == 1)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
		}
    }
}
