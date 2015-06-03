using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    private PlayerController player;
    private Vector3 input;
    private float deltaRotation;
    private bool jumping;
	// Use this for initialization
	void Awake ()
	{
	    player = GetComponent<PlayerController>();
        input = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetButtonDown("Jump"))
	    {
            jumping = true;
        }
        
	}

    void FixedUpdate()
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
}
