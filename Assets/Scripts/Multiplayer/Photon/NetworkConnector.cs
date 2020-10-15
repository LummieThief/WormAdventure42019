using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkConnector : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Awake()
    {
		if (!PhotonNetwork.IsConnected)
		{
			DontDestroyOnLoad(this);
			PhotonNetwork.ConnectUsingSettings();
		}
    }

	public override void OnConnectedToMaster()
	{
		Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server");
	}
}
