using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{
	public GameObject playerUnitPrefab;

	// since the player object is invisible and not part of the world,
	// give me something physical to move around

    // Start is called before the first frame update
    void Start()
	{
		//Is this actually my own local player object?
		if (!isLocalPlayer)
		{
			return;
		}

		//Ok, now that the other guys are gone, we can execute some code for this clients object.
		Debug.Log("spawning my own personal unit");
		CmdSpawnPlayerUnit();



		//GameObject go = Instantiate(playerUnitPrefab);
		//NetworkServer.Spawn(go);
    }

    // Update is called once per frame
    void Update()
    {
		//UPDATE runs on everyones computer whether or not they own the object
		/*if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdMoveUnitUp();
		}*/
    }

	////////////////////////////////// COMANDS
	// commands are special functions that only get executed on the server

	GameObject myPlayerUnit;

	[Command]
	void CmdSpawnPlayerUnit()
	{
		GameObject go = Instantiate(playerUnitPrefab);
		myPlayerUnit = go;
		NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
	}

	/*
	[Command]
	void CmdMoveUnitUp()
	{
		if (myPlayerUnit == null)
		{
			return;
		}

		myPlayerUnit.transform.Translate(Vector3.up);
	}
	*/

}
