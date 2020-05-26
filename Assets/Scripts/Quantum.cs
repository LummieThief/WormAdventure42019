using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quantum : MonoBehaviour
{
	private bool active;
	private bool visible;
	private bool shouldBeVisible;
	private MeshRenderer rendr;
	public Material invisibleMat;
	private Material initMat;

	private bool calledActive;
	private bool canRender = true;
    // Start is called before the first frame update
    void Start()
    {
		rendr = GetComponent<MeshRenderer>();
		initMat = rendr.material;
		rendr.material = invisibleMat;
		visible = false;

    }

    // Update is called once per frame
    void LateUpdate()
    {
		calledActive = false;
		if (!active)
		{
			if (!shouldBeVisible)
			{
				visible = false;
			}
		}

		if (visible)
		{
			rendr.material = initMat;
		}
		else
		{
			rendr.material = invisibleMat;
		}

		if (!canRender)
		{
			rendr.material = invisibleMat;
		}

	}

	private void OnBecameVisible()
	{
		if (active)
		{
			visible = true;
		}

			shouldBeVisible = true;

	}

	private void OnBecameInvisible()
	{
		if (!active)
		{
			visible = false;
		}

			shouldBeVisible = false;

	}
	

	public void setActive(bool actv)
	{
		if (actv)
		{
			active = true;
			calledActive = true;
		}
		else if (!calledActive)
		{
			active = false;
			
		}
	}

	public bool getVisible()
	{
		return visible;
	}

	public void setVisible(bool b)
	{
		visible = b;
	}

	public void setCanRender(bool t)
	{
		canRender = t;
	}
}
