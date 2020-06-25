using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterInitializer : MonoBehaviour
{
	private float timer;
	public Camera backupCam;
	public DetectWin winner;
    // Start is called before the first frame update
    void Start()
    {
		winner.toggleWin();
	}

    // Update is called once per frame
    void Update()
    {
		
		timer += Time.deltaTime;
		if (timer > 50)
		{
			backupCam.enabled = false;
			Destroy(gameObject);
			
		}
		if (timer > 2 && timer < 10)
		{
			winner.toggleWin();
			timer = 49.8f;
		}

    }
}
