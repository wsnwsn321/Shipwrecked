using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListManager : MonoBehaviour {

	public GameObject lobby1, lobby2, lobby3;
	private Text lobby1Name, lobby2Name, lobby3Name;
	private Text lobby1PC, lobby2PC, lobby3PC;

	private Color unselectedColor;
	private Color selectedColor;
	private string selectedLobby;

	// Use this for initialization
	void Start () {

		// Get hooks on lobby info
		lobby1Name = lobby1.transform.Find("LobbyName").gameObject.GetComponent<Text>();
		lobby2Name = lobby2.transform.Find("LobbyName").gameObject.GetComponent<Text>();
		lobby3Name = lobby3.transform.Find("LobbyName").gameObject.GetComponent<Text>();

		lobby1PC = lobby1.transform.Find("PlayerCount").gameObject.GetComponent<Text>();
		lobby2PC = lobby2.transform.Find("PlayerCount").gameObject.GetComponent<Text>();
		lobby3PC = lobby3.transform.Find("PlayerCount").gameObject.GetComponent<Text>();

		// Set selection colors
		unselectedColor = lobby1.GetComponent<Image> ().color;
		selectedColor = Color.blue;

		// Make all lobbies invisible 
		lobby1.SetActive (false);
		lobby2.SetActive (false);
		lobby3.SetActive (false);
	}

	public void SetLobbyInfo(int lobbyNum, string lobbyName, int playerCount, bool isActive) {
		switch(lobbyNum) {

		case (1):
			lobby1Name.text = lobbyName;
			lobby1PC.text = "" + playerCount + "/4";
			lobby1.SetActive (isActive);
			break;
		case (2):
			lobby2Name.text = lobbyName;
			lobby2PC.text = "" + playerCount + "/4";
			lobby2.SetActive (isActive);
			break;
		case (3):
			lobby3Name.text = lobbyName;
			lobby3PC.text = "" + playerCount + "/4";
			lobby3.SetActive (isActive);
			break;
		}
	}

	public void LobbyOneSelected() {
		lobby1.GetComponent<Image> ().color = selectedColor;
		lobby2.GetComponent<Image> ().color = unselectedColor;
		lobby3.GetComponent<Image> ().color = unselectedColor;
		selectedLobby = lobby1Name.text;

	}

	public void LobbyTwoSelected() {
		lobby1.GetComponent<Image> ().color = unselectedColor;
		lobby2.GetComponent<Image> ().color = selectedColor;
		lobby3.GetComponent<Image> ().color = unselectedColor;
		selectedLobby = lobby2Name.text;
	}

	public void LobbyThreeSelected() {
		lobby1.GetComponent<Image> ().color = unselectedColor;
		lobby2.GetComponent<Image> ().color = unselectedColor;
		lobby3.GetComponent<Image> ().color = selectedColor;
		selectedLobby = lobby3Name.text;
	}

	public void UnselectAll() {
		lobby1.GetComponent<Image> ().color = unselectedColor;
		lobby2.GetComponent<Image> ().color = unselectedColor;
		lobby3.GetComponent<Image> ().color = selectedColor;
		selectedLobby = null;
	}

	/// <summary>
	/// Gets the selected lobby.
	///
	/// </summary>
	/// <returns>The selected lobby or null if none is selected.</returns>
	public string GetSelectedLobby() {
		return selectedLobby;
	}


}
