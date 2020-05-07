using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Every x seconds, all the objects with OuterWilds get moved back to the origin
public class Recenter : MonoBehaviour
{
	public GameObject worm;
	public Transform cam;
	private float timer;
	private float timeBetweenResets = 60f;
	private float xDis, yDis, zDis;
	private int count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime;
		if (timer >= timeBetweenResets)
		{
			timer = 0;
			count++;
			//displacements from the origin
			xDis = worm.transform.position.x;
			yDis = worm.transform.position.y;
			zDis = worm.transform.position.z;
			//sends the worm back to the origin
			worm.transform.position = new Vector3(worm.transform.position.x - xDis, worm.transform.position.y - yDis, worm.transform.position.z - zDis);
			cam.position = new Vector3(cam.position.x - xDis, cam.position.y - yDis, cam.position.z - zDis);
			foreach (OuterWilds n in FindObjectsOfType<OuterWilds>())
			{
				GameObject obj = n.gameObject;
				obj.transform.position = new Vector3(obj.transform.position.x - xDis, obj.transform.position.y - yDis, obj.transform.position.z - zDis);
			}
			Debug.Log(count);
		}
    }
}
