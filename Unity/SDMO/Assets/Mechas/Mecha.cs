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
			stream.SendNext (transform.rotation);
		} else {
			transform.position = (Vector3)stream.ReceiveNext ();
			transform.rotation = (Quaternion)stream.ReceiveNext ();
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
