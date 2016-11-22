using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	private Transform[] spawnPoints;

	void Awake()
	{
		Transform _spawnRoot = transform.Find ("_SpawnPoints");
		spawnPoints = new Transform[2];
		spawnPoints [0] = transform.Find ("Spawn 1");
		spawnPoints [1] = transform.Find ("Spawn 2");
	}

	public void Initialize()
	{
	}

	public void Spawn(GameObject playr, int index)
	{
		Transform thisSpawn = spawnPoints [index];
		playr.transform.position = thisSpawn.position;
		playr.transform.rotation = thisSpawn.rotation;
	}
}
