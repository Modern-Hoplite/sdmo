﻿using System.Collections;
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

		if (includeUnitAnimations && u != null) {
			l.AddRange(u.GetWeapon1 ().GetAnimations ());
			l.AddRange (u.GetWeapon2 ().GetAnimations ());
			l.AddRange (u.GetWeapon3 ().GetAnimations ());
		}

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
	public bool PlayAnim(AnimationData d, Mecha m = null)
	{
		if (currentAnim.CanBeCancelled(d)) {
			ForceAnim (d, m);
			return true;
		}
		return false;
	}

	// Plays said animation imediately, regardless of the previous one
	public void ForceAnim(AnimationData d, Mecha m = null)
	{
		d.ResetAnim (m, this);
		currentAnim = d;
	}

	// Makes the current animtion progress
	public void ProgressAnim(Mecha m, float time)
	{
		currentAnim.AnimProgress (m, this, time);
	}
}
