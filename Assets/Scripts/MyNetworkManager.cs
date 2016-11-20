using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkManager : NetworkManager {

	public Transform[] spawnPositions;
	public static int curPlayer = 0;

	public static void SetCurPlayer(int _curPlayer)
	{
		curPlayer = _curPlayer;
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		IntegerMessage msg = new IntegerMessage (curPlayer);

		ClientScene.AddPlayer (conn, 0, msg);

	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		if (extraMessageReader != null) {
			var stream = extraMessageReader.ReadMessage<IntegerMessage> ();
			curPlayer = stream.value;
		}

		var playerPrefab = spawnPrefabs [curPlayer];

		int spawnIndex = (int)(Random.Range (0, spawnPositions.Length - float.Epsilon));

		var player = Instantiate (playerPrefab, spawnPositions[spawnIndex].position, Quaternion.identity) as GameObject;
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		if (curPlayer != 0)
			SetCurPlayer (0);
	}
}
