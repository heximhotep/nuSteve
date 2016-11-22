using UnityEngine;
using UnityEngine.SceneManagement;
using AssemblyCSharp;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public MatchSettings matchSettings;

	[SerializeField]
	private LevelManager[] levels;

	[SerializeField]
	private GameObject gvrView;

	private GameObject rigInstance;

	private const string PLAYER_ID_PREFIX = "Player ";

	private static Dictionary<string, Player> players = new Dictionary<string, Player>();

	void Awake()
	{
		if (instance != null)
			Debug.LogError ("More than one Gamemanager instance in scene.");
		else
			instance = this;
	}

	public static GameObject GetRig()
	{
		if (instance.rigInstance == null) {
			Debug.LogError ("Tried to get rig before a rig has been registered");
			return null;
		} else {
			return instance.rigInstance;
		}
	}

	public static void RegisterRig(GameObject _rig)
	{
		instance.rigInstance = _rig;
	}

	public static void RegisterPlayer(string _netID, Player _player)
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add (_playerID, _player);
		_player.transform.name = _playerID;
	}

	public static void UnRegisterPlayer(string _playerID)
	{
		players.Remove (_playerID);
	}

	public static Player GetPlayer (string _playerID)
	{
		return players [_playerID];
	}

	public static void EnableVR(Camera mainCam)
	{
		instance.gvrView.SetActive (true);
	}

	public static void ChangeLevel(int levelIndex)
	{
		LevelManager newLevel = instance.levels[levelIndex];
		newLevel.gameObject.SetActive (true);
		newLevel.Initialize ();
		int playerIndex = 0;
		foreach (KeyValuePair<string, Player> entry in players) 
		{
			newLevel.Spawn (entry.Value.gameObject, playerIndex);
			playerIndex++;
		}
	}

//	void OnGUI()
//	{
//		GUILayout.BeginArea (new Rect (200, 200, 200, 500));
//
//		GUILayout.BeginVertical ();
//
//		foreach (string _playerID in players.Keys) 
//		{
//			GUILayout.Label (_playerID + "  -  " + players [_playerID].transform.name);
//		}
//
//		GUILayout.EndVertical ();
//
//		GUILayout.EndArea ();
//	}
}
