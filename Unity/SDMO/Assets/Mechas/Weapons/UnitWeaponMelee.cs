using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeaponMelee : UnitWeapon
{
	protected List<Mecha> hitList = new List<Mecha>();
	public float meleeRange = 1.5f, meleeHeight = 1f, meleeArc = 40f;
	public int combo = 0;

	private List<AnimationData> comboAnims = new List<AnimationData>();

	public UnitWeaponMelee(string name, AttackData attackData)
	{
		Constructor (name, attackData);
	}
		
	public override bool UseWeapon (Mecha m)
	{
		hitList.Clear();
		hitList.Add (m);

		// check if mecha in front for boost
		// link to getanimuse

		return base.UseWeapon(m);
	}

	public override AnimationData GetAnimUse ()
	{
		// COMBO SYSTEM HERE
		combo = combo % comboAnims.Count;

		return comboAnims [combo];
	}

	public override void Shoot (Mecha m)
	{
		//Debug.Log ("MELEE START");
		foreach (Mecha d in ManagerLocal.i.GetMechaList()) {
			if (d && !hitList.Contains (d)) {
				//Debug.Log ("HITLIST OK");
				if (MeleeHitbox (m, d)) {
					MeleeHit (d);
				}
			}
		}
	}

	protected bool MeleeHitbox(Mecha atk, Mecha def)
	{
		//Debug.Log ("M HITBOX CHECK");
		Vector3 ad = def.transform.position - atk.transform.position;
		float adSqrMag = ad.sqrMagnitude;

		float rangeCheck = meleeRange + def.charC.radius;
		//Debug.Log ("Range check : " + (rangeCheck*rangeCheck)+ " <" + adSqrMag);
		if (rangeCheck * rangeCheck < adSqrMag)
			return false;

		//Debug.Log ("RANGE PASS");
		/*
		// Height check for cylinder collider
		float defMidPoint = def.transform.position + def.charC.height / 2f;
		float atkMidPoint = atk.transform.position + atk.charC.height / 2f;
		float heightDiff = Mathf.Abs (defMidPoint - atkMidPoint);
		heightDiff -= def.charC.height/2f;
		if (heightDiff > meleeHeight)
			return false;*/

		// TODO SIMPLIFIED ARC CHECK
		// Should be completed to be more accurate
		float dotP = Vector3.Dot(atk.transform.TransformDirection(Vector3.forward), ad.normalized);
		float cosArc = Mathf.Cos (meleeArc * Mathf.Deg2Rad);
		//Debug.Log ("DOTP = " + dotP + " / " + cosArc);

		return dotP >= cosArc;
	}

	protected void MeleeHit(Mecha target)
	{
		//Debug.Log ("MELEE HIIIIT");
		target.GetHit (GetAttackData ());
		hitList.Add (target);
	}

	public void AddComboAnim(AnimationData attack)
	{
		comboAnims.Add (attack);
	}

	public List<AnimationData> GetComboAnims()
	{
		return comboAnims;
	}

	public override List<AnimationData> GetAnimations ()
	{
		List<AnimationData> l = new List<AnimationData> ();
		l.Add (GetAnimSwitch ());
		l.AddRange (GetComboAnims ());
		return l;
	}
}
