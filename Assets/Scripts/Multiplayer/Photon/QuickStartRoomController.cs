using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{

	private int multiplayerSceneIndex = 0;

	public override void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	public override void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Joined room");
		StartGame();
	}

	private void StartGame()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.Log("Starting game");
			//PhotonNetwork.LoadLevel(multiplayerSceneIndex);
		}
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
	}
}
