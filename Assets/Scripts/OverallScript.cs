using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class OverallScript : MonoBehaviour {
    private GameObject leftHand, rightHand, head, BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;
    public float multiplier = 10f;
	private Vector3 LHandPos, RHandPos, HeadPos;

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
				CameraSpacePoint lhpoint = body.Joints[JointType.HandLeft].Position;
                CameraSpacePoint rhpoint = body.Joints[JointType.HandRight].Position;
                CameraSpacePoint hpoint = body.Joints[JointType.Head].Position;
			
				LHandPos = new Vector3 (lhpoint.X, lhpoint.Y, lhpoint.Z) * multiplier;
				RHandPos = new Vector3 (rhpoint.X, rhpoint.Y, rhpoint.Z) * multiplier;
				HeadPos =  new Vector3 ( hpoint.X,  hpoint.Y,  hpoint.Z) * multiplier;

				if (body.HandLeftState != HandState.Lasso && body.HandRightState != HandState.Lasso) {
					leftHand.GetComponent<Rigidbody> ().MovePosition (LHandPos);
					rightHand.GetComponent<Rigidbody> ().MovePosition (RHandPos);
					head.GetComponent<Rigidbody> ().MovePosition (HeadPos);
				}
            }
        }
    }
}
