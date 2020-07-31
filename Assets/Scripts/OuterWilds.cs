using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Any objects with this script will rotate around the worm and be effected by Recenter
public class OuterWilds : MonoBehaviour
{
	private float rotSpeed = 150f;
	private float airRotSpeed = 40f;
	private Transform worm;
	private JumpTrigger jumpTrigger;
	private Game game;
	private CameraFollow cameraFollow;
	private bool grounded;

	// Start is called before the first frame update

    void Start()
    {
		game = FindObjectOfType<Game>();
		worm = GameObject.FindGameObjectWithTag("Player").transform;
		jumpTrigger = worm.gameObject.GetComponentInChildren<JumpTrigger>();
		cameraFollow = FindObjectOfType<CameraFollow>();

		if (gameObject.tag == "OuterWildsWorld")
		{
			StartCoroutine(resetSkybox());
		}
	}
	//*
    // Update is called once per frame
    void FixedUpdate()
    {
		if (worm.GetComponent<WormMove>().twod || (game != null && game.getStartingGrapple()) || PauseMenu.isPaused
			|| cameraFollow.getState() != 0 || cameraFollow.getMouseFrozen())
		{
			return;
		}

		
		if (Input.GetAxis("Horizontal") != 0)
		{
			grounded = jumpTrigger.getGrounded();
			Quaternion prevRot = transform.rotation;

			var rot = Input.GetAxis("Horizontal") * Time.deltaTime;
			if (grounded)
			{
				rot *= rotSpeed;
				rotate(-rot);
			}
			else
			{
				
				rot *= airRotSpeed;
				rotate(-rot);
			}

		}
	}
	private void LateUpdate()
	{
		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
			jumpTrigger = worm.gameObject.GetComponentInChildren<JumpTrigger>();
		}
	}

	public void rotate(float degrees)
	{
		if (worm == null)
		{
			worm = GameObject.FindGameObjectWithTag("Player").transform;
		}
		transform.RotateAround(worm.position, Vector3.up, degrees);
		if (gameObject.tag == "OuterWildsWorld")
		{
			RenderSettings.skybox.SetFloat("_Rotation", -transform.eulerAngles.y);
		}
	}
	//*/

	private IEnumerator resetSkybox()
	{
		for (int i = 0; i < 5; i++)
		{
			RenderSettings.skybox.SetFloat("_Rotation", -transform.eulerAngles.y);
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForEndOfFrame();
	}

}
