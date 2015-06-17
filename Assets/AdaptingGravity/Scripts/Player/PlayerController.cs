using UnityEngine;
using System.Collections;
using AdaptingGravity.Physics.Gravity;

namespace AdaptingGravity.Player
{
    [System.Flags]
    public enum PlayerState
    {
        Initialized = 1 << 0,
        Movable = 1 << 1,
        Turnable = 1 << 2
    }

    [RequireComponent(typeof (GravityHandler))]
    [RequireComponent(typeof (Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 0.5f;
        [SerializeField] private float movementSpeed = 5f;
        public PlayerState State { get; private set; }
        private GravityHandler gravityController;
        private new Rigidbody rigidbody;
        // Use this for initialization
        private void Awake()
        {
            gravityController = GetComponent<GravityHandler>();
            rigidbody = GetComponent<Rigidbody>();
            State = PlayerState.Initialized;
            EnableState(PlayerState.Movable);
            EnableState(PlayerState.Turnable);
        }

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

        private void UpdateMovement(Vector3 movementInput, bool jumping)
        {
            Vector3 mappedMovement = movementInput.x*transform.right + movementInput.z*transform.forward;
            float groundModifier = (gravityController.OnGround) ? 1.0f : 0.2f;

            mappedMovement = Vector3.ProjectOnPlane(mappedMovement, gravityController.GroundNormal);
            Debug.DrawLine(transform.position, transform.position + 3f*mappedMovement, Color.red);
            mappedMovement *= groundModifier*movementSpeed*Time.fixedDeltaTime;
            MoveRelative(mappedMovement);
        }

        private void MoveRelative(Vector3 relativeChange)
        {
            rigidbody.MovePosition(transform.position + relativeChange);
        }

        private void UpdateRotation(float angle)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle*1000f*mouseSensitivity*Time.fixedDeltaTime,
                gravityController.GroundNormal);
            transform.rotation = rotation*transform.rotation;
        }


        public void DisableState(PlayerState stateToDisable)
        {
            State &= ~stateToDisable;
        }

        public void EnableState(PlayerState stateToEnable)
        {
            State |= stateToEnable;
        }

        public bool HasState(PlayerState stateToCheck)
        {
            return ((State & stateToCheck) == stateToCheck);
        }
    }
}
