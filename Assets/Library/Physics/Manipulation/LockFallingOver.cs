using UnityEngine;
using System.Collections;
using Deml.Math;
using Deml.Physics.Gravity;

namespace Deml.Physics.Manipulation
{
    [RequireComponent(typeof(Rigidbody))]
    public class LockFallingOver : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 180f)]
        private float maxRotationX = 20f;
        [SerializeField]
        [Range(0f, 180f)]
        private float maxRotationY = 180f;
        [SerializeField]
        [Range(0f, 180f)]
        private float maxRotationZ = 20f;

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
            rotationLimitations = new bool[3];
            rotationLimitations[0] = maxRotationX < 180f;
            rotationLimitations[1] = maxRotationY < 180f;
            rotationLimitations[2] = maxRotationZ < 180f;
            groundNormal = Vector3.up;
            lastGroundNormal = groundNormal;
            perfectAlignmentQuaternion = rigidbody.rotation;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (groundNormal != lastGroundNormal)
            {
                Quaternion rot = Quaternion.FromToRotation(lastGroundNormal, groundNormal);
                perfectAlignmentQuaternion *= rot;
                lastGroundNormal = groundNormal;
            }
            Debug.Log(Vector3.Angle(transform.up, groundNormal));
            if (Quaternion.Angle(rigidbody.rotation, perfectAlignmentQuaternion) > 0.2f)
            {
                rigidbody.angularVelocity /= 2f;
                rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, perfectAlignmentQuaternion, Quaternion.Angle(rigidbody.rotation, perfectAlignmentQuaternion));
            }

            //Vector3 rotation = rigidbody.rotation.eulerAngles;
            //Vector3 angularVeloctiy = rigidbody.angularVelocity;
            //rotation = Vector3.Project(rotation, groundNormal);

            //Quaternion lastRot = rigidbody.rotation;
            //rigidbody.rotation = Quaternion.LookRotation(transform.forward, groundNormal);
            ////rigidbody.angularVelocity = Vector3.zero;
            //Debug.Log(lastRot == rigidbody.rotation);

            //rigidbody.angularVelocity = Vector3.zero;
            //if (rotationLimitations[0])
            //{
            //    rotation.x = rotation.x.ClampAngle(-maxRotationX, maxRotationX);
            //    rotation.x = Mathf.LerpAngle(rotation.x, 0, 0.3f);
            //}

            //if (rotationLimitations[1])
            //{
            //    rotation.y = rotation.y.ClampAngle(-maxRotationY, maxRotationY);
            //    rotation.y = Mathf.LerpAngle(rotation.y, 0, 0.3f);
            //}

            //if (rotationLimitations[2])
            //{
            //    rotation.z = rotation.z.ClampAngle(-maxRotationZ, maxRotationZ);
            //    rotation.z = Mathf.LerpAngle(rotation.z, 0, 0.3f);
            //}
            //rigidbody.rotation = Quaternion.Euler(rotation);
            //rigidbody.angularVelocity = angularVeloctiy;
        }

        public void SetGravityDirection(Vector3 newGravityDirection)
        {
            groundNormal = -newGravityDirection;
        }
    }
}