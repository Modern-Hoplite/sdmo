using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* In game camera
 * 
 * Uses the reference to the local mecha in ManagerLocal
 */
public class MechaCam : MonoBehaviour
{
	public float viewX=0f, viewY=0f;
	public Vector3 offsetRelative, offsetGlobal;

	public void LateUpdate()
	{
		Mecha m = ManagerLocal.mecha;

		if (!m)
			return;

		viewX = (viewX + MechaInput.aim.x) % 360f;
		viewY = Mathf.Clamp (viewY + MechaInput.aim.y, -80f, 80f);

		Vector3 aimDir = GetAimDir(viewX, viewY);
		transform.LookAt (transform.position + aimDir);
		m.aimDirection = aimDir;

		Vector3 targetPos = m.transform.position;
		targetPos += offsetGlobal;
		targetPos += transform.TransformDirection (offsetRelative);

		transform.position = targetPos;

		RaycastHit hitInfo;
		if (Physics.Raycast (transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, 500f)) {
			m.aimPoint = hitInfo.point;
		} else
			m.aimPoint = transform.position + aimDir * 500f;
	}

	public static Vector3 GetAimDir(float x, float y)
	{
		Vector3 aimDir = Vector3.zero;

		aimDir += Vector3.right * Mathf.Cos (x * Mathf.Deg2Rad);
		aimDir += Vector3.forward * Mathf.Sin (x * Mathf.Deg2Rad);
		aimDir *= Mathf.Cos (y * Mathf.Deg2Rad);
		aimDir += Vector3.up * Mathf.Sin (y * Mathf.Deg2Rad);

		return aimDir;
	}

	public void OnGUI()
	{
		GUI.Box (new Rect (Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20), "+");
	}
}
