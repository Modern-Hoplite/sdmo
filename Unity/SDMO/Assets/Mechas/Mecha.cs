using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class Mecha : PunBehaviour
{
	public Unit unit;

	private int tempColorID = 0;
	public GameObject[] tempColors;

	public void Awake()
	{
		unit = new Unit (1, "Gundam");
	}

	public void Update()
	{
		if (photonView.isMine) {
			if (Input.GetKeyDown (KeyCode.UpArrow))
				tempColorID++;
			else if (Input.GetKeyDown (KeyCode.DownArrow))
				tempColorID--;
			tempColorID = Mathf.Clamp (tempColorID, 0, tempColors.Length-1);
		} else {
			
		}

		int i = 0;
		foreach (GameObject go in tempColors) {
			go.SetActive (tempColorID == i);
			i++;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (tempColorID);
		} else {
			tempColorID = (int) stream.ReceiveNext ();
		}
	}

}
