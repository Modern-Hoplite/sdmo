﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attack Up
 * 
 * Raises the damage of every attack
 */
public class UnitSkillAttackUp : UnitSkill
{
	private float multiplier = 1.0f;

	public override void OnAttack (AttackData attackData)
	{
		attackData.SetDamage ((int)(attackData.GetDamage() * multiplier));
	}

	public override string GetName ()
	{
		return "Attack Up";
	}
}
