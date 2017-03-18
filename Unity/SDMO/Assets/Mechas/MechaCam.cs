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

		if (!m) {
			return;
		}

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
		if (Physics.Raycast (m.transform.position + offsetGlobal, transform.TransformDirection(Vector3.forward), out hitInfo, 500f)) {
			m.aimPoint = hitInfo.point;
		} else
			m.aimPoint = transform.position + aimDir * 500f;

		//m.aimPoint = transform.position + aimDir * 500f;
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
		Mecha m = ManagerLocal.mecha;
		if (!m)
			return;

		// TEMPORARY HUD

		GUI.Box (new Rect (Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20), "+");

		float enBarMaxLength = Screen.width / 2f, enBarY = Screen.height * 4f / 5f, enBarHeight = 15f;
		float enBarActualLength = enBarMaxLength * (m.energy / m.energyMax);

		GUI.Box (new Rect ((Screen.width-enBarMaxLength)/2f - 5f, enBarY - 5f, 5f, enBarHeight+10f), "");
		GUI.Box (new Rect ((Screen.width+enBarMaxLength)/2f, enBarY - 5f, 5f, enBarHeight+10f), "");
		GUI.Box (new Rect ((Screen.width-enBarActualLength)/2f, enBarY, enBarActualLength, enBarHeight), "");

		float hpBarMaxLength = enBarMaxLength, hpBarY = enBarY + enBarHeight, hpBarHeight = 25f;
		float hpBarActualLength = hpBarMaxLength * (m.hp / m.hpMax);

		GUI.Box (new Rect ((Screen.width-hpBarMaxLength)/2f - 5f, hpBarY - 5f, 5f, hpBarHeight+10f), "");
		GUI.Box (new Rect ((Screen.width+hpBarMaxLength)/2f, hpBarY - 5f, 5f, hpBarHeight+10f), "");
		GUI.Box (new Rect ((Screen.width-hpBarActualLength)/2f, hpBarY, hpBarActualLength, hpBarHeight), Mathf.RoundToInt(m.hp)+"/"+Mathf.RoundToInt(m.hpMax));

		int activeWeapon = m.GetCurrentWeaponID();
		float weaponLength = Screen.width * 3f / 20f, weaponSelectLength = weaponLength *1.25f, weaponHeight = Screen.height / 10f;

		GUI.Box (new Rect (0f, Screen.height - weaponHeight * 3f, (activeWeapon == 1 ? weaponSelectLength : weaponLength), weaponHeight),
			(activeWeapon == 1 ? "> " : "") + m.unit.GetWeapon1 ().GetName ());
		GUI.Box (new Rect (0f, Screen.height - weaponHeight * 2f, (activeWeapon == 2 ? weaponSelectLength : weaponLength), weaponHeight),
			(activeWeapon == 2 ? "> " : "") + m.unit.GetWeapon2 ().GetName ());
		GUI.Box (new Rect (0f, Screen.height - weaponHeight * 1f, (activeWeapon == 3 ? weaponSelectLength : weaponLength), weaponHeight),
			(activeWeapon == 3 ? "> " : "") + m.unit.GetWeapon3 ().GetName ()); 

		float skillLength = weaponLength, skillHeight = weaponHeight;
		GUI.Box (new Rect (Screen.width - skillLength, Screen.height - skillHeight * 1f, skillLength, skillHeight), m.unit.GetSkill1 ().GetName());
		GUI.Box (new Rect (Screen.width - skillLength, Screen.height - skillHeight * 2f, skillLength, skillHeight), m.unit.GetSkill2 ().GetName());
		GUI.Box (new Rect (Screen.width - skillLength, Screen.height - skillHeight * 3f, skillLength, skillHeight), "Special Attack");

		float spBarWidth = weaponSelectLength - weaponLength, spBarHeight = skillHeight * 3f;
		float spBarActualHeight = spBarHeight * (m.sp / m.spMax);

		GUI.Box (new Rect(Screen.width - skillLength - spBarWidth, Screen.height - spBarActualHeight, spBarWidth, spBarActualHeight), "");
	}
}
