﻿using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace AdaptingGravity.Physics.Gravity
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityHandler : MonoBehaviour
    {
        public delegate void GravityEventDelegate(Vector3 gravityDirection);
        public event GravityEventDelegate GravityChanged;
        public bool OnGround { get; private set; }
        public Vector3 GroundNormal { get; private set; }
        public string[] attractingObjectTags = new string[] {"Ground"};
        public float gravityStrength = 15f;
        public float gravityCheckDistance = 5f;
        public float groundCheckDistance = 0.1f;
        public GameObject GravityHandles;
        public GameObject GravityHandlePrefab;
        public List<GravityHandle> handles;

        private Vector3 gravityDirection = Vector3.down;
        public float groundDistance { get; private set; }

        private new Rigidbody rigidbody;
        
    
    
        void Awake ()
        {
            groundDistance = 0f;
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
            if (UnityEngine.Physics.Raycast(transform.position + (transform.up * 0.1f), gravityDirection, out hitInfo, gravityCheckDistance))
            {
                if(attractingObjectTags.Contains(hitInfo.transform.tag))
                {
                    if (GroundNormal != hitInfo.normal)
                    {
                        GroundNormal = hitInfo.normal;
                        gravityDirection = -GroundNormal;
                        if (GravityChanged != null)
                        {
                            GravityChanged(gravityDirection);
                        }
                    }
                    groundDistance = hitInfo.distance - 0.1f;
                    if (UnityEngine.Physics.Raycast(transform.position + (transform.up * 0.1f), gravityDirection, out hitInfo, groundCheckDistance + 0.1f))
                    {
                        if (attractingObjectTags.Contains(hitInfo.transform.tag))
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

        private void Reset()
        {
            if (GravityHandles == null)
            {
                if (transform.FindChild("GravityHandles") != null)
                {
                    GravityHandles = transform.FindChild("GravityHandles").gameObject;
                }
                else
                {
                    GravityHandles = new GameObject("GravityHandles");
                    GravityHandles.transform.SetParent(transform, false);
                }
            }
        }


    }
}
