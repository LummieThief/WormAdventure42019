using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Camera controls with the mouse
public class CameraFollow : MonoBehaviour
{
	public float mouseSensitivity = 100f;
	public float camDistance = 10f;
	public float zoomSpeed = 0.5f;
	public float baseOffsetUp = 0.84f;
	public float baseOffsetForward = -0.84f;
	public float secondaryOffsetUp = 0.7f;
	private float initialX;
	private float initialY;
	private float initialZ;
	public Transform target;
	private float mouseX;
	private float mouseY;
	private Transform cam;
	private float prevDistance;

	private bool mouseFrozen;

	private float timer;
	public float timeBetweenResets = 5f;
	public Recenter recenter;

	private bool obscured;
	private bool twod;

	private float lastTimeScale;

	private Vector3 lastCamPosition;
	private int state = 0; //0 = standard, 1 = static.

	//private Rigidbody rb;
	// Start is called before the first frame update
	void Awake()
    {
		//rb = GetComponent<Rigidbody>()
		cam = GetComponentInChildren<Camera>().transform;
		cam.localPosition = new Vector3(0, 0, -camDistance);
		prevDistance = camDistance;
		//maintains the same distance from the target
		initialX = transform.position.x - target.position.x;
		initialY = transform.position.y - target.position.y;
		initialZ = transform.position.z - target.position.z + baseOffsetForward;

		lastCamPosition = cam.position;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;


	}
	

	// Update is called once per frame
	void LateUpdate()
    {
		//Debug.Log(mouseFrozen);
		/*
		if (PauseMenu.isPaused)
		{
			return;
		}
		*/

		if (state == 0) //standard camera.
		{

			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

			cam.localPosition = new Vector3(0, 0, -Mathf.Clamp(prevDistance + zoomSpeed * Time.deltaTime, 0, camDistance));
			if (!mouseFrozen)
			{
				mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.timeScale;
				mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.timeScale;
			}

			//mouseX = Mathf.Clamp(mouseX, -160, 160);
			mouseY = Mathf.Clamp(mouseY, -75, 75);

			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
			transform.Rotate(Vector3.up, mouseX);
			transform.eulerAngles = new Vector3(mouseY, transform.eulerAngles.y, transform.eulerAngles.z);
			//transform.position = Vector3.Lerp(transform.position, new Vector3(initialX + target.position.x, initialY + target.position.y, initialZ + target.position.z), 0.1f);
			transform.position = new Vector3(initialX + target.position.x, initialY + target.position.y, initialZ + target.position.z);

			RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, baseOffsetUp);

			bool didHit = false;
			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].collider.gameObject.tag == "Solid")
				{
					transform.Translate(Vector3.up * (Vector3.Distance(transform.position, hits[i].point) - 0.1f));
					didHit = true;
					break;
				}
			}
			if (!didHit)
			{
				transform.Translate(Vector3.up * baseOffsetUp);
			}

			bool collided = checkForCollision(transform.position, cam.position);

			float eulerX = transform.eulerAngles.x;
			if (eulerX > 180)
			{
				eulerX -= 360;
			}

			if (collided)
			{
				cam.position = Vector3.MoveTowards(cam.position, target.position, 0.2f);
			}

			prevDistance = -cam.localPosition.z;


			if (collided)
			{
				prevDistance += zoomSpeed * Time.deltaTime;
				obscured = true;
			}
			else
			{
				obscured = false;
			}

			lastCamPosition = cam.position;
		}
		else if (state == 1) //static camera follow.
		{
			transform.position = new Vector3(initialX + target.position.x, initialY + target.position.y, initialZ + target.position.z);
			transform.Translate(Vector3.up * baseOffsetUp);

			cam.position = lastCamPosition;
			cam.transform.LookAt(transform);
		}
		else if (state == 2) //frozen camera
		{
			cam.position = lastCamPosition;
		}

	}

	bool checkForCollision(Vector3 origin, Vector3 target)
	{
		return checkForCollision(origin, target, origin);
	}
	private bool checkForCollision(Vector3 origin, Vector3 target, Vector3 initialPosition)
	{
		RaycastHit hit;
		LayerMask mask = 1 << 8;
		if (Physics.Linecast(origin, target, out hit, mask))
		{
			if (hit.collider.gameObject.tag == "Solid")
			{
				cam.position = hit.point;
				
				return true;
			}
			else
			{
				//Debug.DrawLine(origin, target);
				return checkForCollision(Vector3.MoveTowards(hit.point, target, 0.01f), target, initialPosition);
			}
		}
		//Debug.DrawLine(origin, target);
		return false;
	}

	public void setSensitivity(float sens)
	{
		mouseSensitivity = sens;
	}

	public void setY(float val)
	{
		mouseY = val;
	}

	public void freezeMouseControl(bool value)
	{
		mouseFrozen = value;
	}

	public float getDistance()
	{
		return cam.localPosition.z;
	}

	public void setState(int value)
	{
		state = value;
	}
	public int getState()
	{
		return state;
	}

	public bool getMouseFrozen()
	{
		return mouseFrozen;
	}
}
