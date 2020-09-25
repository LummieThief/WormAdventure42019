using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreecamController : MonoBehaviour
{
	private bool woke = false;
	private CanvasGroup ui;
	private Camera cam;
	private bool active = false;
	public float speed = 10f;
	public float acceleration = 2f;
	private float currentSpeed;
	public float mouseSensitivity = 10f;
	private float mouseX;
	private float mouseY;
	public Transform origin;

	public GameObject point;
	private Transform[] points;
	public float trollyTime = 1f; //how long (in seconds) to travel the trolly
	private float trollyTimer;
	public float scrollSensitivity = 0.1f;
	private bool running = false;



	//TFGH move
	//R to reset trolly points
	//Y to set a new trolly point
	//E to run the trolly
	//MouseWheel to change the trolly speed


    // Start is called before the first frame update
    void Awake()
    {
		ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasGroup>();
		cam = GetComponent<Camera>();
		cam.enabled = false;
		points = new Transform[2];
		woke = true;
    }

	private void LateUpdate()
	{
		if (woke)
		{
			activate();
			woke = false;
		}
	}

	// Update is called once per frame
	void Update()
    {
		
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (active)
			{
				deactivate();
			}
			else
			{
				activate();
			}
			Debug.Log("freecam is " + active);
		}

		if (active)
		{
			if (!running)
			{
				doCameraMovement();
			}
			doTrolly();


			doSlowMo();
		}
	}

	private void doCameraMovement()
	{
		if (Input.GetKeyDown(KeyCode.F2))
		{
			if (ui.interactable)
			{
				ui.alpha = 0;
				ui.interactable = false;
			}
			else
			{
				ui.alpha = 1;
				ui.interactable = true;
			}
		}

		if (Input.GetKey(KeyCode.T))
		{
			transform.Translate((Vector3.forward) * currentSpeed * Time.unscaledDeltaTime);
		}
		if (Input.GetKey(KeyCode.G))
		{
			transform.Translate((Vector3.back) * currentSpeed * Time.unscaledDeltaTime);
		}
		if (Input.GetKey(KeyCode.F))
		{
			transform.Translate((Vector3.left) * currentSpeed * Time.unscaledDeltaTime);
		}
		if (Input.GetKey(KeyCode.H))
		{
			transform.Translate((Vector3.right) * currentSpeed * Time.unscaledDeltaTime);
		}

		if (!Input.GetKey(KeyCode.T) && !Input.GetKey(KeyCode.G) && !Input.GetKey(KeyCode.F) && !Input.GetKey(KeyCode.H))
		{
			currentSpeed = speed;
		}
		else
		{
			currentSpeed += acceleration * Time.unscaledDeltaTime;
		}


		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);

		mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
		mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		//mouseX = Mathf.Clamp(mouseX, -160, 160);
		mouseY = Mathf.Clamp(mouseY, -90, 90);

		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
		transform.Rotate(Vector3.up, mouseX);
		transform.eulerAngles = new Vector3(mouseY, transform.eulerAngles.y, transform.eulerAngles.z);
	}

	private void doSlowMo()
	{
		float speed = Time.timeScale;

		if (Input.GetKeyDown(KeyCode.Alpha0)) 
			speed = 0;
		else if (Input.GetKeyDown(KeyCode.Alpha1)) 
			speed = 0.1f;
		else if (Input.GetKeyDown(KeyCode.Alpha2)) 
			speed = 0.2f;
		else if (Input.GetKeyDown(KeyCode.Alpha3)) 
			speed = 0.3f;
		else if (Input.GetKeyDown(KeyCode.Alpha4)) 
			speed = 0.4f;
		else if (Input.GetKeyDown(KeyCode.Alpha5)) 
			speed = 0.5f;
		else if (Input.GetKeyDown(KeyCode.Alpha6)) 
			speed = 0.6f;
		else if (Input.GetKeyDown(KeyCode.Alpha7)) 
			speed = 0.7f;
		else if (Input.GetKeyDown(KeyCode.Alpha8)) 
			speed = 0.8f;
		else if (Input.GetKeyDown(KeyCode.Alpha9)) 
			speed = 0.9f;
		else if (Input.GetKeyDown(KeyCode.Backspace)) 
			speed = 1f;

		if (Time.timeScale != speed)
		{
			Debug.Log(speed);
		}
		Time.timeScale = speed;

	}

	private void doTrolly()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			running = false;
			points = new Transform[2];
		}
		else if (Input.GetKeyDown(KeyCode.Y))
		{
			running = false;
			Transform newPoint = Instantiate(point, GameObject.FindGameObjectWithTag("OuterWildsWorld").transform).transform;
			newPoint.position = transform.position;
			newPoint.rotation = transform.rotation;
			if (points[0] == null)
			{
				points[0] = newPoint;
			}
			else
			{
				points[1] = newPoint;
			}
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (running)
			{
				running = false;
			}
			else if (points[1] != null)
			{

				running = true;
				trollyTimer = 0;
				transform.position = points[0].position;
				transform.rotation = points[0].rotation;
			}
		}

		if (running)
		{
			trollyTimer += Time.unscaledDeltaTime;
			if (trollyTimer > trollyTime)
			{
				running = false;
				//mouseY = points[1].rotation.x;
			}


			float distance = Vector3.Distance(points[0].position, points[1].position);
			float angle = Quaternion.Angle(points[0].rotation, points[1].rotation);
			transform.position = Vector3.MoveTowards(points[0].position, points[1].position, distance * (trollyTimer / trollyTime));
			transform.rotation = Quaternion.RotateTowards(points[0].rotation, points[1].rotation, angle * (trollyTimer / trollyTime));
		}
		else
		{
			if (Input.mouseScrollDelta.y != 0)
			{
				trollyTime += Input.mouseScrollDelta.y * scrollSensitivity;
				trollyTime = Mathf.Clamp(trollyTime, 0, 9999);
				Debug.Log(trollyTime);
			}
		}

	}

	private void activate()
	{
		active = true;
		cam.enabled = true;
		transform.position = origin.position;
		transform.rotation = origin.rotation;
		mouseY = FindObjectOfType<CameraFollow>().getY();
	}

	private void deactivate()
	{
		active = false;
		cam.enabled = false;
	}
}
