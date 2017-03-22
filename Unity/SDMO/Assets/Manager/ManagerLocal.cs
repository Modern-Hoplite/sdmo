using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

/* Manager Local is not shared between clients
 * 
 * It is used by the master client to process the game logic
 * All of its data is stored in room properties so in case of a disconnect any client can take up the role
 */
public class ManagerLocal : PunBehaviour
{
	public static ManagerLocal i;
	public static Mecha mecha; // Refers to this player's mecha

	public static string versionName = "Demo";
	public static int versionNumber = 1;

	public GameObject respawnCam;

	public void Awake()
	{
		i = this;

		Cursor.lockState = CursorLockMode.Locked;

		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.InstantiateSceneObject ("manager-shared", Vector3.zero, Quaternion.identity, 0, null);
		}
	}

	public void Start()
	{
		MakeRespawnCam ();
	}

	public void MakeRespawnCam()
	{
		Instantiate (respawnCam);
	}

	public void SpawnMe()
	{
		Transform spawnPoint = ManagerLevel.i.spawnPoints[PhotonNetwork.player.ID % ManagerLevel.i.spawnPoints.Length];

		GameObject go = PhotonNetwork.Instantiate ("mecha", spawnPoint.position, spawnPoint.rotation, 0);
		mecha = go.GetComponent<Mecha> ();
	}

	public void OnGUI()
	{
		// Temporary list of players
		int i = 0, height = 30;
		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			string s = "";
			s += (p.IsMasterClient ? "[M] " : "");
			s += (p == PhotonNetwork.player ? "> " : "");
			s += "(" + p.ID + ") " + p.NickName;
			GUI.Box (new Rect (0, height * i, 200, height), s);
			i++;
		}
	}
}
