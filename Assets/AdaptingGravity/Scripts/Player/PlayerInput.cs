// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerInput.cs" company="Johannes Deml">
//   Copyright (c) 2015 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

namespace AdaptingGravity.Player
{
    using UnityEngine;
    using System.Collections;

    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private PlayerController player;
        private Vector3 input;
        private float deltaRotation;
        private bool jumping;
        // Use this for initialization
        private void Awake()
        {
            if (player == null)
            {
                Debug.LogWarning("There is no player component assigned to player input.");
            }
            input = Vector3.zero;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumping = true;
            }

        }

        private void FixedUpdate()
        {
            input.x = Input.GetAxis("Horizontal");
            input.z = Input.GetAxis("Vertical");
            if (input.magnitude > 1)
            {
                input = input.normalized;
            }
            deltaRotation = Input.GetAxis("Mouse X");
            player.Move(input, deltaRotation, jumping);
            jumping = false;
        }

        private void Reset()
        {
            if (player == null)
            {
                player = GetComponent<PlayerController>();
            }
        }
    }
}
