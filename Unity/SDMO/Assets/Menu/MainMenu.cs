using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class MainMenu : PunBehaviour
{
	private string playerName = "Undefined";
	private int unitID = 1;

	public void Awake()
	{
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.ConnectUsingSettings ("alpha1");

		string[] randomNames = { "Amuro Ray", "Char Aznable", "Kamille Bidan", "Judau Ashta", "Ramba Ral", "Garma Zabi", "Heero Yuy", "Bright Noa",
		"Setsuna F. Seiei", "Lockon Stratos", "Loran Cehack", "Domon Kashu", "Garrod Ran", "Kira Yamato", "Mikazuki Augus", "Bellri Zenam",
		"Shiro Amada", "Anavel Gato", "Uso Ebin", "Duo Maxwell"};
		playerName = randomNames[Random.Range(0, randomNames.Length)];
	}

	public void OnGUI()
	{
		if (PhotonNetwork.connectedAndReady) {
			GUI.Box (new Rect(50, 50, 100, 50), "Name :");
			playerName = GUI.TextField (new Rect (200, 50, Screen.width - 250, 50), playerName);
			if (GUI.Button (new Rect (50, 150, Screen.width - 100, Screen.height - 200), "Join game")) {
				if (playerName == null)
					playerName = "Unknown Soldier " + Random.Range(0,100);
				PhotonNetwork.playerName = playerName;
				RoomOptions ro = new RoomOptions ();
				ro.MaxPlayers = 16;
				PhotonNetwork.JoinOrCreateRoom ("TestRoom", ro, TypedLobby.Default);
			}
		} else {
			GUI.Box (new Rect (50, 50, Screen.width - 100, 50), "Not ready");
			GUI.Box (new Rect (50, 150, Screen.width - 100, Screen.height - 200), "State : " + PhotonNetwork.connectionStateDetailed.ToString());
		}
	}

	public override void OnJoinedRoom ()
	{
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.LoadLevel ("BlankRoom");
		}
	}
}
