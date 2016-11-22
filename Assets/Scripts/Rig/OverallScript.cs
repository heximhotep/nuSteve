using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class OverallScript : MonoBehaviour {
	public JointType TrackedJoint;

	[SerializeField]
	private float heightOffset = 0;
	[SerializeField]
	public float multiplier = 10f;


    private GameObject leftHand, rightHand, head, BodySrcManager;
    private BodySourceManager bodyManager;
    private Body[] bodies;
	private Vector3 LHandPos, RHandPos, HeadPos, MShoulderPos, LShoulderPos, RShoulderPos;
    private HandState previousHandState;
    private Vector3 previousHandPosition;
    // Use this for initialization
    public void Setup() {
        leftHand = transform.Find("LeftHand").gameObject;
        rightHand = transform.Find("RightHand").gameObject;
        head = transform.Find("Head").gameObject;
		BodySrcManager = transform.Find ("BodySourceManager").gameObject;

		LHandPos = new Vector3 ();
		RHandPos = new Vector3 ();
		HeadPos  = new Vector3 ();
		LShoulderPos = new Vector3 ();
		RShoulderPos = new Vector3 ();
		MShoulderPos = new Vector3 ();

        previousHandState = HandState.NotTracked;
        previousHandPosition = Vector3.zero;

        if (BodySrcManager == null)
        {
            Debug.Log("Assign Game Object with Body Source Manager");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
    }

	public Vector3 GetLHandPos()
	{
		return LHandPos;
	}

	public Vector3 GetRHandPos()
	{
		return RHandPos;
	}

	public Vector3 GetHeadPos()
	{
		return HeadPos;
	}

	Vector3 getJointPos(Body body,JointType thisJoint)
	{
		CameraSpacePoint _thisPoint = body.Joints [thisJoint].Position;
		Vector3 _result = new Vector3 (-_thisPoint.X, _thisPoint.Y + heightOffset, _thisPoint.Z) * multiplier;
		return _result + transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        if (bodyManager == null)
        {
            return;
        }

        bodies = bodyManager.GetData();
        if (bodies == null)
        {
            return;
        }

        foreach (var body in bodies)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
				LHandPos = getJointPos (body, JointType.HandLeft);
				RHandPos = getJointPos (body, JointType.HandRight);
				HeadPos = getJointPos (body, JointType.Head);
				LShoulderPos = getJointPos (body, JointType.ShoulderLeft);
				RShoulderPos = getJointPos (body, JointType.ShoulderRight);
				MShoulderPos = getJointPos (body, JointType.SpineShoulder);
				Vector3 _camForward = Vector3.Cross (LShoulderPos - RShoulderPos, HeadPos - MShoulderPos).normalized;

				if (body.HandLeftState != HandState.Lasso && body.HandRightState != HandState.Lasso) 
				{
					leftHand.GetComponent<Rigidbody> ().MovePosition (LHandPos);
					rightHand.GetComponent<Rigidbody> ().MovePosition (RHandPos);
					head.GetComponent<Rigidbody> ().MovePosition (HeadPos);
					head.transform.forward = _camForward;
				}
            }
        }
    }
}
