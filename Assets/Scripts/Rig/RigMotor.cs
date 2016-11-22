using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RigMotor : MonoBehaviour {

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;

	private Rigidbody rb;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		PerformMovement ();
		PerformRotation ();
	}

	void PerformMovement()
	{
		if (velocity != Vector3.zero) {
			rb.MovePosition (transform.position + velocity * Time.fixedDeltaTime);
		}
	}

	void PerformRotation()
	{
		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation));
	}

	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
	}

	public void Rotate (Vector3 _rotation)
	{
		rotation = _rotation;
	}
}
