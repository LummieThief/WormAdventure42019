using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
	public GameObject quickStartButton;
	public GameObject cancelButton;
	[SerializeField]
	private int roomSize = 4;

	public override void OnConnectedToMaster()
	{
		//PhotonNetwork.AutomaticallySyncScene = false;
		quickStartButton.SetActive(true);
	}

	public void QuickStart()
	{
		quickStartButton.SetActive(false);
		cancelButton.SetActive(true);
		PhotonNetwork.JoinRandomRoom();
		Debug.Log("Quick start");
	}


	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("failed to join a room");
		CreateRoom();
	}

	void CreateRoom()
	{
		Debug.Log("Creating a new room now");
		int randomRoomNumber = Random.Range(1000, 10000);
		RoomOptions roomOps = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
		PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
		Debug.Log(randomRoomNumber);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("Failed to create a room... trying again");
		CreateRoom(); //retrying with a different name
	}

	public void Cancel()
	{
		cancelButton.SetActive(false);
		quickStartButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
}
