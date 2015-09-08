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
    /// <summary>
    /// The gravity handle is used to check for ground objects inside their rays.
    /// Objects that use adapting gravity can and should have more than one gravity handle
    /// Gravity handles have a visual representation in the editor for easier tweaking
    /// </summary>
    public class GravityHandle : MonoBehaviour
    {
        public GravityHandler Parent;
        public bool IsActiveGravityDirection = false; // True if it is used to define the gravity direction of the object (Also if used for weighted average)
        public float GroundDistance { get; private set; } // Distance from the handle point to the next ground object face along the ray
        public Vector3 GroundNormal { get; private set; } // Normal of the face that is hit by the raycast
        public bool OnGround { get; private set; } // true if the hanlde point is touches a ground object

        void Awake()
        {
            GroundDistance = float.MaxValue;
            OnGround = false;
        }

        /// <summary>
        /// Shoots a raycast to find the nearest ground object face and stores the distance to taht face
        /// as well as the normal of the face. The raycast has a length that is defined by gravityCheckDistance
        /// which is stored in GravityHandler (<see cref="GravityHandler"/>).
        /// </summary>
        /// <returns>True if an object was hit</returns>
        public bool CalculateGravityDirection()
        {
            RaycastHit hitInfo;
            OnGround = false;
            GroundDistance = float.MaxValue;
            IsActiveGravityDirection = false;
            // An offeset of 0.1f is used to start the ray inside of the character
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

        /// <summary>
        /// Draws a sphere and a ray to show in the editor. The color of the spehere and ray is green
        /// if the ray is active, otherwise they are yellow
        /// For the functionality of the method see http://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html
        /// </summary>
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

        /// <summary>
        /// Draws a wireframe aroung the sphere if the game object is selected
        /// For the functionality of the method see http://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmosSelected.html
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.13f);
        }
    }
}
