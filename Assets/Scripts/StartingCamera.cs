using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingCamera : MonoBehaviour
{
	public StartingCountdown startingCount;
	private Transform cam;
	private CameraFollow follow;
	private float timer = 0;
	private float delay = 2f;
	private float finalDistance = 1f;
	private float camRigStartingRotation = 20f;
	private int state = 0;
    // Start is called before the first frame update
    void Start()
    {
		GameObject cameraRig = GameObject.FindGameObjectWithTag("MainCamera");
		cam = cameraRig.GetComponentInChildren<Camera>().transform;
		follow = cameraRig.GetComponentInChildren<CameraFollow>();
		follow.setY(camRigStartingRotation);
		follow.freezeMouseControl(true);
    }

    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime;
		if (timer > delay)
		{
			if (state == 0)
			{
				transform.position = Vector3.Lerp(transform.position, cam.position, (4 + timer - delay) * Time.deltaTime);
				transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, (4 + timer - delay) * Time.deltaTime);
			}
			if (Vector3.Distance(transform.position, cam.position) < finalDistance)
			{
				startingCount.startCountdown();
			}
		}
		else
		{
			if (state == 0)
			{
				transform.position = Vector3.Lerp(transform.position, cam.position, 0.1f * Time.deltaTime);
				transform.rotation = Quaternion.Lerp(transform.rotation, cam.rotation, 0.1f * Time.deltaTime);
			}
		}

		if (state == 1)
		{
			startingCount.startCountdown();
			transform.position = cam.position;
			transform.rotation = cam.rotation;
		}
    }

	public void startGame()
	{
		follow.freezeMouseControl(false);
		FindObjectOfType<WormMove>().setJumpDown();
		Destroy(gameObject);
	}

	public void setState(int s)
	{
		state = s;
	}
}
