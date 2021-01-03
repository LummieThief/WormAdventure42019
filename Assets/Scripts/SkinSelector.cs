using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelector : MonoBehaviour
{
	public int selectedSkin;
	public GameObject[] skinPrefabs;
	public int[] unlockOrder;
	private GameObject model;


	// Start is called before the first frame update
	void Awake()
	{
		Destroy(transform.GetChild(0).gameObject);
		if (selectedSkin >= skinPrefabs.Length)
		{
			selectedSkin = 0;
		}
		model = Instantiate(skinPrefabs[selectedSkin], transform);
	}

	public void updateModel(GameObject newModel, int skinIndex)
	{
		Destroy(model);
		model = newModel;
		selectedSkin = skinIndex;
	}

	
}
