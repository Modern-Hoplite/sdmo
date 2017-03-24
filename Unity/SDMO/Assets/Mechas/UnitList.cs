using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Unit List
 * Contains some static methods to centralize access to units.
 * 
 */
public class UnitList
{
	public static Unit GetUnit(int id)
	{
		int unitID = id % 100;
		int seriesID = (unitID - id) / 100;

		switch (seriesID) {
		case 0:
			return List000 (unitID);
		default:
			return UndefinedUnit (id);
		}

		/* Transformable unit template
		 if (id == 2) {
			Unit untransformed = new Unit (id, "Gundam (Magnetic Coating)", UnitRank.A);
			Unit transformed = new Unit (id, "Gundam (Last Stand)", UnitRank.A);
			u = new UnitTransform (untransformed, transformed);
		}*/
	}
		
	private static Unit UndefinedUnit(int id)
	{
		string name = "Toastron" + (id == 0 ? "" : " (" + id + ")");
		Unit u = new Unit (id, name, UnitRank.B);
		UnitSkill s1, s2;
		UnitWeapon w1, w2, w3;

		s1 = new UnitSkillAttackUp ();
		s2 = new UnitSkillAttackUp ();

		w1 = new UnitWeaponMelee ("Melee", new AttackData());
		w2 = new UnitWeaponHitscan ("Water Launcher", new AttackData());

		w3 = new UnitWeaponHitscan ("Microwave Burst", new AttackData());

		AnimationSet animSet = u.GetAnimationSet ();
		animSet.stand = new AnimationData ("stand", "Stand", 0, 60, 24f);
		animSet.boostF = new AnimationData ("boostF", "Boost Forward", 10, 1, 24f);
		animSet.boostL = new AnimationData ("boostL", "Boost Left", 10, 1, 24f);
		animSet.boostR = new AnimationData ("boostR", "Boost Right", 10, 1, 24f);
		animSet.boostB = new AnimationData ("boostB", "Boost Back", 10, 1, 24f);
		animSet.jump = new AnimationData ("jump", "Jump", 1, 1, 24f);

		AnimationData w1animSwitch = new AnimationData ("w1switch", "Weapon 1 Switch", 50, 24, 24);
		AnimationData w1animUse = new AnimationData ("w1use", "Weapon 1 Use", 100, 24, 24);
		AnimationData w2animSwitch = new AnimationData ("w2switch", "Weapon 2 Switch", 50, 24, 24);
		AnimationData w2animUse = new AnimationData ("w2use", "Weapon 2 Use", 100, 24, 24);
		AnimationData w3animSwitch = new AnimationData ("w3switch", "Weapon 3 Switch", 50, 24, 24);
		AnimationData w3animUse = new AnimationData ("w3use", "Weapon 3 Use", 100, 24, 24);

		w1.SetAnimSwitch (w1animSwitch);
		w1.SetAnimUse (w1animUse);
		w2.SetAnimSwitch (w2animSwitch);
		w2.SetAnimUse (w2animUse);
		w3.SetAnimSwitch (w3animSwitch);
		w3.SetAnimUse (w3animUse);

		AnimationData[][] anims = new AnimationData[4][];
		anims[0] = new AnimationData[]{ animSet.stand, animSet.jump };
		anims[1] = new AnimationData[]{ animSet.boostF, animSet.boostL, animSet.boostR, animSet.boostB };
		anims[2] = new AnimationData[]{ w1animSwitch, w2animSwitch, w3animSwitch};
		anims[3] = new AnimationData[]{w1animUse, w2animUse, w3animUse};

		int[] animsPrio = { 0, 10, 50, 100 }, animsPrioOverride = { 0, 100, 5, 50 };

		for (int j = 0; j < anims.Length; j++) {
			for (int i = 0; i < anims [j].Length; i++) {
				anims [j] [i].priority = animsPrio [j];
				anims [j] [i].minimumPriorityToCancel = animsPrioOverride [j];
			}
		}

		return BuildUnit(u,w1,w2,w3,s1,s2);
	}

	// Basic stuff
	private static Unit List000(int unitID)
	{
		int seriesID = 0;
		int id = seriesID * 100 + unitID;

		Unit unit;
		UnitSkill skill1, skill2;
		UnitWeapon weapon1, weapon2, weapon3;

		switch (unitID) {
		/*case 1:
			unit = new Unit (id, "Gundam", UnitRank.B);
			skill1 = new UnitSkillAttackUp ();
			skill2 = new UnitSkillAttackUp ();
			weapon1 = new UnitWeaponMelee ("Beam Saber", new AttackData());
			weapon2 = new UnitWeaponHitscan ("Beam Rifle", new AttackData());
			weapon3 = new UnitWeaponBuiltin(new UnitWeaponHitscan ("Vulcan", new AttackData()));
			break;*/
		default:
			return UndefinedUnit (id);
		}

		return BuildUnit (unit, weapon1, weapon2, weapon3, skill1, skill2);
	}

	// For convinence
	private static Unit BuildUnit(Unit unit, UnitWeapon weapon1, UnitWeapon weapon2, UnitWeapon weapon3, UnitSkill skill1, UnitSkill skill2)
	{
		unit.SetSkill1 (skill1);
		unit.SetSkill2 (skill2);
		unit.SetWeapon1 (weapon1);
		unit.SetWeapon2 (weapon2);
		unit.SetWeapon3 (weapon3);

		return unit;
	}
}
