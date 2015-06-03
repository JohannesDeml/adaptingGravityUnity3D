using UnityEngine;

namespace Deml.Physics.Gravity
{
    [RequireComponent(typeof(Rigidbody))]
    public class AdaptingGravity : MonoBehaviour {
        public bool OnGround { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        [SerializeField]
        private float gravityStrength = 15f;
        [SerializeField]
        private float gravityCheckDistance = 5f;
        [SerializeField]
        private float groundCheckDistance = 0.05f;
    
        private Vector3 gravityDirection = Vector3.down;
        private float groundDistance = 0f;
        private new Rigidbody rigidbody;
    
    
        void Awake () {
            rigidbody = GetComponent<Rigidbody>();
            GroundNormal = Vector3.up;
        }
	
        // Update is called once per frame
        void FixedUpdate () {
            CheckGroundStatus();
            if (Mathf.Abs(gravityStrength) > 0.02f)
            {
                rigidbody.AddForce(gravityDirection * gravityStrength, ForceMode.Force);
            }
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (gravityDirection * gravityCheckDistance), Color.green);
#endif
            OnGround = false;
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (UnityEngine.Physics.Raycast(transform.position + (Vector3.up * 0.1f), gravityDirection, out hitInfo, gravityCheckDistance))
            {
                if(hitInfo.transform.tag == "Ground")
                {
                    GroundNormal = hitInfo.normal;
                    gravityDirection = -GroundNormal;
                    groundDistance = hitInfo.distance - 0.1f;
                    if (UnityEngine.Physics.Raycast(transform.position + (Vector3.up * 0.1f), gravityDirection, out hitInfo, groundCheckDistance + 0.1f))
                    {
                        if (hitInfo.transform.tag == "Ground")
                        {
                            OnGround = true;
                        }
                    }
                }
            }
        }

        public void ChangeGravityStrength(float newGravity)
        {
            gravityStrength = newGravity;
        }
    }
}
