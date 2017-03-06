using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
	private int id = 0;
	private string name = "Undefined";
	private UnitRank rank = UnitRank.C;

	private UnitWeapon weapon1, weapon2, weapon3;
	private UnitSkill skill1, skill2;

	private float speed = 10f;
	private int hp = 1000;

	public Unit()
	{
		Setup (0, "Undefined", UnitRank.C, new UnitWeapon (), new UnitWeapon (), new UnitWeapon (), new UnitSkill (), new UnitSkill ());
	}

	public Unit(int id, string name, UnitRank rank)
	{
		Setup (id, name, rank, new UnitWeapon (), new UnitWeapon (), new UnitWeapon (), new UnitSkill (), new UnitSkill ());
	}

	public Unit(int id, string name, UnitRank rank, UnitWeapon weapon1, UnitWeapon weapon2, UnitWeapon weapon3, UnitSkill skill1, UnitSkill skill2)
	{
		Setup (id, name, rank, weapon1, weapon2, weapon3, skill1, skill2);
	}

	private void Setup(int id, string name, UnitRank rank, UnitWeapon weapon1, UnitWeapon weapon2, UnitWeapon weapon3, UnitSkill skill1, UnitSkill skill2)
	{
		this.id = id;
		this.name = name;
		this.rank = rank;
		this.weapon1 = weapon1;
		this.weapon2 = weapon2;
		this.weapon3 = weapon3;
		this.skill1 = skill1;
		this.skill2 = skill2;
	}
		
	public virtual void Transform() {}

	public virtual int GetID() { return id; }
	public virtual void SetID(int id) { this.id = id; }

	public virtual string GetName() { return name; }
	public virtual void SetName(string name) { this.name = name; }

	public virtual UnitRank GetRank() { return rank; }
	public virtual void SetName(UnitRank rank) { this.rank = rank; }

	public virtual UnitWeapon GetWeapon1() { return weapon1; }
	public virtual void SetWeapon1(UnitWeapon w) { weapon1 = w; }

	public virtual UnitWeapon GetWeapon2() { return weapon2; }
	public virtual void SetWeapon2(UnitWeapon w) { weapon2 = w; }

	public virtual UnitWeapon GetWeapon3() { return weapon3; }
	public virtual void SetWeapon3(UnitWeapon w) { weapon3 = w; }

	public virtual UnitSkill GetSkill1() { return skill1; }
	public virtual void SetSkill1(UnitSkill s) { skill1 = s; }

	public virtual UnitSkill GetSkill2() { return skill2; }
	public virtual void SetSkill2(UnitSkill s) { skill2 = s; }

	public virtual float GetSpeed() { return speed; }
	public virtual void SetSpeed(float speed) { this.speed = speed; } 
	public virtual int GetHp() { return hp; }
	public virtual void SetHp(int hp) { this.hp = hp; } 
}

public enum UnitRank
{
	C = 2, B = 3, A = 4, S = 5
}