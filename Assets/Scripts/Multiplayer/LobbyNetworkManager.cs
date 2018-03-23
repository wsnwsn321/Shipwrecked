using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetworkManager : Photon.MonoBehaviour {

	public GameObject lobbyNameManager;
	public GameObject lobbyListManager;
	public GameObject waitingRoomManager;

	/// <summary>
	/// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
	/// </summary>   
	[Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
	public byte MaxPlayersPerRoom = 4;

	private string playerNickname;
	private string lobbyName;
	private LobbyListManager llm;
	private WaitingRoomManager wrm;
	private int playerCount;

	static string playerNamePrefKey = "PlayerName";

	/// <summary>
	/// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
	/// </summary>
	string _gameVersion = "1";


	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
	/// </summary>
	void Awake()
	{

		// #Critical
		// we don't join the lobby. There is no need to join a lobby to get the list of rooms.
		PhotonNetwork.autoJoinLobby = true;


		// #Critical
		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;
	}


	/// <summary>
	/// MonoBehaviour method called on GameObject by Unity during initialization phase.
	/// </summary>
	void Start()
	{
		llm = lobbyListManager.GetComponent<LobbyListManager> ();
		wrm = waitingRoomManager.GetComponent<WaitingRoomManager> ();
		Connect();
	}
		
	/// <summary>
	/// Start the connection process. 
	/// Connect this application instance to Photon Cloud Network
	/// </summary>
	public void Connect()
	{

		// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
		if (!PhotonNetwork.connected)
		{
			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings(_gameVersion);
		}
	}


	public void Disconnect() {
		// Go back to the main menu
	}

	public void ConnectToRoom() {
		string selectedLobby = llm.GetSelectedLobby ();

		/* Determines whether or not the player is creating a lobby 
		 * or joining the selected lobby
		 */
		if (selectedLobby != null) {
			lobbyName = selectedLobby;
		} else {
			lobbyName = lobbyNameManager.GetComponent<LobbynameManager> ().GetLobbyname ();
		}
		if (lobbyName != null) {
			PhotonNetwork.playerName = PlayerPrefs.GetString (playerNamePrefKey, "Mike");
			PhotonNetwork.JoinOrCreateRoom (lobbyName, new RoomOptions () { MaxPlayers = MaxPlayersPerRoom }, TypedLobby.Default);
		}
	}

	public void DisconnectFromRoom() {
		PhotonNetwork.LeaveRoom (true);

	}

	// Called when a player requests to "Refresh" the menu
	public void RefreshAvailableRooms() {
		RoomInfo[] ri = PhotonNetwork.GetRoomList ();
		// Display all available rooms
		for (int i = 0; i < ri.Length; i++) {
			llm.SetLobbyInfo (i + 1, ri [i].Name, ri [i].PlayerCount, true);
		}
		// Make sure that only current rooms are displayed, and not old ones
		for (int i = 3; i > ri.Length; i--) {
			llm.SetLobbyInfo (i, "NA", 0, false);
		}
	}

	public void BeginMatch() {

		// Only the host may start the game
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.LoadLevel (GameObject.Find ("NavigationManager").GetComponent<NavigationManager> ().sceneOnStart);
		} else {
			// Tell everyone that the player is ready
			wrm.playerIsReady = !wrm.playerIsReady;
		}
	}

	void OnJoinedRoom() {
		// Master client joined, set playerCount to 1
		if (PhotonNetwork.isMasterClient) {
			playerCount = 1;
		}
		wrm.DisplayWindow ();
	}

	void OnPhotonPlayerConnected(PhotonPlayer other) {
		
		// Player joined, master client increments playerCount
		if (PhotonNetwork.isMasterClient) {
			playerCount++;

			// Hide and close the room if it is full
			if (playerCount == MaxPlayersPerRoom) {
				PhotonNetwork.room.IsVisible = true;
				PhotonNetwork.room.open = false;
			}



		}
		wrm.UpdateWindow ();

	}

	void OnPhotonPlayerDisconnected(PhotonPlayer other) {

		// Player joined, master client increments playerCount
		if (PhotonNetwork.isMasterClient) {
			playerCount--;

			// Show and open the room for others to join
			if (playerCount < MaxPlayersPerRoom) {
				PhotonNetwork.room.IsVisible = true;
				PhotonNetwork.room.IsOpen = true;
			}
		}
		wrm.UpdateWindow ();

	}


}