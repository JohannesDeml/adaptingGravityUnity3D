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

    /// <summary>
    /// Handles the keyboard input and mouse movement, that is necessary for player movement 
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private PlayerController player; // The player, the input data is sent to
        private Vector3 input; // The keyboard input is stored in this Vector3 for easier handling 
        private float deltaRotation; // The rotation difference of the mouse from one frame to the next 
        private bool jumping; // A bool that stores if the jump key was pressed

        /// <summary>
        /// Checks if a PlayerController is set for the input
        /// </summary>
        private void Awake()
        {
            if (player == null)
            {
                Debug.LogWarning("There is no player component assigned to player input.");
            }
            input = Vector3.zero;
        }

        /// <summary>
        /// Catches any jump input. It is necessary to do this in the update in order to get every keystroke
        /// </summary>
        private void Update()
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumping = true;
            }

        }

        /// <summary>
        /// In the fixed update the input is processed normalized and sent to the player
        /// </summary>
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

        /// <summary>
        /// Checks if a player is set and tries to set a player if none is set.
        /// For information about the reset functionality see http://docs.unity3d.com/ScriptReference/MonoBehaviour.Reset.html
        /// </summary>
        private void Reset()
        {
            if (player == null)
            {
                player = GetComponent<PlayerController>();
            }
        }
    }
}
