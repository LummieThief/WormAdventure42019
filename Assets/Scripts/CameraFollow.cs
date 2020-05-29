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

	private float timer;
	public float timeBetweenResets = 5f;
	public Recenter recenter;

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
		initialY = transform.position.y - target.position.y + baseOffsetUp;
		initialZ = transform.position.z - target.position.z + baseOffsetForward;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;


	}
	

	// Update is called once per frame
	void LateUpdate()
    {
		cam.localPosition = new Vector3(0, 0, -Mathf.Clamp(prevDistance + zoomSpeed, 0, camDistance));
		mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		//mouseX = Mathf.Clamp(mouseX, -160, 160);
		mouseY = Mathf.Clamp(mouseY, -60, 60);

		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
		transform.Rotate(Vector3.up, mouseX);
		transform.eulerAngles = new Vector3(mouseY, transform.eulerAngles.y, transform.eulerAngles.z);
		//transform.position = Vector3.Lerp(transform.position, new Vector3(initialX + target.position.x, initialY + target.position.y, initialZ + target.position.z), 0.1f);
		transform.position = new Vector3(initialX + target.position.x, initialY + target.position.y, initialZ + target.position.z);


		bool collided = checkForCollision(transform.position, cam.position);
	    cam.localPosition += Vector3.up * secondaryOffsetUp;

		prevDistance = Vector3.Distance(cam.position, transform.position);

		
		if (collided)
		{
			prevDistance += zoomSpeed;
		}

		/*
		timer += Time.deltaTime;
		if (timer > timeBetweenResets)
		{
			recenter.recenter();
			timer = 0;
		}
		*/
	}

	bool checkForCollision(Vector3 origin, Vector3 target)
	{
		return checkForCollision(origin, target, origin);
	}
	private bool checkForCollision(Vector3 origin, Vector3 target, Vector3 initialPosition)
	{
		RaycastHit hit;
		if (Physics.Linecast(origin, target, out hit))
		{
			if (hit.collider.gameObject.tag == "Solid")
			{
				cam.position = hit.point;
				return true;
			}
			else
			{
				return checkForCollision(Vector3.MoveTowards(hit.point, target, 0.01f), target, initialPosition);
			}
		}
		return false;
	}

	public void sensitivityChange(float newSens)
	{
		mouseSensitivity = newSens * 30;
	}

	public void setY(float val)
	{
		mouseY = val;
	}
}
