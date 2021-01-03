using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailLightBank : MonoBehaviour
{
	[SerializeField]
	private Material freshOn;
	[SerializeField]
	private Material freshOff;
	[SerializeField]
	private Material exertedOn;
	[SerializeField]
	private Material exertedOff;
	[SerializeField]
	private Material tailMat;

	private void Start()
	{
		tailMat = new Material(freshOff.shader);
		tailMat.name = "Independent Tail Material";
		GetComponent<MeshRenderer>().sharedMaterial = tailMat;
	}

	private void OnDestroy()
	{
		Destroy(tailMat);
	}

	public Material getFreshOn()
	{
		return freshOn;
	}
	public Material getFreshOff()
	{
		return freshOff;
	}
	public Material getExertedOn()
	{
		return exertedOn;
	}
	public Material getExertedOff()
	{
		return exertedOff;
	}
}
