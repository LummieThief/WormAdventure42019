using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogBox : MonoBehaviour
{
	public float intensity = 0.013f;
	public Color color;
	public float transitionSpeed = 2;
    // Start is called before the first frame update

	private void OnTriggerStay(Collider other)
	{
		
		if (other.gameObject.tag == "Player")
		{
			RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, color, transitionSpeed * Time.deltaTime);
			RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, intensity, transitionSpeed * Time.deltaTime);
		}
	}
	// Update is called once per frame
}
