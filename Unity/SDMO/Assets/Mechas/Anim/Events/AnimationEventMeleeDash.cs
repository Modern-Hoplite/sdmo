using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventMeleeDash : AnimationEvent {
	public float slashRange, maxDashDistance;
	public MechaMovement movement;
	private Vector3 oldPosition = Vector3.zero;
	private float remainingDistance;

	public AnimationEventMeleeDash(MechaMovement boostMov, float attackRange, float boostDistanceMax)
	{
		movement = boostMov;
		slashRange = attackRange;
		maxDashDistance = boostDistanceMax;
	}

	// Activate is at the start of the dash
	public override void Activate (Mecha m, AnimationSet s, int curFrame)
	{
		Mecha a = Autolock (m);
		oldPosition = m.transform.position;
		remainingDistance = maxDashDistance;
		if (a != null) {
			// Dash
			m.mMovement = movement;
		} else {
			// Skip to next
			ActivateAttack(m, s);
		}
	}

	// Stops if is in range of the slash
	public override void AfterActivation (Mecha m, AnimationSet s, int curFrame)
	{
		Mecha a = Autolock (m);
		if (a != null) {
			float distance = (a.transform.position - m.transform.position).sqrMagnitude;
			if (distance <= slashRange * slashRange)
				ActivateAttack (m, s);
		}

		float distMoved = (m.transform.position - oldPosition).magnitude;
		remainingDistance -= distMoved;
		oldPosition = m.transform.position;
		if (remainingDistance <= 0f)
			ActivateAttack (m, s);
		
	}

	private Mecha Autolock(Mecha m)
	{
		return m.GetAutolock (m.aimDirection);
	}

	private void ActivateAttack(Mecha m, AnimationSet s)
	{
		m.currentWeapon.UseWeapon (m);
	}
}
