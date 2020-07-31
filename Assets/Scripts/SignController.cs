using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignController : MonoBehaviour
{
	public GameObject child;

    // Update is called once per frame
    void Update()
    {
		child.SetActive(OptionsMenu.signsEnabled);
	}
}
