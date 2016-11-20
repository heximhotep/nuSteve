using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class KinectMessenger : NetworkBehaviour {
	
	private OverallScript KinectOnServer;

	[SyncVar]
	private SyncListFloat headPos, lHandPos, rHandPos;

	void Start()
	{
		headPos = new SyncListFloat ();
		lHandPos = new SyncListFloat ();
		rHandPos = new SyncListFloat ();
	}

	void OnServerStart()
	{
		KinectOnServer = transform.parent.Find ("KinectObjects").gameObject.GetComponent<OverallScript> ();
	}

	void Update()
	{
		if (isServer) {
			Vector3 _headPos = KinectOnServer.GetHeadPos();
			Vector3 _lHandPos = KinectOnServer.GetLHandPos();
			Vector3 _rHandPos = KinectOnServer.GetRHandPos();

			headPos [0] = _headPos.x;
			headPos [1] = _headPos.y;
			headPos [2] = _headPos.z;

			lHandPos [0] = _lHandPos.x;
			lHandPos [1] = _lHandPos.y;
			lHandPos [2] = _lHandPos.z;

			rHandPos [0] = _rHandPos.x;
			rHandPos [1] = _rHandPos.y;
			rHandPos [2] = _rHandPos.z;
		}
	}

	public Vector3 GetHeadPos()
	{
		return new Vector3 (headPos [0], headPos [1], headPos [2]);
	}

	public Vector3 GetLHandPos()
	{
		return new Vector3 (lHandPos [0], lHandPos [1], lHandPos [2]);
	}

	public Vector3 GetRHandPos()
	{
		return new Vector3 (rHandPos [0], rHandPos [1], rHandPos [2]);
	}
}
