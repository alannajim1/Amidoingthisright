using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1, 6)] public float fallMultiplier = 2.5f;
    [SerializeField][Range(1, 6)] public float lowJumpMultiplier = 3f;
    [SerializeField][Range(10, 30)] public float jumpStrength = 20f;
    private bool jumpPressed;
    private bool grounded;

    private Rigidbody rb;

    private PlayerInput input;
    UnityEngine.InputSystem.InputAction jumpAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        jumpAction = input.actions["Jump"];
    }


    // Check if the player is on the ground - in Layers field, give all objects the
    // tag "Ground" that you want to use as a solid surface to jump from
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    // Check if the player is not grounded and in the air
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    void Update()
    {
        // Check for input
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }

        // check for long jump
        if (Input.GetButton("Jump"))
        {
            jumpPressed = true;
        }
        else
        {
            jumpPressed = false;
        }

    }

    // Using fixedUpdate for the Physics to move the player up in the air
    void FixedUpdate()
    {
        // increase gravity if falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        // increase gravity for low jumps
        else if (rb.velocity.y > 0 && !jumpPressed)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}
