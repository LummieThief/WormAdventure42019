using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraPan : MonoBehaviour
{
	private float rotSpeed = 10f;
	private CameraFollow follow;
	private Game game;
    // Start is called before the first frame update
    void Start()
    {
		follow = FindObjectOfType<CameraFollow>();
		game = FindObjectOfType<Game>();
		if (game != null)
		{
			follow.freezeMouseControl(true);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (game != null)
		{
			var degrees = rotSpeed;

			transform.Rotate(Vector3.up, degrees * Time.deltaTime);
			if (!game.getStartingGrapple())
			{
				this.enabled = false;
				follow.freezeMouseControl(false);
			}
		}
    }
}
