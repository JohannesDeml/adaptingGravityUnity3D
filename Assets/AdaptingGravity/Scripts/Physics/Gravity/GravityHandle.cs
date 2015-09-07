// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GravityHandle.cs" company="Johannes Deml">
//   Copyright (c) 2015 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptingGravity.Physics.Gravity
{
    using UnityEngine;
    using System.Collections;
    using System.Linq;

    public class GravityHandle : MonoBehaviour
    {
        public GravityHandler Parent;
        public bool IsActiveGravityDirection = false;
        public float GroundDistance { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        public bool OnGround { get; private set; }

        void Awake()
        {
            GroundDistance = float.MaxValue;
            OnGround = false;
        }

        public bool CalculateGravityDirection()
        {
            RaycastHit hitInfo;
            OnGround = false;
            GroundDistance = float.MaxValue;
            IsActiveGravityDirection = false;
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (UnityEngine.Physics.Raycast(transform.position + (-transform.forward * 0.1f), transform.forward, out hitInfo, Parent.gravityCheckDistance +0.1f))
            {
                if (Parent.attractingObjectTags.Contains(hitInfo.transform.tag))
                {
                    if (GroundNormal != hitInfo.normal)
                    {
                        GroundNormal = hitInfo.normal;
                    }
                    GroundDistance = hitInfo.distance - 0.1f;
                    if (GroundDistance <= Parent.groundCheckDistance)
                    {
                        OnGround = true;
                    }
                    return true;
                }
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            if (IsActiveGravityDirection)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }
            
            Gizmos.DrawSphere(transform.position, 0.07f);
            Gizmos.DrawRay(transform.position, transform.forward * Parent.gravityCheckDistance);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.13f);
        }
    }
}
