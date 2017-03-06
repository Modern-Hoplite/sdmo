using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

/*
 * 
 * 
 * 
 */
[RequireComponent(typeof(PhotonView))]
public class Mecha : PunBehaviour
{
	public Unit unit;
	public MechaSub sub;

	public void Awake()
	{
		unit = UnitList.GetUnit (1);

		GameObject go = PhotonNetwork.Instantiate ("units/" + unit.GetID (), transform.position, Quaternion.identity, 0);
		go.transform.parent = transform;
		sub = go.GetComponent<MechaSub> ();
	}

	public void FixedUpdate()
	{
		if (photonView.isMine) {
			Vector3 inputMov = Vector3.right * Input.GetAxis ("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");

			transform.position += inputMov * unit.GetSpeed () * Time.fixedDeltaTime;

			if (inputMov.sqrMagnitude > 0f) {
				transform.LookAt (transform.position + inputMov);
			}

			if(Input.GetKeyDown(KeyCode.Space))
			{
				unit.GetWeapon1 ().UseWeapon (this);
			}

		} else {
			
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (transform.position);
		} else {
			transform.position = (Vector3)stream.ReceiveNext ();
		}
	}

	public List<UnitSkill> GetActiveSkills()
	{
		List<UnitSkill> activeSkills = new List<UnitSkill> ();

		activeSkills.Add (unit.GetSkill1 ());
		activeSkills.Add (unit.GetSkill2 ());

		return activeSkills;
	}
}
