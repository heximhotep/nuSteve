using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rig))]
public class RigSetup : NetworkBehaviour {

	[SerializeField]
	private Behaviour[] ComponentsToDisable;
	[SerializeField]
	private GameObject kinectBase;

	void Start () {
		if (!isLocalPlayer) {
			DisableComponents ();
		} else {
			OverallScript kinectControl = kinectBase.AddComponent<OverallScript> ();
			kinectControl.Setup ();
		}
			
		GameManager.RegisterRig (gameObject);
	}

	void DisableComponents()
	{
		foreach (Behaviour b in ComponentsToDisable) {
			b.enabled = false;
		}

		GetComponent<Rig> ().Setup ();
	}
}
