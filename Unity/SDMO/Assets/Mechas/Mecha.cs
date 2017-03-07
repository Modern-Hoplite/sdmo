using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

/* Main entity for each player
 * Mecha, along with the rest of its game object will process most of the gameplay code related to the current player
 * It uses an Unit to get its stats and a MechaSub to delegate the unit-specific operations (mostly graphics
 */
[RequireComponent(typeof(PhotonView))]
public class Mecha : PunBehaviour
{
	public Unit unit;
	public MechaSub sub;
	public Vector3 aimDirection = Vector3.forward, aimPoint = Vector3.zero;

	public void Awake()
	{
		unit = UnitList.GetUnit (1);

		GameObject go = Instantiate(Resources.Load("units/" + unit.GetID (), typeof(GameObject))) as GameObject;
		//GameObject go = Instantiate ("units/" + unit.GetID (), transform.position, Quaternion.identity);
		go.transform.parent = transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		sub = go.GetComponent<MechaSub> ();
	}

	public void FixedUpdate()
	{
		Vector3 aimDirectionHor = (new Vector3 (aimDirection.x, 0f, aimDirection.z)).normalized;
		transform.LookAt (transform.position + aimDirectionHor);

		if (photonView.isMine) {
			MechaInput.Poll (Time.deltaTime);

			Vector3 inputMov = Vector3.right * MechaInput.movement.x + Vector3.forward * MechaInput.movement.y;

			transform.position += transform.TransformDirection(inputMov) * unit.GetSpeed () * Time.fixedDeltaTime;

			if(MechaInput.shoot)
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
			stream.SendNext (aimDirection);
		} else {
			transform.position = (Vector3)stream.ReceiveNext ();
			aimDirection = (Vector3)stream.ReceiveNext ();
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
