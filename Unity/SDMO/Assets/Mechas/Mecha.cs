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

	[HideInInspector]
	public float backBoostRemainingTime = 0f;

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

			// Movement variables, should be pulled from the units at a later point
			float verticalMinSpeed = 20f, verticalMaxSpeed = 15f, boostVerticalMinSpeed = 10f,
				boostSideVerticalMinSpeed = 12f, boostBackVerticalMinSpeed = 6f;
			float speedWalk = 8f, speedBoost = 20f, speedBoostSide = 18f;
			float jumpSpeed = 30f, gravity = 45f;
			float energyJump = 40f, energyBoost = 30f, energyBoostSide = 25f;
			float speedBoostBack = 80f, energyBoostBack = 30f, boostBackTime = 0.4f;

			// Normal movement
			Vector3 movement = Vector3.zero;
			movement = transform.TransformDirection (inputMov) * speedWalk;

			verticalMinSpeed = (charC.isGrounded ? 0.0f : -verticalMinSpeed);

			if (MechaInput.boosting) {
				if (MechaInput.boostingDirection.y > 0f && UseEnergy (energyBoost * deltaTime)) { // FORWARD BOOST
					Vector3 boostDir = aimDirectionHor + Vector3.up * Mathf.Max (0.0f, aimDirection.y);
					boostDir.Normalize ();
					if (MechaInput.jump) {
						boostDir += Vector3.up;
						boostDir.Normalize ();
					}
					movement = boostDir * speedBoost;
					verticalMinSpeed = Mathf.Max (verticalMinSpeed, -boostVerticalMinSpeed);
				} else if (MechaInput.boostingDirection.x != 0f && UseEnergy (energyBoostSide * deltaTime)) { // SIDEWAYS BOOST
					Vector3 boostDir = transform.TransformDirection(MechaInput.boostingDirection);
					if (MechaInput.jump) {
						boostDir += Vector3.up;
						boostDir.Normalize ();
					}
					movement = boostDir * speedBoostSide;
					verticalMinSpeed = Mathf.Max (verticalMinSpeed, -boostSideVerticalMinSpeed);
				} else if (MechaInput.boostingDirection.y < 0f && backBoostRemainingTime <= 0f && UseEnergy (energyBoostBack)) { // BACKWARDS BOOST
					backBoostRemainingTime = boostBackTime;
				} else
					MechaInput.boosting = false;
			} else
				MechaInput.boosting = false;

			// Back boost overrides movement for a time
			if (backBoostRemainingTime > 0f) {
				backBoostRemainingTime -= deltaTime;
				verticalMinSpeed = Mathf.Max (verticalMinSpeed, -boostBackVerticalMinSpeed);
				movement = -aimDirectionHor * speedBoostBack;
				usedEnergyThisFrame = true;

				if (backBoostRemainingTime > 0f) {
					MechaInput.boosting = true;
					MechaInput.boostingDirection = Vector2.down;
				} else {
					MechaInput.boosting = false;
				}
			}

			// Jumping
			if (!MechaInput.boosting && MechaInput.jump && UseEnergy (energyJump * deltaTime)) {
				vSpeed += jumpSpeed * deltaTime;
			} else {
				vSpeed -= gravity * deltaTime;
			}

			// Vertical movement adjust and actual movement
			vSpeed += movement.y;
			movement -= Vector3.up * movement.y;
			vSpeed = Mathf.Clamp (vSpeed, verticalMinSpeed, verticalMaxSpeed);
			movement += Vector3.up * vSpeed;
			charC.Move (movement * deltaTime);

			// Energy regen
			if (!usedEnergyThisFrame)
				energy += energyRegen * deltaTime;
			energy = Mathf.Clamp (energy, 0.0f, energyMax);

			// Weapons
			if(MechaInput.shoot)
			{
				unit.GetWeapon1 ().UseWeapon (this);
			}

		} else {
			// TODO Interpolation/Extrapolation
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
