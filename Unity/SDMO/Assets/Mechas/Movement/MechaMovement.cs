using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Pattern composite & strategy
 * 
 * 
 */
public class MechaMovement
{
	public MechaMovement content = null;

	public MechaMovement()
	{
	}

	public MechaMovement(MechaMovement c)
	{
		content = c;
	}

	public virtual Vector3 Movement(Mecha m, Vector3 mov)
	{
		if(content != null)
			mov = content.Movement (m, mov);

		return mov;
	}
}
