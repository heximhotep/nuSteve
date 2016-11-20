using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {



	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;
	[SerializeField]
	private float thrusterForce = 1000f;

	[Header("Spring Settings:")]
	[SerializeField]
	private JointDriveMode jointMode = JointDriveMode.Position;
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;

	[Header("VR Settings")]
	[SerializeField]
	private bool vRModeEnabled = false;

	private ConfigurableJoint cJoint;
	private PlayerMotor motor;
	private GameObject localInstance;
	private bool localInstanceSet = false;

	void Start()
	{
		motor = GetComponent<PlayerMotor> ();
		cJoint = GetComponent<ConfigurableJoint> ();

		SetJointSettings (jointSpring);
	}

	void Update()
	{
		Vector3 _velocity = Vector3.zero;

		if (!vRModeEnabled) 
		{
			float _xMov = Input.GetAxisRaw ("Horizontal");
			float _zMov = Input.GetAxisRaw ("Vertical");

			Vector3 _movHorizontal = transform.right * _xMov;
			Vector3 _movVertical = transform.forward * _zMov;

			//final movement vector
			_velocity = (_movHorizontal + _movVertical).normalized * speed;

		

			//we only affect Y axis since looking up and down should only affect camera
			float _yRot = Input.GetAxisRaw ("Mouse X");

			Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * lookSensitivity;

			motor.Rotate (_rotation);

			float _xRot = Input.GetAxisRaw ("Mouse Y");

			float _cameraRotationX = _xRot * lookSensitivity;

			motor.RotateCamera (_cameraRotationX);

			Vector3 _thrusterForce = Vector3.zero;

			if (Input.GetButton ("Jump")) {
				_thrusterForce = Vector3.up * thrusterForce;
				SetJointSettings (0f);
			} else {
				SetJointSettings (jointSpring);
			}

			motor.ApplyThruster (_thrusterForce);
		}
		else 
		{
			float _zMov = Input.GetButton ("Fire1") ? 1 : 0;
			if (localInstanceSet)
			{
				Vector3 _movVertical = localInstance.transform.forward * _zMov * speed;
				_velocity = _movVertical;
			}
		}
		motor.Move (_velocity);
	}

	void SetJointSettings(float _jointSpring)
	{
		cJoint.yDrive = new JointDrive { mode = jointMode,
			positionSpring = _jointSpring,
			maximumForce = jointMaxForce
		};
	}

	public void SetLocalInstance(GameObject _localInstance)
	{
		localInstance = _localInstance;
		localInstanceSet = true;
	}
}
