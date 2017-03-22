using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is used for the time between dying and respawning.
 * 
 */
public class RespawnCam : MonoBehaviour
{
	public float respawnTime = 3f;

	public void Update()
	{
		respawnTime -= Time.deltaTime;

		transform.position = ManagerLevel.i.spectatorCamPoint.position;
		transform.rotation = ManagerLevel.i.spectatorCamPoint.rotation;

		if (Input.GetKeyDown (KeyCode.Space) && respawnTime <= 0f) {
			ManagerLocal.i.SpawnMe ();
			Destroy (gameObject);
		}
	}

	public void OnGUI()
	{
		if (respawnTime > 0f) {
			GUI.Box (new Rect(Screen.width/2f - 75f, Screen.height/2f - 75f, 150f, 150f), "" +respawnTime);
		} else {
			GUI.Box (new Rect(Screen.width/2 - 100, Screen.height-50, 200, 50), "Press space to respawn");
		}
	}
}
