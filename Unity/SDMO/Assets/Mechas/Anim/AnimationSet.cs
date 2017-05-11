using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Contains all the animations of a unit
 * 
 * Is called by MechaSub to make the game work
 * 
 * When adding an animation to the set, also add it to :
 *  - GetAllAnimations
 */
public class AnimationSet
{
	public Unit u;

	public AnimationData
		stand, boostF, boostL, boostR, boostB,
		jump, descend, transform,
		error, knockdown, hit,
		menuSelect, taunt;
	// Weapon switch and shooting are in their own classes

	public AnimationData currentAnim;

	public List<AnimationData> GetAllAnimations(bool includeUnitAnimations = true)
	{
		List<AnimationData> l = new List<AnimationData>();

		l.Add(stand); l.Add(boostF); l.Add(boostL); l.Add(boostR); l.Add(boostB);
		l.Add(jump); l.Add(descend); l.Add(transform);
		l.Add(error); l.Add(knockdown); l.Add(hit);
		l.Add (menuSelect); l.Add (taunt);

		if (includeUnitAnimations) {
			l.AddRange (GetAllWeaponAnimations ());
		}

		return l;
	}

	public List<AnimationData> GetAllWeaponAnimations()
	{
		List<AnimationData> l = new List<AnimationData> ();
		if (u == null)
			return l;
		l.AddRange(u.GetWeapon1 ().GetAnimations ());
		l.AddRange (u.GetWeapon2 ().GetAnimations ());
		l.AddRange (u.GetWeapon3 ().GetAnimations ());
		return l;
	}

	public List<AnimationData> GetWeaponSwitchAnimations()
	{
		List<AnimationData> l = new List<AnimationData> ();
		if (u == null)
			return l;
		l.AddRange (u.GetWeapon1 ().GetAnimationsSwitch ());
		l.AddRange (u.GetWeapon2 ().GetAnimationsSwitch ());
		l.AddRange (u.GetWeapon3 ().GetAnimationsSwitch ());
		return l;
	}

	public List<AnimationData> GetWeaponUseAnimations()
	{
		List<AnimationData> l = new List<AnimationData> ();
		if (u == null)
			return l;
		l.AddRange (u.GetWeapon1 ().GetAnimationsUse ());
		l.AddRange (u.GetWeapon2 ().GetAnimationsUse ());
		l.AddRange (u.GetWeapon3 ().GetAnimationsUse ());
		return l;
	}

	public AnimationSet(){
		stand = AnimationData.GetAnimationDataError ("Unset Anims");
		currentAnim = stand;
	}
	public AnimationSet(Unit u) {
		this.u = u;
		stand = AnimationData.GetAnimationDataError ("Unset Anims");
		currentAnim = stand;
	}

	// Tries to play an animation
	// Returns whether or not the animation has started
	// If said animation is already playing, returns true but doesn't reset it
	public bool PlayAnim(AnimationData d, Mecha m = null)
	{
		if (currentAnim == d)
			return true;

		if (currentAnim.CanBeCancelled(m, this, d)) {
			ForceAnim (d, m);
			return true;
		}
		return false;
	}

	// Plays said animation imediately, regardless of the previous one
	public void ForceAnim(AnimationData d, Mecha m = null)
	{
		Debug.Log ("PLAYING : " + d.animNameUser);
		d.ResetAnim (m, this);
		currentAnim = d;
	}

	// Runs all the events of an animation. Doesn't disturb the current one
	public void FastForwardAnim(AnimationData d, Mecha m)
	{
		d.ResetAnim (m, this);
		for (int i = 0; i < d.nbFrames; i++) {
			d.FramePass (m, this, i);
		}
	}

	// Makes the current animtion progress
	public void ProgressAnim(Mecha m, float time)
	{
		currentAnim.AnimProgress (m, this, time);
	}
}
