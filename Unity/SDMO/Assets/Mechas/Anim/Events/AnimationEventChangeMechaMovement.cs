using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Changes the current Mecha Movement of the Mecha
 * If null, sets it back to the standard one
 */
public class AnimationEventChangeMechaMovement : AnimationEvent
{
	public MechaMovement mechaMov = null;

	public AnimationEventChangeMechaMovement()
	{}

	public AnimationEventChangeMechaMovement(MechaMovement mechaMov)
	{
		this.mechaMov = mechaMov;
	}

	public override void Activate (Mecha m, AnimationSet s, int curFrame)
	{
		if (mechaMov == null)
			m.mMovement = m.unit.GetBaseMechaMovement ();
		else
			m.mMovement = mechaMov;
	}
}
