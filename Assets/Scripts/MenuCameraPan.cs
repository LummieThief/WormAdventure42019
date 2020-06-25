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
		follow.freezeMouseControl(true);
    }

    // Update is called once per frame
    void Update()
    {
		var degrees = rotSpeed;
		//degrees *= (11 - Mathf.Abs(follow.getDistance()));
		Debug.Log(follow.getDistance());
		transform.Rotate(Vector3.up, degrees * Time.deltaTime);
		if (!game.getStartingGrapple())
		{
			this.enabled = false;
			follow.freezeMouseControl(false);
		}
    }
}
