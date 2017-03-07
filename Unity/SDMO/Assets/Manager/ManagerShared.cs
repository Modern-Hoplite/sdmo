using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

/* The manager shared is created by the master client at the beginning of each map
 * 
 * It is used mainly to create objects on all clients that don't need a photon view
 * 
 */
[RequireComponent(typeof(PhotonView))]
public class ManagerShared : PunBehaviour
{
	public static ManagerShared i;

	public GameObject[] sfxPrefab;

	public List<ManagerSharedSFX> sfxAlive = new List<ManagerSharedSFX>();

	public void Awake()
	{
		i = this;
	}

	public void FixedUpdate()
	{
		List<ManagerSharedSFX> sfxToDelete = new List<ManagerSharedSFX>();
		foreach (ManagerSharedSFX s in sfxAlive) {
			s.timeToLive -= Time.fixedDeltaTime;
			if (s.timeToLive <= 0f) {
				sfxToDelete.Add (s);
				Destroy (s.transform.gameObject);
			}
		}

		foreach (ManagerSharedSFX s in sfxToDelete) {
			sfxAlive.Remove (s);
		}
	}

	public void CreateSFX(int sfxID, Vector3 position, Quaternion rotation)
	{
		object[] data = { sfxID, position, rotation };

		RPCCreateSFX (sfxID, position, rotation);
		photonView.RPC ("RPCCreateSFX", PhotonTargets.Others, data);
	}

	[PunRPC]
	public void RPCCreateSFX(int sfxID, Vector3 position, Quaternion rotation)
	{
		GameObject go = Instantiate (sfxPrefab[sfxID], position, rotation);
		ManagerSharedSFX s = new ManagerSharedSFX ();
		s.transform = go.transform;
		sfxAlive.Add (s);
	}
}

public class ManagerSharedSFX
{
	public float timeToLive = 1f;
	public Transform transform;
}