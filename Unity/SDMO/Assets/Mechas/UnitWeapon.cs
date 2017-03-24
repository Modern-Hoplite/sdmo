using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This represents the weapon of a unit
 * It takes care of everything that happens until the shot is fired
 */
public class UnitWeapon
{
	private string name = "Gun";
	private AttackData attackData = new AttackData();
	private int burst = 1;	// How many shots per use
	private float burstInterval = 0.1f; // Interval between shots

	private string sfxShot = "beam-sfx";
	private int burstID = 0;

	private AnimationData animSwitch, animUse;

	public UnitWeapon()
	{
		
	}

	public UnitWeapon(string name, AttackData attackData)
	{
		Constructor (name, attackData);
	}

	protected void Constructor(string name, AttackData attackData)
	{
		this.name = name;
		this.attackData = attackData;
	}

	public virtual string GetName() { return name; }
	public virtual void SetName(string name) { this.name = name; }

	public virtual AttackData GetAttackData() { return attackData; }
	public virtual void SetAttackData(AttackData attackData) { this.attackData = attackData; }
	public virtual AnimationData GetAnimSwitch() { return animSwitch; }
	public virtual void SetAnimSwitch(AnimationData d) { animSwitch = d; }
	public virtual AnimationData GetAnimUse() { return animUse; }
	public virtual void SetAnimUse(AnimationData d) { animUse = d; }

	// Should be called by the player to start using the weapon
	public virtual bool UseWeapon(Mecha m)
	{
		return m.unit.GetAnimationSet ().PlayAnim (animUse);
	}

	// Should be called to usually fire the weapon
	public virtual void Shoot(Mecha m)
	{
		Transform firePoint = BurstFirePoints (FirePoints(m));

		FireShot (m, firePoint);
	}

	protected virtual Transform BurstFirePoints(Transform[] firePoints)
	{
		if (firePoints.Length <= 0)
			return null;

		Transform firePoint;

		burstID = burstID % firePoints.Length;

		firePoint = firePoints [burstID];

		burstID++;

		return firePoint;
	}

	protected virtual Transform[] FirePoints (Mecha m)
	{
		return m.sub.GetActiveFirePoints ();
	}

	// The actual shot, bypasses all the logic of ammunition and stuff
	public virtual void FireShot(Mecha m, Transform firePoint) {
		Debug.LogError ("["+m.unit.GetName()+"/"+GetName()+"] UnitWeapon.Shoot() : You should redifine Shoot() if you want this to do anything");
	}

	public virtual void ShotSFX(Mecha m, Vector3 firePointPos, Quaternion firePointRot) {
		ManagerShared.i.CreateSFX (sfxShot, firePointPos, firePointRot);
	}

	public virtual void SwitchToWeapon(Mecha m)
	{
		if(m.unit.GetAnimationSet().PlayAnim(GetAnimSwitch()))
			m.currentWeapon = this;
	}

	public virtual List<AnimationData> GetAnimations()
	{
		List<AnimationData> l = new List<AnimationData> ();
		l.Add (GetAnimSwitch ());
		l.Add (GetAnimUse ());
		return l;
	}
}
