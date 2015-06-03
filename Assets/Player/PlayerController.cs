using UnityEngine;
using System.Collections;
using Deml.Physics.Gravity;

[System.Flags]
public enum PlayerState
{
    Initialized = 1 << 0,
    Movable = 1 << 1,
    Turnable = 1 << 2
}

[RequireComponent(typeof(AdaptingGravity))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private float movementSpeed = 5f;
    public PlayerState State { get; private set; }
    private AdaptingGravity gravityController;
    private new Rigidbody rigidbody;
	// Use this for initialization
	void Awake ()
	{
	    gravityController = GetComponent<AdaptingGravity>();
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
        Debug.Log(movementInput);
        Vector3 mappedMovement = movementInput.x * transform.right + movementInput.z * transform.forward;
        float groundModifier = (gravityController.OnGround) ? 1.0f : 0.2f;

        mappedMovement = Vector3.ProjectOnPlane(mappedMovement, gravityController.GroundNormal);
        mappedMovement *= groundModifier*movementSpeed*Time.fixedDeltaTime;
        MoveRelative(mappedMovement);
    }

    private void MoveRelative(Vector3 relativeChange)
    {
        rigidbody.MovePosition(transform.position + relativeChange);
    }

    private void UpdateRotation(float angle)
    {
        transform.Rotate(transform.up, angle * 1000f * mouseSensitivity * Time.fixedDeltaTime);
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
