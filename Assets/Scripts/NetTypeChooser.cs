using UnityEngine;
using UnityEngine.Networking;

public class NetTypeChooser : MonoBehaviour {
	public void ToClient()
	{
		MyNetworkManager.singleton.StartClient ();
	}

	public void ToHost()
	{
		Camera sceneCamera = Camera.main;
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (false);
		}
		MyNetworkManager.SetCurPlayer (1);
		MyNetworkManager.singleton.StartHost ();
	}

	public void ToServer()
	{
		MyNetworkManager.singleton.StartServer();
	}
}
