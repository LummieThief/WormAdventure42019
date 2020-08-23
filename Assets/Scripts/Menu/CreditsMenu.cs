using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{

	public GameObject creditsMenuUI;
	public GameObject startMenuUI;

	public void Back()
	{
		creditsMenuUI.SetActive(false);
		startMenuUI.SetActive(true);
	}
}
