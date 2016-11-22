using UnityEngine;
using System.Collections;

public class PersistenceScript : MonoBehaviour {

	[SerializeField]
	private GameObject[] PersistentObjects;

	void Awake()
	{
		foreach (GameObject obj in PersistentObjects) {
			DontDestroyOnLoad (obj);
		}
	}

	public void MakePersistent(GameObject newObj)
	{
		DontDestroyOnLoad (newObj);
	}
}
