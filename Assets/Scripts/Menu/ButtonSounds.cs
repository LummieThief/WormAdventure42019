using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
	private SoundManager sm;
    // Start is called before the first frame update
    void Start()
    {
		sm = FindObjectOfType<SoundManager>();
    }

	public void MouseDown()
	{
		if (sm == null)
			sm = FindObjectOfType<SoundManager>();
		sm.playMouseDown();
	}

	public void MouseUp()
	{
		//sm.playMouseUp();
	}

	public void MouseEnter()
	{
		if (sm == null)
			sm = FindObjectOfType<SoundManager>();
		//Debug.Log("mouse entered");
		sm.playMouseEnter();
	}
}
