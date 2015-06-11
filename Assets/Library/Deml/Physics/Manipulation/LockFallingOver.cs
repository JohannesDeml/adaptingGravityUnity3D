using UnityEngine;
using System.Collections;
using Deml.Math;
using Deml.Physics.Gravity;

namespace Deml.Physics.Manipulation
{
    [RequireComponent(typeof(Rigidbody))]
    public class LockFallingOver : MonoBehaviour
    {
        //[SerializeField]
        //[Range(0f, 180f)]
        //private float maxRotationX = 20f;
        //[SerializeField]
        //[Range(0f, 180f)]
        //private float maxRotationY = 180f;
        //[SerializeField]
        //[Range(0f, 180f)]
        //private float maxRotationZ = 20f;

        private bool[] rotationLimitations;
        private new Rigidbody rigidbody;
        private Vector3 groundNormal;
        private Vector3 lastGroundNormal;
        private Quaternion perfectAlignmentQuaternion;
        // Use this for initialization
        private void Start()
        {
            AdaptingGravity.OnGravityChangedEvent += SetGravityDirection;
            rigidbody = GetComponent<Rigidbody>();
            //rotationLimitations = new bool[3];
            //rotationLimitations[0] = maxRotationX < 180f;
            //rotationLimitations[1] = maxRotationY < 180f;
            //rotationLimitations[2] = maxRotationZ < 180f;
            groundNormal = Vector3.up;
            lastGroundNormal = groundNormal;
            perfectAlignmentQuaternion = transform.rotation;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            //if (groundNormal != lastGroundNormal)
            //{
            //    Quaternion rot = Quaternion.FromToRotation(lastGroundNormal, groundNormal);
            //    perfectAlignmentQuaternion = rot * perfectAlignmentQuaternion;
            //    lastGroundNormal = groundNormal;
            //}
            Quaternion rot = Quaternion.FromToRotation(transform.up, groundNormal);
            perfectAlignmentQuaternion = rot * transform.rotation;
            if (Quaternion.Angle(rigidbody.rotation, perfectAlignmentQuaternion) > 0.2f)
            {
                rigidbody.angularVelocity /= 2f;
                float timeFactor = Mathf.Pow(Quaternion.Angle(rigidbody.rotation, perfectAlignmentQuaternion), 1/5f);
                transform.rotation = Quaternion.Lerp(rigidbody.rotation, perfectAlignmentQuaternion, timeFactor);
            }
        }

        public void SetGravityDirection(Vector3 newGravityDirection)
        {
            groundNormal = -newGravityDirection;
        }
    }
}