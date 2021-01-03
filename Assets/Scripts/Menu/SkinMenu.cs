using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinMenu : MonoBehaviour
{
	public GameObject startMenuUI;
	public GameObject skinMenuUI;

	private WormMove worm;
	private SkinSelector ss;
	private int skinNumber;
    // Start is called before the first frame update
    void Start()
    {
		worm = FindObjectOfType<WormMove>();
		ss = FindObjectOfType<SkinSelector>();
		skinNumber = ss.selectedSkin;

	}

	public void ChangeSkinLeft()
	{
		ChangeSkin(false);
	}

	public void ChangeSkinRight()
	{
		ChangeSkin(true);
	}

	private void ChangeSkin(bool right)
	{
		if (worm == null)
		{
			worm = FindObjectOfType<WormMove>();
		}
		if (ss == null)
		{
			ss = FindObjectOfType<SkinSelector>();
			skinNumber = ss.selectedSkin;
		}
		skinNumber = ss.selectedSkin;

		string unlockedSkins = "00000000000000000000000000000001";
		if (PlayerPrefs.HasKey("skin"))
		{
			string binary = System.Convert.ToString((int)Mathf.Pow(2, 31) + PlayerPrefs.GetInt("skin"), 2);
			unlockedSkins = binary;
		}
		unlockedSkins = reverseString(unlockedSkins);
		//Debug.Log(unlockedSkins);
		//unlocked skins is now a binary string with a 1 in the 32nd slot at the very end.


		int num;
		if (right)
		{
			num = skinNumber + 1;
			if (num >= ss.skinPrefabs.Length)
			{
				num = 0;
			}

			while (!(ss.unlockOrder[num] < 0 || unlockedSkins[ss.unlockOrder[num]].Equals('1')))
			{
				num++;
				if (num >= ss.skinPrefabs.Length)
				{
					num = 0;
				}
			}			
		}
		else
		{
			num = skinNumber - 1;
			if (num < 0)
			{
				num = ss.skinPrefabs.Length - 1;
			}

			while (!(ss.unlockOrder[num] < 0 || unlockedSkins[ss.unlockOrder[num]].Equals('1')))
			{
				num--;
				if (num < 0)
				{
					num = ss.skinPrefabs.Length - 1;
				}
			}

		}
		skinNumber = num;
		
		worm.changeSkin(num);
		
	}

	public void Back()
	{
		startMenuUI.SetActive(true);
		skinMenuUI.SetActive(false);
	}


	private string reverseString(string text)
	{
		char[] cArray = text.ToCharArray();
		string reverse = "";
		for (int i = cArray.Length - 1; i > -1; i--)
		{
			reverse += cArray[i];
		}
		return reverse;
	}
}
