using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class contrains info and objects specific to a unit.
 * It takes care mostly of the graphics.
 * 
 */
public class MechaSub : MonoBehaviour
{
	public Mecha m;
	public Transform firePoint;

	public Transform[] tmpAnim;
	private int anim;

	public void Update()
	{
		if (m.photonView.isMine) {
			anim = 0;
			if (MechaInput.jump)
				anim = 1;
			if (MechaInput.boosting)
				anim = 2;
		} else {
		}

		int i = 0;
		foreach (Transform t in tmpAnim) {
			t.gameObject.SetActive (i == anim);
			i++;
		}
	}

	public void PhotonSend(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext (anim);
	}

	public void PhotonRecieve(PhotonStream stream, PhotonMessageInfo info)
	{
		anim = (int)stream.ReceiveNext ();
	}
}
