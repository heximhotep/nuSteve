using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {
	[SyncVar]
	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOndDeath;
	private bool[] wasEnabled;

	public void Setup()
	{
		wasEnabled = new bool[disableOndDeath.Length];
		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled [i] = disableOndDeath [i].enabled;
		}
		transform.parent = GameManager.GetRig ().transform;
		SetDefaults ();
	}

	public void SetDefaults()
	{
		isDead = false;

		currentHealth = maxHealth;

		for (int i = 0; i < disableOndDeath.Length; i++) {
			disableOndDeath [i].enabled = wasEnabled [i];
		}

		Collider _col = GetComponent<Collider> ();
		if (_col != null)
			_col.enabled = true;
	}

	[ClientRpc]
	public void RpcTakeDamage (int _amount)
	{
		currentHealth -= _amount;

		Debug.Log (transform.name + " now has " + currentHealth + " health remaining.");

		if (currentHealth <= 0) {
			Die ();
		}
	}

	private void Die()
	{
		isDead = true;

		for (int i = 0; i < disableOndDeath.Length; i++) {
			disableOndDeath [i].enabled = false;
		}

		Collider _col = GetComponent<Collider> ();
		if (_col != null)
			_col.enabled = false;
		//DISABLE COMPONENTS
		Debug.Log( transform.name + " DIED");

		//CALL RESPAWN METHOD
		StartCoroutine(Respawn());
	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);

		SetDefaults ();
		Transform _spawnPoint = MyNetworkManager.singleton.GetStartPosition ();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;

		Debug.Log (transform.name + " respawned.");
	}
}
