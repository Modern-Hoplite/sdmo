﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This represents the weapon of a unit
 * It takes care of everything that happens until the shot is fired
 */
public class UnitWeapon
{
	private string name = "Gun";
	private AttackData attackData = new AttackData();

	public UnitWeapon()
	{}

	public UnitWeapon(string name, AttackData attackData)
	{
		this.name = name;
		this.attackData = attackData;
	}

	public virtual string GetName() { return name; }
	public virtual void SetName(string name) { this.name = name; }

	public virtual AttackData GetAttackData() { return attackData; }
	public virtual void SetAttackData(AttackData attackData) { this.attackData = attackData; }

	public virtual void UseWeapon(Mecha m) {}
}