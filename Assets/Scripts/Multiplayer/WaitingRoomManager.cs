using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : Photon.PunBehaviour {
	public GameObject waitingWindow;
	public Text lobbyTitle;
	public GameObject player;
	public Text player1Name, player2Name, player3Name, player4Name;
	public Text player1Class, player2Class, player3Class, player4Class;
	public Text startReady;

	[HideInInspector]
	public bool playerIsReady = false;

	private PhotonPlayer[] playerList;
	private int playerIndex;

	public void DisplayWindow() {
		waitingWindow.SetActive(true);
		lobbyTitle.text = PhotonNetwork.room.Name;
		startReady.text = PhotonNetwork.isMasterClient ? "Start" : "Ready";
	}

	void OnPhotonPlayerConnected(PhotonPlayer player) {
		if (PhotonNetwork.isMasterClient) {
			this.photonView.RPC ("SetPlayerList", PhotonTargets.All, PhotonNetwork.playerList);
		}
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player) {
		if (PhotonNetwork.isMasterClient) {
			this.photonView.RPC ("SetPlayerList", PhotonTargets.All, PhotonNetwork.playerList);
		}
	}

	public void UpdateWindow() {
		string playerName = PhotonNetwork.player.NickName;
		string playerClass = player.GetComponent<playerInfo>().GetClassName();
		string methodName = "SetPlayer1Info";
		switch (playerIndex) {
		case 1:
			methodName = "SetPlayer2Info";
			break;
		case 2:
			methodName = "SetPlayer3Info";
			break;
		case 3:
			methodName = "SetPlayer4Info";
			break;
		}

		string nameToDisplay = playerIsReady ? playerName + " (Ready)" : playerName;
		this.photonView.RPC (methodName, PhotonTargets.All, nameToDisplay, playerClass);


	}
			
	[PunRPC]
	public void SetPlayerList(PhotonPlayer[] pl) {
		playerList = pl;
		for (int i = 0; i < playerList.Length; i++) {
			if (playerList [i].NickName.Equals (PhotonNetwork.player.NickName)) {
				playerIndex = i; 
				SetSpawnIndex (playerIndex);
				break;
			}
		}
		UpdateWindow ();
	}

	[PunRPC]
	private void SetPlayer1Info(string playerName, string playerClass) {
		player1Name.text = playerName + " (Host)";
		player1Class.text = playerClass;
	}

	[PunRPC]
	private void SetPlayer2Info(string playerName, string playerClass) {
		player2Name.text = playerName;
		player2Class.text = playerClass;
	}

	[PunRPC]
	private void SetPlayer3Info(string playerName, string playerClass) {
		player3Name.text = playerName;
		player3Class.text = playerClass;
	}

	[PunRPC]
	private void SetPlayer4Info(string playerName, string playerClass) {
		player4Name.text = playerName;
		player4Class.text = playerClass;
	}

	private void SetSpawnIndex(int i) {
		// Sets the playerprefs for the spawn point that the player will use
		// If this is used for local testing, every player will spawn in the same spot
		PlayerPrefs.SetInt ("pin", i);

	}
}
