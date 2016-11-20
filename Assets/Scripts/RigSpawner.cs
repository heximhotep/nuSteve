using UnityEngine;
using UnityEngine.Networking;

public class RigSpawner : NetworkBehaviour {

	[SerializeField]
	private GameObject rigPrefab;


	public override void OnStartServer()
	{
		GameObject rig = (GameObject)Instantiate (rigPrefab, Vector3.zero, Quaternion.identity);
		NetworkServer.Spawn (rig);
	}
}
