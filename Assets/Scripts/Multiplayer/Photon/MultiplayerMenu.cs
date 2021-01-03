using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class MultiplayerMenu : MonoBehaviourPunCallbacks
{
	public GameObject[] panels;
	public Text roomCode;
	public InputField joinInputField;
	private int roomSize = 12;
	private bool returnToJoin = false;
	public Text errorLog;
	private float errorTimeout = 3;
	private float errorTimeoutTimer = 0; //this is the one that changes
	private int region;

	private void Update()
	{
		if (PhotonNetwork.InRoom)
		{
			roomCode.text = PhotonNetwork.CurrentRoom.Name;
		}
		else
		{
			roomCode.text = "";
		}

		if (errorTimeoutTimer < errorTimeout)
		{
			errorTimeoutTimer += Time.unscaledDeltaTime;
			if (errorTimeoutTimer >= errorTimeout)
			{
				errorLog.text = "";
				Debug.Log("Error cleared");
			}
		}
	}
	public void JoinButton()
	{
		string room = "";
		if (joinInputField.IsActive())
		{
			if (joinInputField.text == "")
			{
				room = "0O0O";
			}
			else
			{
				room = joinInputField.text;
			}
		}
		
		Debug.Log("Trying to join room");


		if (!PhotonNetwork.IsConnected) //If a connection to the server hasnt been set up, do that then come back.
		{
			Debug.Log("Network wasnt connected. Connecting and trying again");
			
			if (PlayerPrefs.HasKey("Region"))
			{
				region = PlayerPrefs.GetInt("Region");
				Debug.Log("region token is " + PlayerPrefs.GetString("Region Token")) ;
				PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = PlayerPrefs.GetString("Region Token");
			}
			
			PhotonNetwork.ConnectUsingSettings();
			returnToJoin = true;
			return;
		}
		else //We are connected to the server
		{
			if (PhotonNetwork.InRoom)
			{
				Debug.Log("You were already in a room.");
				logError("Already connected to a server");
				return;
			}


			if (room == "")
			{
				Debug.Log("Input field was blank. Joining a random room");
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				Debug.Log("Joining room " + room);
				PhotonNetwork.JoinRoom(room);
			}
		}
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server");
		if (returnToJoin)
		{
			returnToJoin = false;
			JoinButton();
		}
		
	}

	public void ForceDisconnect()
	{
		PhotonNetwork.Disconnect();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("disconnected");
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log("That room doesnt exist");

		logError(message);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("failed to join a room");
		Debug.Log(returnCode);
		if (returnCode == 32760)
		{
			logError(message + ". Creating new room");
		}
		else
		{
			logError(message);
		}
		CreateRoom(true);
	}

	public override void OnLeftRoom()
	{
		Debug.Log("Left the room");
		
	}

	void CreateRoom(bool visible)
	{
		if (PhotonNetwork.InRoom)
		{
			logError("Already connected to a server");
			return;
		}

		Debug.Log("Creating a new room now");
		string randomRoomName = RandomString(4);
		RoomOptions roomOps = new RoomOptions { IsVisible = visible, IsOpen = true, MaxPlayers = (byte)roomSize };
		PhotonNetwork.CreateRoom("" + randomRoomName, roomOps);
		Debug.Log(randomRoomName);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("Failed to create a room... trying again");
		CreateRoom(true); //retrying with a different name
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("You are now in a room");
		GameObject photonPlayer = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
		SkinSelector ss = FindObjectOfType<SkinSelector>();
		photonPlayer.GetComponent<PhotonPlayerController>().setSkin(ss.selectedSkin);
	}
	

	public void CreatePublic()
	{
		CreateRoom(true);
	}

	public void CreatePrivate()
	{
		CreateRoom(false);
	}

	public void LeaveRoom()
	{
		if (PhotonNetwork.InRoom)
		{
			PhotonNetwork.LeaveRoom();
		}
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
	}


	public static string RandomString(int length)
	{
		const string chars = "ACDEFGHIJKLMNPQRTUVWXY134679134679";
		string choice = "";
		for (int i = 0; i < length; i++)
		{
			int index = Random.Range(0, chars.Length);
			choice += chars.Substring(index, 1);
		}
		return choice;
		
	}


	private void logError(string error)
	{
		errorLog.text = error;
		errorTimeoutTimer = 0;
	}

	public void openPanel(int index)
	{
		for (int i = 0; i < panels.Length; i++)
		{
			if (i == index)
			{
				panels[i].SetActive(true);
			}
			else
			{
				panels[i].SetActive(false);
			}
		}
	}
}
