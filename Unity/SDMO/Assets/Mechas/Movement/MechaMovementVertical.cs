using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMovementVertical : MechaMovement
{
	public MechaMovement jumpPressed, jumpNotPressed;
	public float verticalMinSpeed, verticalMaxSpeed;

	public MechaMovementVertical (MechaMovement jump, MechaMovement nonJump, float verticalMinSpeed, float verticalMaxSpeed)
	{
		this.jumpPressed = jump;
		this.jumpNotPressed = nonJump;
		this.verticalMaxSpeed = verticalMaxSpeed;
		this.verticalMinSpeed = verticalMinSpeed;
	}

	public override Vector3 Movement (Mecha m, Vector3 mov)
	{
		if (MechaInput.jump)
			mov = jumpPressed.Movement (m, mov);
		else
			mov = jumpNotPressed.Movement (m, mov);

		return new Vector3 (mov.x, Mathf.Clamp(mov.y, verticalMinSpeed, verticalMaxSpeed), mov.z);
	}

	public override string ToString ()
	{
		return "Vertical[" + verticalMinSpeed + ", "+verticalMaxSpeed+"] ( " + jumpPressed.ToString() + " / "+jumpNotPressed.ToString()+" )";
	}
}
