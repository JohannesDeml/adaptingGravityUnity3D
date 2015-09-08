// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Johannes Deml">
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
    using AdaptingGravity.Physics.Gravity;

    /// <summary>
    /// Is used to define the current states the player is in right now.
    /// The player can be in more than one state, that's why a normal enum would not work
    /// The states should only be checked and manipulated through the methods
    /// <code>
    /// EnableState(PlayerState stateToEnable)
    /// DisableState(PlayerState stateToDisable)
    /// HasState(PlayerState stateToEvaluate)
    /// </code>
    /// </summary>
    [System.Flags]
    public enum PlayerState
    {
        Initialized = 1 << 0, // Is true as soon as the Awake method finishes
        Movable = 1 << 1, // Is used to define whether or not the player can be moved
        Turnable = 1 << 2 // // Is used to define whether or not the player can be turned
    }
    /// <summary>
    /// Is used as a controller class that can be used with adapting gravity. It is dependet on
    /// a GravityHandler and also manipulates the rigidBopdy. 
    /// </summary>
    [RequireComponent(typeof (GravityHandler))]
    [RequireComponent(typeof (Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        [SerializeField] private float mouseSensitivity = 0.5f; // Regulates the sensitivity of the mouse movement.
        [SerializeField] private float movementSpeed = 8f; // Movement speed of the player defined in units per second
        public PlayerState State { get; private set; }
        private GravityHandler gravityController; // Has a connection to all gravity handles
        private new Rigidbody rigidbody;

        /// <summary>
        /// Get all neceassry components and activate all states
        /// </summary>
        private void Awake()
        {
            gravityController = GetComponent<GravityHandler>();
            rigidbody = GetComponent<Rigidbody>();
            State = PlayerState.Initialized;
            EnableState(PlayerState.Movable);
            EnableState(PlayerState.Turnable);
        }

        /// <summary>
        /// This method is called by PlayerInput (<see cref="PlayerInput"/>) and moves the player
        /// according to the input
        /// </summary>
        /// <param name="movementInput">A normalized vector3 that stores the movement direction in the object space</param>
        /// <param name="rotationInput">A value of how much the player should turn, it is modified with a factor of 1000 as well 
        /// as the mouse sensitivity</param>
        /// <param name="jumpingInput">A bool whether or not the player is trying to jump</param>
        public void Move(Vector3 movementInput, float rotationInput, bool jumpingInput)
        {

            if (!HasState(PlayerState.Initialized))
            {
                return;
            }

            if (HasState(PlayerState.Turnable))
            {
                UpdateRotation(rotationInput);
            }

            if (HasState(PlayerState.Movable))
            {
                UpdateMovement(movementInput, jumpingInput);
            }

        }

        /// <summary>
        /// Processes the movement it receives to move the player accordingly. The movement vecotr is mapped to the current roatation of the player.
        /// The Method also draws a red line in the movement direction to visualize the movement
        /// </summary>
        /// <param name="movementInput">A normalized vector3 that stores the movement direction in the object space</param>
        /// <param name="jumpingInput">A bool whether or not the player is trying to jump</param>
        private void UpdateMovement(Vector3 movementInput, bool jumpingInput)
        {
            Vector3 mappedMovement = movementInput.x*transform.right + movementInput.z*transform.forward;
            // The ground modifier is used to handle the movement on the ground and in the air differently
            float groundModifier = (gravityController.OnGround) ? 1.0f : 0.2f;

            mappedMovement = Vector3.ProjectOnPlane(mappedMovement, gravityController.GroundNormal);
            Debug.DrawLine(transform.position, transform.position + 3f*mappedMovement, Color.red);
            mappedMovement *= groundModifier*movementSpeed*Time.fixedDeltaTime;
            MoveRelative(mappedMovement);
        }

        /// <summary>
        /// Moves the rigidbody relative to its current position
        /// </summary>
        /// <param name="relativeChange">The relative difference of movement that should be added to the player position</param>
        private void MoveRelative(Vector3 relativeChange)
        {
            rigidbody.MovePosition(transform.position + relativeChange);
        }

        /// <summary>
        /// Rotates the player along the current ground normal
        /// </summary>
        /// <param name="rotationInput">A value of how much the player should turn, it is modified with a factor of 1000 as well 
        /// as the mouse sensitivity</param>
        private void UpdateRotation(float rotationInput)
        {
            Quaternion rotation = Quaternion.AngleAxis(rotationInput*1000f*mouseSensitivity*Time.fixedDeltaTime,
                gravityController.GroundNormal);
            transform.rotation = rotation*transform.rotation;
        }

        /// <summary>
        /// Enables a specific state without changing the values of all other states. 
        /// If the state is already enabled, there will be no change at all
        /// </summary>
        /// <param name="stateToEnable">The state that should be enabled</param>
        public void EnableState(PlayerState stateToEnable)
        {
            State |= stateToEnable;
        }

        /// <summary>
        /// Diables a specific state without changing the values of all other states. 
        /// If the state is already disabled, there will be no change at all
        /// </summary>
        /// <param name="stateToDisable"></param>
        public void DisableState(PlayerState stateToDisable)
        {
            State &= ~stateToDisable;
        }

        /// <summary>
        /// Returns whether a state is enabled or not
        /// </summary>
        /// <param name="stateToEvaluate">The state that should be checked</param>
        /// <returns>true if the state is enabled, false if the state is disabled</returns>
        public bool HasState(PlayerState stateToEvaluate)
        {
            return ((State & stateToEvaluate) == stateToEvaluate);
        }
    }
}
