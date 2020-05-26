using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumParent : MonoBehaviour
{
	private bool active;
	private bool visible;
	private bool shouldBeVisible;
	private MeshRenderer rendr;
	public Material invisibleMat;
	private Material initMat;
	private int numRenderers;
	private MeshRenderer[] renderers;
	private Quantum[] quantums;
	//private Material[] mats;

	private bool calledActive;
	// Start is called before the first frame update
	void Start()
	{
		if (GetComponent<MeshRenderer>() != null) //this objects own renderer
		{
			numRenderers++;
		}
		foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) //all renderes in children
		{
			numRenderers++;
		}

		renderers = new MeshRenderer[numRenderers];
		quantums = new Quantum[numRenderers];
		//mats = new Material[numRenderers];

		var tem = 0;
		if (GetComponent<MeshRenderer>() != null)
		{
			renderers[tem] = GetComponent<MeshRenderer>();
			quantums[tem] = gameObject.AddComponent<Quantum>();
			//mats[tem] = renderers[tem].material;
			quantums[tem].invisibleMat = invisibleMat;

			tem++;
		}
		foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) //all renderes in children
		{
			renderers[tem] = r;
			//mats[tem] = r.material;
			quantums[tem] = r.gameObject.AddComponent<Quantum>();
			quantums[tem].invisibleMat = invisibleMat;
			tem++;
		}
	}

	// Update is called once per frame
	void LateUpdate()
	{
		var works = true;
		for (int i = 0; i < numRenderers; i++)
		{
			Debug.Log(quantums[i].getVisible());
			if (quantums[i].getVisible() == false)
			{
				works = false;
			}
		}

		
		for (int i = 0; i < numRenderers; i++)
		{
			quantums[i].setCanRender(works);
		}
		
	}
}
