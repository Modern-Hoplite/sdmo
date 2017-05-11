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

	public Animator animSyst;

	private int animID;
	private AnimationData lastAnim = null;

	private Vector3 previousPosition = Vector3.zero;

	public void Update()
	{
		if (!m)
			return;

		AnimationSet s = m.unit.GetAnimationSet ();
		List<AnimationData> l = m.unit.GetAnimations();
		AnimationData curAnim = s.currentAnim;
		if (m.photonView.isMine) {
			animID = l.IndexOf(curAnim);

			animSyst.SetFloat ("InputX", MechaInput.movement.x);
			animSyst.SetFloat ("InputY", MechaInput.movement.y);
		} else {
			curAnim = l [animID];
			Vector3 posDiff = transform.position - previousPosition;
			Vector3 posDiffT = transform.TransformDirection (posDiff);
			Vector3 posDiffTHor = new Vector3 (posDiffT.x, 0f, posDiffT.z);
			posDiffTHor.Normalize ();
			animSyst.SetFloat ("InputX", posDiffTHor.x);
			animSyst.SetFloat ("InputY", posDiffTHor.z);
		}

		//return; // TEMP To check the internal system

		if (lastAnim != curAnim) {
			animSyst.Play (curAnim.animNameSystem);
		}
		lastAnim = curAnim;
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
		stream.SendNext (animID);
	}

	public void PhotonRecieve(PhotonStream stream, PhotonMessageInfo info)
	{
		animID = (int)stream.ReceiveNext ();
		previousPosition = transform.position;
	}
}
