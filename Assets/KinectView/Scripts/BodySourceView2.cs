using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;


public class BodySourceView2 : MonoBehaviour
{
    // Start is called before the first frame update

    public BodySourceManager mBodySourceManager;
    public GameObject mJointObject;
    public float speedUp = 2f;
    public float speedUpY = 2f;
    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType> {
        JointType.HandLeft,
         JointType.HandRight,
    };
    public Vector3 targetPosition;
    public Vector3 targetPosition1;
    public static BodySourceView2 Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Body[] data = mBodySourceManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
                continue;
            if (body.IsTracked)
                trackedIds.Add(body.TrackingId);
        }

        //Delete

        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach(ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(mBodies[trackingId]);
                mBodies.Remove(trackingId);
            }
        }
        //Create Kinect bodies

        foreach(var body in data)
        {
            if (body == null)
                continue;

            if (body.IsTracked)
            {
                if (!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

                UpdateBodyObject(body, mBodies[body.TrackingId]);
            }
        }

    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body " + id);

        foreach(JointType joint in _joints)
        {
            GameObject newJoint = Instantiate(mJointObject);
            newJoint.name = joint.ToString();

            newJoint.transform.parent = body.transform;
        }

        return body;
    }
    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        /*
        foreach (JointType _joint in _joints)
        {
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z = 0;

            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            jointObject.position = targetPosition;


            Oscillator.Instance.gain = map(targetPosition.y, -2, 5, 0, 1);
        }*/

         
            Joint sourceJoint = body.Joints[JointType.HandLeft];
            targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.x *= speedUp;
            targetPosition.y *= speedUpY;
            targetPosition.z = 1;

            Transform jointObject = bodyObject.transform.Find(JointType.HandLeft.ToString());
            jointObject.position = targetPosition;

            //Oscillator.Instance.gain = map(targetPosition.y, -2, 5, 0, 1);

        Joint sourceJoint1 = body.Joints[JointType.HandRight];
        targetPosition1 = GetVector3FromJoint(sourceJoint1);
        targetPosition1.x *= speedUp;
        targetPosition1.y *= speedUpY;
        targetPosition1.z = 1;

        Transform jointObject1 = bodyObject.transform.Find(JointType.HandRight.ToString());
        jointObject1.position = targetPosition1;

        //Oscillator.Instance.frequency = map(targetPosition1.y, -2, 5, 400, 600);



    }
    private Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
    double map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

}
