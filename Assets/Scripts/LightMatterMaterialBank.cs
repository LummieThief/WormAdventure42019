using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightMatterMaterialBank : MonoBehaviour
{
	public Material[] lightMatterMats;
	public Material[] singleLightMats;

	public Material[] getLightMatterMats()
	{
		return lightMatterMats;
	}
	public Material[] getSingleLightMats()
	{
		return singleLightMats;
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
			foreach (Material m in lightMatterMats)
			{
				Shader shader = m.shader;
				m.SetVector(shader.GetPropertyNameId(0), new Vector4(0, 0, 0, 9999));
			}
			foreach (Material m in singleLightMats)
			{
				Shader shader = m.shader;
				m.SetVector(shader.GetPropertyNameId(0), new Vector4(0, 0, 0, 9999));
			}
		}
	}
}
