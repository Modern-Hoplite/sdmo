using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Static structure containing all the input needed
 * 
 * Polled by Mecha
 * Pass the delta time to the Poll method
 * 
 * Pretty much hardcoded right now
 */
public class MechaInput
{
	public static Vector2 movement, aim;
	public static bool shoot;

	public static void Poll(float deltaTime)
	{
		float mouseSensX = -5f, mouseSensY = 3f;

		movement = Vector2.zero;
		if (Input.GetKey(KeyCode.D))
			movement += Vector2.right;
		if (Input.GetKey(KeyCode.A) || Input.GetKey (KeyCode.Q))
			movement -= Vector2.right;
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.Z))
			movement += Vector2.up;
		if (Input.GetKey (KeyCode.S))
			movement -= Vector2.up;

		aim = Vector2.zero;
		aim += Vector2.right * Input.GetAxis ("Mouse X") * mouseSensX;
		aim += Vector2.up * Input.GetAxis ("Mouse Y") * mouseSensY;

		shoot = Input.GetKeyDown (KeyCode.Mouse0);
	}
}
