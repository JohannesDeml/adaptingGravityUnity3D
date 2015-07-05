using UnityEngine;
using System.Collections;
using System.Linq;

namespace AdaptingGravity.Physics.Gravity
{
    public class GravityHandle : MonoBehaviour
    {
        public GravityHandler Parent;
        public float GroundDistance { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        public bool OnGround { get; private set; }

        void Awake()
        {
            GroundDistance = float.MaxValue;
            OnGround = false;
        }

        public void CheckGroundStatus()
        {
            RaycastHit hitInfo;
            OnGround = false;
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
                    if (UnityEngine.Physics.Raycast(transform.position + (transform.up * 0.1f), transform.forward, out hitInfo, Parent.gravityCheckDistance + 0.1f))
                    {
                        if (Parent.attractingObjectTags.Contains(hitInfo.transform.tag))
                        {
                            OnGround = true;
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.05f);
            Gizmos.DrawRay(transform.position, transform.forward * Parent.gravityCheckDistance);
        }
    }
}
