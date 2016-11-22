using UnityEngine;
using System.Collections;

public class RigController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float turnSpeed = 3f;

	private RigMotor motor;

	// Use this for initialization
	void Start () {
		motor = GetComponent<RigMotor> ();
	}
	
	// Update is called once per frame
	void Update () {
		float _yRot = Input.GetAxisRaw ("Horizontal");
		float _zMov = Input.GetAxisRaw ("Vertical");

		Vector3 _velocity = transform.forward * _zMov * speed;

		Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * turnSpeed;

		motor.Rotate (_rotation);
		motor.Move (_velocity);
	}
}
