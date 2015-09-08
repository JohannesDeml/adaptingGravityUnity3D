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
    using AdaptingGravity.Physics.Gravity;

    public class LockFallingOver : MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody rigidbody;
        [SerializeField]
        private GravityHandler gravityComponent;
        private Vector3 groundNormal; // The normal is a refernce for the up vector the player should have
        private Quaternion perfectAlignmentQuaternion; // The rotation the player should have according to the groundNormal
        
        /// <summary>
        /// Check for all components and throw errors if they are not set
        /// </summary>
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
            
            groundNormal = Vector3.up;
            perfectAlignmentQuaternion = transform.rotation;
        }

        /// <summary>
        /// Calculates the perfect rotation quaternion and lerps the values of the current transformation and the perfect rotation if 
        /// their angle differs more than 0.2 degrees
        /// </summary>
        private void FixedUpdate()
        {
            Quaternion rot = Quaternion.FromToRotation(transform.up, groundNormal);
            perfectAlignmentQuaternion = rot * transform.rotation;
            if (Quaternion.Angle(rigidbody.rotation, perfectAlignmentQuaternion) > 0.2f)
            {
                rigidbody.angularVelocity /= 2f;
                float timeFactor = Mathf.Pow(Quaternion.Angle(rigidbody.rotation, perfectAlignmentQuaternion), 1/2f);
                transform.rotation = Quaternion.Lerp(rigidbody.rotation, perfectAlignmentQuaternion, timeFactor*Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Is called from the gravity changed event from the gravity handler (<see cref="GravityHandler"/>)
        /// </summary>
        /// <param name="newGravityDirection">The negative gravityDirection is used as the new ground normal</param>
        public void SetGravityDirection(Vector3 newGravityDirection)
        {
            groundNormal = -newGravityDirection;
        }

        /// <summary>
        /// Tries to set the rigidbody and gravity component if they are not set
        /// For more information about the functionality of reset take a look at http://docs.unity3d.com/ScriptReference/MonoBehaviour.Reset.html
        /// </summary>
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