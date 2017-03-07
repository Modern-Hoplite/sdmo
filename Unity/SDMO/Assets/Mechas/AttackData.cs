using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attack Data is a simple structure that contains the properties of a single attack
 * It reprents a shot or melee attack after it is fired.
 */
public class AttackData
{
	private int damage;
	private AttackType type;

	public AttackData(int damage = 50, AttackType type = AttackType.True)
	{
		this.damage = damage;
		this.type = type;
	}

	public AttackData Clone()
	{
		return new AttackData(damage, type);
	}

	public virtual int GetDamage() { return damage; }
	public virtual void SetDamage(int damage) { this.damage = damage; }

	public virtual AttackType GetAttackType() { return type; }
	public virtual void SetAttackType(AttackType type) { this.type = type; }
}

public enum AttackType
{
	True, Physical, Beam
}