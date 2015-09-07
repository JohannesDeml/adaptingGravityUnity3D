// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LockFallingOver.cs" company="Johannes Deml">
//   Copyright (c) 2015 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptingGravity.Physics.Manipulation
{
    using UnityEngine;
    using System.Collections;
    using AdaptingGravity.Math;
    using AdaptingGravity.Physics.Gravity;

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
        [SerializeField]
        private new Rigidbody rigidbody;
        [SerializeField]
        private GravityHandler gravityComponent;
        private Vector3 groundNormal;
        private Quaternion perfectAlignmentQuaternion;
        // Use this for initialization
        private void Start()
        {
            if (rigidbody == null)
            {
                Debug.LogWarning("There is no rigidbody assigned to lockfallingover");
            }
            if (gravityComponent == null)
            {
                Debug.LogWarning("There is no gravity component assigned to lockfallingover");
            }
            gravityComponent.GravityChanged += SetGravityDirection;
            
            //rotationLimitations = new bool[3];
            //rotationLimitations[0] = maxRotationX < 180f;
            //rotationLimitations[1] = maxRotationY < 180f;
            //rotationLimitations[2] = maxRotationZ < 180f;
            groundNormal = Vector3.up;
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
                float timeFactor = Mathf.Pow(Quaternion.Angle(rigidbody.rotation, perfectAlignmentQuaternion), 1/2f);
                transform.rotation = Quaternion.Lerp(rigidbody.rotation, perfectAlignmentQuaternion, timeFactor*Time.fixedDeltaTime);
            }
        }

        public void SetGravityDirection(Vector3 newGravityDirection)
        {
            groundNormal = -newGravityDirection;
        }

        private void Reset()
        {
            if (rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody>();
            }
            if (gravityComponent == null)
            {
                gravityComponent = GetComponent<GravityHandler>();
            }
        }
    }
}