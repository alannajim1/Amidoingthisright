using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1, 6)] public float fallMultiplier = 2.5f;
    [SerializeField][Range(1, 6)] public float lowJumpMultiplier = 3f;
    [SerializeField][Range(10, 30)] public float jumpStrength = 20f;
    private bool jumpPressed;
    private bool grounded;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode2D.Impulse);
        }

        // check for long jump
        if (Input.GetKey(KeyCode.W))
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
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        // increase gravity for low jumps
        else if (rb.velocity.y > 0 && !jumpPressed)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        if (grounded)
        {
            Vector2 v = rb.velocity;
            v.y = 0;
            rb.velocity = v;
        }
    }
}
