using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyWorm : MonoBehaviour
{
	public GameObject baseSkin;
	public GameObject rainbowSkin;
	public GameObject blackSkin;
    // Start is called before the first frame update
    void Awake()
    {
		/*
		Game game = GameObject.FindObjectOfType<Game>();
		if (game == null)
		{
			GameObject.Instantiate(baseSkin, transform.position, transform.rotation);
		}
		else
		{
			switch (game.getSkin())
			{
				case "base":
					GameObject.Instantiate(baseSkin, transform.position, transform.rotation);
					break;
				case "black":
					GameObject.Instantiate(blackSkin, transform.position, transform.rotation);
					break;
				case "rainbow":
					GameObject.Instantiate(rainbowSkin, transform.position, transform.rotation);
					break;
			}
		}
		GameObject.Destroy(this.gameObject);
		*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
