using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	GameObject localInstancePrefab;

	[SerializeField]
	string RemoteLayerName = "Remote Player Layer";

	Camera sceneCamera;

	void Start()
	{
		if (!isLocalPlayer) {
			DisableComponents ();
			AssignRemoteLayer ();
		} else {
			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive (false);
			}
			SetupLocalInstance ();
		}

		GetComponent<Player> ().Setup ();
		transform.parent = GameManager.GetRig ().transform.Find("KinectObjects").Find("Head").Find("ClientCameraOffset");
		transform.localPosition = Vector3.zero;
	}

	public override void OnStartClient()
	{
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString ();;
		Player _player = GetComponent<Player> ();

		GameManager.RegisterPlayer (_netID, _player);
	}

	void SetupLocalInstance()
	{
		GameObject localInstance = (GameObject)Instantiate (localInstancePrefab);
		localInstance.transform.SetParent (transform);
		localInstance.transform.localPosition = Vector3.zero;
		gameObject.GetComponent<PlayerShoot> ().cam = localInstance.GetComponent<Camera> ();
		GameManager.EnableVR (localInstance.GetComponent<Camera> ());
	}

	void AssignRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer (RemoteLayerName);
	}

	void DisableComponents()
	{
		for (int i = 0; i < componentsToDisable.Length; i++) 
		{
			componentsToDisable [i].enabled = false;
		}
	}

	void OnDisable()
	{
		if (sceneCamera != null) 
		{
			sceneCamera.gameObject.SetActive (true);
		}

		GameManager.UnRegisterPlayer (transform.name);
	}

}
