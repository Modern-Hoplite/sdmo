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
	public float energy = 100f, energyMax = 100f, energyRegen = 20f,
	hp = 1000f, hpMax = 1000f,
	sp = 50f, spMax = 100f;

	private float vSpeed = 0f;
	private bool usedEnergyThisFrame = false;

	public UnitWeapon currentWeapon;

	[HideInInspector]
	public float backBoostRemainingTime = 0f;

	private bool deadRobot = false;

	public void Awake()
	{
		int unitID = (int)photonView.owner.CustomProperties ["UnitID"];
		unit = UnitList.GetUnit (unitID);

		GameObject go = Instantiate(Resources.Load(unit.GetRessourcePath(), typeof(GameObject))) as GameObject;
		go.transform.parent = transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localRotation = Quaternion.identity;

		sub = go.GetComponent<MechaSub> ();
		sub.m = this;
		charC = GetComponent<CharacterController> ();

		currentWeapon = unit.GetWeapon2 ();
	}

	public void Update()
	{
		MechaInput.Poll (Time.deltaTime);
	}

	public void FixedUpdate()
	{
		Vector3 aimDirectionHor = (new Vector3 (aimDirection.x, 0f, aimDirection.z)).normalized;
		transform.LookAt (transform.position + aimDirectionHor);
		float deltaTime = Time.fixedDeltaTime;

		if (photonView.isMine) {
			Vector3 inputMov = Vector3.right * MechaInput.movement.x + Vector3.forward * MechaInput.movement.y;
			usedEnergyThisFrame = false;



			// Movement variables, should be pulled from the units at a later point
			float verticalMinSpeed = 20f, verticalMaxSpeed = 15f, boostVerticalMinSpeed = 10f,
				boostSideVerticalMinSpeed = 12f, boostBackVerticalMinSpeed = 6f;
			float speedWalk = 8f, speedBoost = 20f, speedBoostSide = 18f;
			float jumpSpeed = 30f, gravity = 45f;
			float energyJump = 20f, energyBoost = 30f, energyBoostSide = 25f;
			float speedBoostBack = 80f, energyBoostBack = 20f, boostBackTime = 0.4f;



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
				MechaInput.jump = (MechaInput.boosting ? MechaInput.jump : false);
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
			if (MechaInput.switchWeapon1)
				unit.GetWeapon1 ().SwitchToWeapon (this);
			if (MechaInput.switchWeapon2)
				unit.GetWeapon2 ().SwitchToWeapon (this);
			if (MechaInput.switchWeapon3)
				unit.GetWeapon3 ().SwitchToWeapon (this);

			foreach (Transform t in sub.GetAllFirePoints())
				t.LookAt (aimPoint);

			if(MechaInput.shoot)
			{
				currentWeapon.UseWeapon (this);
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
		if(sp >= spMax/2f)
			activeSkills.Add (unit.GetSkill2 ());

		return activeSkills;
	}

	public int GetCurrentWeaponID()
	{
		return GetWeaponID (currentWeapon);
	}

	public int GetWeaponID(UnitWeapon w)
	{
		int cwid = 0;

		if (w == unit.GetWeapon1 ())
			cwid = 1;
		if (w == unit.GetWeapon2 ())
			cwid = 2;
		if (w == unit.GetWeapon3 ())
			cwid = 3;

		return cwid;
	}

	// Applies damage to this mecha.
	// If not local, calls RPCGetHit on the remote
	public void GetHit(AttackData attackData)
	{
		if (photonView.isMine) {
			foreach (UnitSkill s in GetActiveSkills()) {
				s.OnDefense (attackData);
			}
			hp -= attackData.GetDamage ();
			if (hp <= 0f)
				Death ();
		} else {
			object[] param = { attackData.GetDamage (), (int)attackData.GetAttackType () };
			photonView.RPC ("RPCGetHit", photonView.owner, param);
		}
	}
	
	// Recieves an attack from a remote
	[PunRPC]
	public void RPCGetHit(int damage, int type)
	{
		AttackData ad = new AttackData (damage, (AttackType)type);
		GetHit (ad);
	}

	public void Death()
	{
		if (deadRobot)
			return;
		deadRobot = true;
		ManagerLocal.i.MakeRespawnCam ();
		PhotonNetwork.Destroy (photonView);
	}

	// Called by UnitWeapon of another client
	/*[PunRPC]
	public void RPCFireWeapon(int weapon, Vector3 position, Quaternion rotation)
	{
		switch (weapon) {
		case 3:
			unit.GetWeapon3 ().Shoot (this, position, rotation);
			break;
		case 2:
			unit.GetWeapon2 ().Shoot (this, position, rotation);
			break;
		default:
			unit.GetWeapon1 ().Shoot (this, position, rotation);
			break;
		}
	}*/
}
