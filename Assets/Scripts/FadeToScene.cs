using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToScene : MonoBehaviour
{
	public Image fadeUI;
	public Animator fadeAnim;
	private bool fading = false;
	private string scene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (fading)
		{
			if (fadeUI.color.a == 1)
			{
				SceneManager.LoadScene(scene);
			}
		}
    }

	public void toScene(string sce)
	{
		//fadeAnim.SetBool("Fade", true);
		//fading = true;
		scene = sce;
		//scene = sce
		SceneManager.LoadScene(scene);
	}

}
