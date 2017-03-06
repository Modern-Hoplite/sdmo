using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

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

		photonView.RPC ("RPCCreateSFX", PhotonTargets.All, data);
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