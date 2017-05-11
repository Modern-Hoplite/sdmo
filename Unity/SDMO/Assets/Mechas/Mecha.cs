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

	public MechaMovement mMovement;

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
		mMovement = unit.GetBaseMechaMovement ();

		ManagerLocal.i.MechaRegister (this);
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
			//Vector3 inputMov = Vector3.right * MechaInput.movement.x + Vector3.forward * MechaInput.movement.y;
			usedEnergyThisFrame = false;

			// Basic animations
			AnimationSet s = unit.GetAnimationSet ();
			s.PlayAnim (ChooseAnim(s), this);

			// Movement
			Vector3 movement = mMovement.Movement (this, Vector3.zero);
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

			// Animations
			unit.GetAnimationSet ().ProgressAnim (this, Time.fixedDeltaTime);

		} else {
			// TODO Interpolation/Extrapolation
		}
	}

	private AnimationData ChooseAnim(AnimationSet s)
	{
		AnimationData animToPlay = s.stand;
		float energyJump = 20f, energyBoost = 30f, energyBoostSide = 25f, energyBoostBack = 20f;
		float deltaTime = Time.fixedDeltaTime;

		if (MechaInput.boosting) {
			if (MechaInput.boostingDirection.y > 0f && UseEnergy (energyBoost * deltaTime)) { // FORWARD BOOST
				animToPlay = s.boostF;
			} else if (MechaInput.boostingDirection.x != 0f && UseEnergy (energyBoostSide * deltaTime)) { // SIDEWAYS BOOST
				animToPlay = (MechaInput.boostingDirection.x < 0f ? s.boostL : s.boostR);
			} else if (MechaInput.boostingDirection.y < 0f && UseEnergy (energyBoostBack * deltaTime)) { // BACKWARDS BOOST
				animToPlay = s.boostB;
			} else
				MechaInput.boosting = false;
		} else
			MechaInput.boosting = false;

		// Jumping
		if (!MechaInput.boosting && MechaInput.jump && UseEnergy (energyJump * deltaTime)) {
			animToPlay = s.jump;
		} else {
			MechaInput.jump = (MechaInput.boosting ? MechaInput.jump : false);
		}

		return animToPlay;
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

		// TODO Add unit skills
		// TODO Add current weapon Skills

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

	// Returns Mecha nearest to the crosshair within the scan parameters
	public Mecha GetAutolock(Vector3 targetDir)
	{
		float maxAngle = 45f;
		float maxDistance = 5000f;

		float maxDistanceSqr = maxDistance * maxDistance;
		float maxAngleCos = Mathf.Cos (maxAngle);

		Mecha target = null;
		float lastTargetScore = -100f;

		foreach (Mecha m in ManagerLocal.i.GetMechaList()) {
			if (!m || m == this)
				continue;

			Vector3 posR = m.transform.position - transform.position;

			if (posR.sqrMagnitude > maxDistanceSqr)
				continue;

			float dot = Vector3.Dot(targetDir, posR.normalized);

			if (dot < maxAngleCos)
				continue;

			if (dot > lastTargetScore) {
				lastTargetScore = dot;
				target = m;
			}
		}

		return target;
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
