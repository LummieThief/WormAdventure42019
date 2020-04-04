using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	private string ryan = "";
	private float typingTimer = 0f;
	private float timeLimit = 2f;
	//public GameObject currentWorm;
	public string skin = "base";
    // Start is called before the first frame update
    void Start()
    {
		GameObject.DontDestroyOnLoad(this);
		UnityEngine.SceneManagement.SceneManager.LoadScene("Attempt 2");
	}

	private void Update()
	{
		if (typingTimer > 0)
		{
			typingTimer += Time.deltaTime;
		}
		if (typingTimer >= timeLimit)
		{
			typingTimer = 0;
			ryan = "";
		}
		if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F4))
		{
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			typingTimer += 0.01f;
			if (ryan.Equals(""))
			{
				ryan = "R";
			}
			else
			{
				ryan = "";
			}
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			if (ryan.Equals("R"))
			{
				ryan += "Y";
			}
			else
			{
				ryan = "";
			}
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (ryan.Equals("RY"))
			{
				ryan += "A";
			}
			else
			{
				ryan = "";
			}
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			if (ryan.Equals("RYA"))
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					skin = "rainbow";
				}
				else if (!skin.Equals("black"))
				{
					skin = "black";
				}
				else if(!skin.Equals("base"))
				{
					skin = "base";
				}
				UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
			}
			else
			{
				ryan = "";
			}
		}
	}

	public string getSkin()
	{
		return skin;
	}
}
