using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

/* Main entity for each player
 * Mecha, along with the rest of its game object will process most of the gameplay code related to the current player
 * It uses an Unit to get its stats and a MechaSub to delegate the unit-specific operations (mostly graphics
 */
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhotonView))]
public class Mecha : PunBehaviour
{
	public CharacterController charC;
	public Unit unit;
	public MechaSub sub;
	public Vector3 aimDirection = Vector3.forward, aimPoint = Vector3.zero;

	[HideInInspector]
	public float energy = 100f, energyMax = 100f, energyRegen = 20f;

	private float vSpeed = 0f;
	private bool usedEnergyThisFrame = false;

	public void Awake()
	{
		unit = UnitList.GetUnit (1);

		GameObject go = Instantiate(Resources.Load("units/" + unit.GetID (), typeof(GameObject))) as GameObject;
		//GameObject go = Instantiate ("units/" + unit.GetID (), transform.position, Quaternion.identity);
		go.transform.parent = transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;
		sub = go.GetComponent<MechaSub> ();
		sub.m = this;
		charC = GetComponent<CharacterController> ();
	}

	public void FixedUpdate()
	{
		Vector3 aimDirectionHor = (new Vector3 (aimDirection.x, 0f, aimDirection.z)).normalized;
		transform.LookAt (transform.position + aimDirectionHor);
		float deltaTime = Time.fixedDeltaTime;

		if (photonView.isMine) {
			MechaInput.Poll (deltaTime);
			Vector3 inputMov = Vector3.right * MechaInput.movement.x + Vector3.forward * MechaInput.movement.y;
			usedEnergyThisFrame = false;

			float verticalMinSpeed = 20f, verticalMaxSpeed = 15f, boostVerticalMinSpeed = 10f;
			float speedWalk = 8f, speedBoost = 20f;
			float jumpSpeed = 30f, gravity = 45f;
			float energyJump = 40f, energyBoost = 30f;

			Vector3 movement = Vector3.zero;

			verticalMinSpeed = (charC.isGrounded ? 0.0f : -verticalMinSpeed);

			if (MechaInput.boosting && UseEnergy(energyBoost * deltaTime)) {
				Vector3 boostDir = aimDirectionHor + Vector3.up * Mathf.Max (0.0f, aimDirection.y);
				boostDir.Normalize ();
				movement += boostDir * speedBoost;
				verticalMinSpeed = Mathf.Max (verticalMinSpeed, -boostVerticalMinSpeed);
			} else {
				MechaInput.boosting = false;
				movement += transform.TransformDirection (inputMov) * speedWalk;
			}

			if (!MechaInput.boosting && MechaInput.jump && UseEnergy (energyJump * deltaTime)) {
				vSpeed += jumpSpeed * deltaTime;
			} else {
				vSpeed -= gravity * deltaTime;
			}

			vSpeed += movement.y;
			movement -= Vector3.up * movement.y;
			vSpeed = Mathf.Clamp (vSpeed, verticalMinSpeed, verticalMaxSpeed);
			movement += Vector3.up * vSpeed;
			charC.Move (movement * deltaTime);

			if (!usedEnergyThisFrame)
				energy += energyRegen * deltaTime;
			energy = Mathf.Clamp (energy, 0.0f, energyMax);

			if(MechaInput.shoot)
			{
				unit.GetWeapon1 ().UseWeapon (this);
			}

		} else {
		}
	}

	// Returns true and uses energy if you have enough energy
	public bool UseEnergy(float en)
	{
		if (en > energy)
			return false;
		usedEnergyThisFrame = true;
		energy -= en;
		return true;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (aimDirection);
			sub.PhotonSend (stream, info);
		} else {
			transform.position = (Vector3)stream.ReceiveNext ();
			aimDirection = (Vector3)stream.ReceiveNext ();
			sub.PhotonRecieve (stream, info);
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
