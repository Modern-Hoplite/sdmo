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

	// Very messy, will be replaced as soon as possible
	private static Unit UndefinedUnit(int id)
	{
		string name = "Toastron" + (id == 0 ? "" : " (" + id + ")");
		Unit u = new Unit (id, name, UnitRank.B);
		UnitSkill s1, s2;
		UnitWeapon w2, w3;
		UnitWeaponMelee w1;

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

		AnimationData w1animSwitch = new AnimationData ("w1switch", "Weapon 1 Switch", 50, 20, 24);
		AnimationData w2animSwitch = new AnimationData ("w2switch", "Weapon 2 Switch", 50, 24, 24);
		AnimationData w2animUse = new AnimationData ("w2use", "Weapon 2 Use", 100, 24, 24);
		AnimationData w3animSwitch = new AnimationData ("w3switch", "Weapon 3 Switch", 50, 24, 24);
		AnimationData w3animUse = new AnimationData ("w3use", "Weapon 3 Use", 100, 24, 24);

		AnimationData w1animDash = new AnimationData ("boostF", "Weapon 1 Dash", 100, 500, 24);
		AnimationData w1animCombo1 = new AnimationData ("w1c1", "Weapon 1 Combo 1", 100, 500, 24);
		AnimationData w1animCombo2 = new AnimationData ("w1c2", "Weapon 1 Combo 2", 100, 500, 24);
		AnimationData w1animCombo3 = new AnimationData ("w1c3", "Weapon 1 Combo 3", 100, 500, 24);
		AnimationData w1animCombo4 = new AnimationData ("w1c4", "Weapon 1 Combo 4", 100, 500, 24);
		AnimationData w1animCombo5 = new AnimationData ("w1c5", "Weapon 1 Combo 5", 100, 500, 24);

		w1.AddComboAnim (w1animDash);
		w1.AddComboAnim (w1animCombo1);
		w1.AddComboAnim (w1animCombo2);
		w1.AddComboAnim (w1animCombo3);
		w1.AddComboAnim (w1animCombo4);
		w1.AddComboAnim (w1animCombo5);

		AnimationEvent eventComboReset = new AnimationEventComboSet (w1, 0);

		w1.SetAnimSwitch (w1animSwitch);
		w1.SetAnimUse (w1animCombo1);
		w2.SetAnimSwitch (w2animSwitch);
		w2.SetAnimUse (w2animUse);
		w3.SetAnimSwitch (w3animSwitch);
		w3.SetAnimUse (w3animUse);

		w1animSwitch.AddEvent (new AnimationEventChangeWeapon (w1));
		w2animSwitch.AddEvent (new AnimationEventChangeWeapon (w2));
		w3animSwitch.AddEvent (new AnimationEventChangeWeapon (w3));

		AnimationData[][] anims = new AnimationData[5][];
		anims[0] = new AnimationData[]{ animSet.stand, animSet.jump };
		anims[1] = new AnimationData[]{ animSet.boostF, animSet.boostL, animSet.boostR, animSet.boostB };
		anims[2] = new AnimationData[]{ w1animSwitch, w2animSwitch, w3animSwitch};
		anims[3] = new AnimationData[]{ w2animUse, w3animUse};
		anims[4] = new AnimationData[]{ w1animDash, w1animCombo1, w1animCombo2, w1animCombo3, w1animCombo4, w1animCombo5 };

		int[] animsPrio = { 0, 10, 50, 100, 100 }, animsPrioOverride = { 0, 100, 5, 50, 10000 };

		for (int j = 0; j < anims.Length; j++) {
			for (int i = 0; i < anims [j].Length; i++) {
				anims [j] [i].priority = animsPrio [j];
				anims [j] [i].minimumPriorityToCancel = animsPrioOverride [j];

				if (j >= 2)
					anims [j] [i].AddEventAtEnd (new AnimationEventPlayAnim(animSet.stand));
				if (j != 4 && j != 2)
					anims [j] [i].AddEvent (eventComboReset);
			}
		}

		w2animUse.AddEvent (new AnimationEventShootWeapon(), 10);

		w3animUse.AddEvent (new AnimationEventShootWeapon(), 10);
		w3animUse.AddEvent (new AnimationEventShootWeapon(), 12);
		w3animUse.AddEvent (new AnimationEventShootWeapon(), 14);
		w3animUse.AddEvent (new AnimationEventShootWeapon(), 16);
		w3animUse.AddEvent (new AnimationEventShootWeapon(), 18);
		w3animUse.AddEvent (new AnimationEventShootWeapon(), 20);

		//w1animCombo1.AddEvent (new AnimationEventChangePriorityOverride (1000, 50), 24);
		//w1animCombo2.AddEvent (new AnimationEventChangePriorityOverride (1000, 50), 24);
		//w1animCombo3.AddEvent (new AnimationEventChangePriorityOverride (1000, 50), 24);
		//w1animCombo4.AddEvent (new AnimationEventChangePriorityOverride (1000, 50), 24);
		//w1animCombo5.AddEvent (new AnimationEventChangePriorityOverride (1000, 50), 40);
		w2animUse.AddEvent (new AnimationEventChangePriorityOverride (1000, 50), 12);
		w3animUse.AddEvent (new AnimationEventChangePriorityOverride (1000, 50), 22);

		// MECHA MOVEMENT STUFF

		/*
		 * float verticalMinSpeed = 20f, verticalMaxSpeed = 15f, boostVerticalMinSpeed = 10f,
				boostSideVerticalMinSpeed = 12f, boostBackVerticalMinSpeed = 6f;
			float speedWalk = 8f, speedBoost = 20f, speedBoostSide = 18f;
			float jumpSpeed = 30f, gravity = 45f;
			float energyJump = 20f, energyBoost = 30f, energyBoostSide = 25f;
			float speedBoostBack = 80f, energyBoostBack = 20f, boostBackTime = 0.4f;
		 */

		MechaMovement mmGravity = new MechaMovementGravity (null, 45f);
		MechaMovement mmJump = new MechaMovementJump (null, 30f);
		MechaMovement mmVertical = new MechaMovementVertical(mmJump, mmGravity, -20f, 15f);
		MechaMovement mmLock = new MechaMovement ();//new MechaMovementWalk(null, 0f);

		MechaMovement mmfBasic = new MechaMovementWalk (mmVertical, 8f);
		MechaMovement mmfBoostF = new MechaMovementBoost (null, 20f);
		MechaMovement mmfBoostS = new MechaMovementBoost (null, 18f);
		MechaMovement mmfBoostB = new MechaMovementBoost (null, 16f);
		u.SetBaseMechaMovement (mmfBasic);

		foreach (AnimationData d in anims[0])
			d.AddEvent (new AnimationEventChangeMechaMovement ());
		animSet.boostF.AddEvent (new AnimationEventChangeMechaMovement (mmfBoostF));
		animSet.boostL.AddEvent (new AnimationEventChangeMechaMovement (mmfBoostS));
		animSet.boostR.AddEvent (new AnimationEventChangeMechaMovement (mmfBoostS));
		animSet.boostB.AddEvent (new AnimationEventChangeMechaMovement (mmfBoostB));


		w1animDash.AddEvent(new AnimationEventComboSet(w1, 1));
		w1animDash.AddEvent (new AnimationEventMeleeDash (mmfBoostF, 2f, 30f), 1);
		// Melee stuff
		for (int i = 1; i < anims [4].Length; i++) {
			AnimationData ad = anims [4] [i];
			ad.AddEvent (new AnimationEventShootWeaponRepeat (null, 19), 7); 
			ad.AddEvent (new AnimationEventChangeMechaMovement (mmLock));
			ad.AddListener (new AnimationListenerMCA ());
			ad.AddEvent(new AnimationEventComboSet(w1, i+1));
		}

		// Boosting Stop
		AnimationListenerBoostStop listenBoostStop = new AnimationListenerBoostStop();
		listenBoostStop.AddVettedAnim (animSet.stand);
		listenBoostStop.AddVettedAnim (animSet.jump);

		animSet.boostF.AddListener (listenBoostStop);
		animSet.boostL.AddListener (listenBoostStop);
		animSet.boostR.AddListener (listenBoostStop);
		animSet.boostB.AddListener (listenBoostStop);

		return BuildUnit(u,w1,w2,w3,s1,s2);
	}

	// Basic stuff
	private static Unit List000(int unitID)
	{
		int seriesID = 0;
		int id = seriesID * 100 + unitID;

		/*Unit unit;
		UnitSkill skill1, skill2;
		UnitWeapon weapon1, weapon2, weapon3;*/

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

		//return BuildUnit (unit, weapon1, weapon2, weapon3, skill1, skill2);
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
