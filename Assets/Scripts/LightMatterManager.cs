using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMatterManager : MonoBehaviour
{
	private Material[] lightMatterMaterials;
	private Transform[] activeLights;
	public int maxActiveLights;
	public Transform wormLocation;
	private float timer = 2;
    // Start is called before the first frame update
    void Start()
    {
		lightMatterMaterials = gameObject.GetComponent<LightMatterMaterialBank>().getLightMatterMats();
		activeLights = new Transform[maxActiveLights];
    }


    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime;
		if (timer >= 1)
		{
			LightSource[] lightScripts = GameObject.FindObjectsOfType<LightSource>();
			GameObject[] lightSources = new GameObject[lightScripts.Length];
			//Debug.Log(lightScripts.Length);
			for (int i = 0; i < lightScripts.Length; i++)
			{
				lightSources[i] = lightScripts[i].gameObject;
			}
			if (lightSources.Length > 0)
			{
				resetActiveLights(lightSources);
			}
			timer = 0;
		}
		foreach (Material m in lightMatterMaterials)
		{
			Shader shader = m.shader;
			for (int i = 0; i < maxActiveLights; i++)
			{
				if (activeLights[i] != null)
				{
					Vector3 lightPos = activeLights[i].position;
					m.SetVector(shader.GetPropertyNameId(i), new Vector4(lightPos.x, lightPos.y, lightPos.z, activeLights[i].GetComponent<LightSource>().getBrightness()));
				}
				else
				{
					m.SetVector(shader.GetPropertyNameId(i), Vector4.zero);
				}
			}
		}
	}


	private void resetActiveLights(GameObject[] sources)
	{
		
		activeLights = new Transform[maxActiveLights]; //clear the current active lights array
		float[] lightDistances = new float[maxActiveLights]; //creates an array to hold the light distances
		int fillCounter = 0;
		foreach (GameObject g in sources)
		{
			//Debug.Log(g);
			//if (g == null)
			//	break;
			Transform light = g.transform;
			if (fillCounter < maxActiveLights)
			{
				activeLights[fillCounter] = light;
				lightDistances[fillCounter] = Vector3.Distance(light.position, wormLocation.position);
				//Debug.Log(activeLights[fillCounter]);
				fillCounter++;
				
			}
			else
			{
				float thisDistance = Vector3.Distance(light.position, wormLocation.position);

				float max = -1;
				int maxIndex = -1;
				for (int k = 0; k < maxActiveLights; k++)
				{
					if (lightDistances[k] > max)
					{
						max = lightDistances[k];
						maxIndex = k;
					}
				}
				if (thisDistance < lightDistances[maxIndex])
				{
					activeLights[maxIndex] = light;
					lightDistances[maxIndex] = thisDistance;
				}
			}
		}
	}
}
