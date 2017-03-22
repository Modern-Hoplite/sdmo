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
	public Transform[] firePoint1, firePoint2, firePoint3;

	public Transform[] tmpAnim;
	private int anim;

	public void Update()
	{
		if (m.photonView.isMine) {
			anim = 0;
			if (MechaInput.jump)
				anim = 1;
			if (MechaInput.boosting) {
				if (MechaInput.boostingDirection == Vector2.up)
					anim = 2;
				else if (MechaInput.boostingDirection == Vector2.down)
					anim = 3;
				else if (MechaInput.boostingDirection == Vector2.right)
					anim = 4;
				else
					anim = 5;
			}
		} else {
		}

		int i = 0;
		foreach (Transform t in tmpAnim) {
			t.gameObject.SetActive (i == anim);
			i++;
		}
	}

	public Transform[] GetActiveFirePoints()
	{
		return GetFirePoints (m.GetCurrentWeaponID ());
	}

	public Transform[] GetFirePoints(int weaponID)
	{
		switch (weaponID) {
		case 3:
			return firePoint3;
		case 2:
			return firePoint2;
		default:
			return firePoint1;
		}
	}

	public Transform[] GetAllFirePoints()
	{
		Transform[][] fpl = { firePoint1, firePoint2, firePoint3 };
		int nbFP = 0;
		foreach (Transform[] fp in fpl)
			nbFP += fp.Length;

		Transform[] t = new Transform[nbFP];

		int i = 0;
		foreach (Transform[] fp in fpl) {
			fp.CopyTo (t, i);
			i += fp.Length;
		}
		return t;
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
