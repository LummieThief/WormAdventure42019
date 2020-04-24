using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightMatterMaterialBank : MonoBehaviour
{
	public Material[] lightMatterMats;

	public Material[] getLightMatterMats()
	{
		return lightMatterMats;
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
		}
	}
}
