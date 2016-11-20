using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	public PlayerWeapon weapon;

	public Camera cam;

	[SerializeField]
	private LayerMask shootMask;

	void Start()
	{
		if (cam == null) {
			Debug.LogError ("PlayerShoot: No camera referenced");
		}
	}

	void Update()
	{
		if (Input.GetButtonDown ("Fire1")) {
			Shoot ();
		}
	}

	[Client]
	void Shoot()
	{
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, shootMask)) 
		{
			if (_hit.collider.CompareTag ("Player")) {
				Debug.Log ("Hit " + _hit.collider.gameObject.name);
				CmdPlayerShot (_hit.collider.gameObject.name, weapon.damage);
			}
		}
	}

	[Command]
	void CmdPlayerShot (string _playerID, int damage)
	{
		Debug.Log (_playerID + " has been shot");

		Player _player = GameManager.GetPlayer (_playerID);
		_player.RpcTakeDamage (damage);
	}
}
