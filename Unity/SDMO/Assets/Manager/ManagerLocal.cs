using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ManagerLocal : PunBehaviour
{
	public static ManagerLocal i;
	public static Mecha mecha;

	public void Awake()
	{
		i = this;

		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.InstantiateSceneObject ("manager-shared", Vector3.zero, Quaternion.identity, 0, null);
		}
	}

	public void Start()
	{
		Transform spawnPoint = ManagerLevel.i.spawnPoints[PhotonNetwork.player.ID % ManagerLevel.i.spawnPoints.Length];

		GameObject go = PhotonNetwork.Instantiate ("mecha", spawnPoint.position, spawnPoint.rotation, 0);
		mecha = go.GetComponent<Mecha> ();
	}

	public void OnGUI()
	{
		// Temporary
		int i = 0, height = 30;
		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			GUI.Box (new Rect (0, height * i, 200, height), (p.IsMasterClient ? "[M] " : "") + "("+p.ID+") "+p.NickName);
			i++;
		}

		GUI.Box (new Rect(Screen.width-150, 0, 150, 50), "Arrow Keys to move\nSpace to shoot");
	}
}
