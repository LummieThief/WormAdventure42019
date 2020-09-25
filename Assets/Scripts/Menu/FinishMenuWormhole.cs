using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenuWormhole : MonoBehaviour
{

	public static bool active;

	public GameObject text1;
	public GameObject text2;
	public GameObject text3;
	public void Menu()
	{
		ResetSave.resetSave();
		GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
		foreach (Persistant p in FindObjectsOfType<Persistant>())
		{
			GameObject g = p.gameObject;
			Destroy(g);
		}
		DetectWin.hasWon = false;
		GameObject.Destroy(FindObjectOfType<DetectWin>());
		PlayerPrefs.SetInt("Continue", 1);
		SceneManager.LoadScene("Attempt 2");

	}


	private void Update()
	{
		if (active)
		{
			StartCoroutine(endingCutscene());
			AchievementManager.Achieve("ACH_FAM");
			active = false;
		}
	}


	private IEnumerator endingCutscene()
	{
		Debug.Log("1");
		yield return new WaitForSeconds(4.707f);
		Debug.Log("2");
		text1.SetActive(true);
		yield return new WaitForSeconds(4.502f);
		Debug.Log("3");
		text2.SetActive(true);
		yield return new WaitForSeconds(2.280f);
		Debug.Log("4");
		text3.SetActive(true);
		yield return new WaitForSeconds(2.268f);
		Menu();
	}
}
