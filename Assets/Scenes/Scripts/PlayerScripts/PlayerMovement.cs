using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        /* Old Input
        // handles input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        */

        Vector3 move;

        if (Input.GetKey(KeyCode.W))
        {
            //GetComponent<SpriteRenderer>().sprite = Up;
            move = new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //GetComponent<SpriteRenderer>().sprite = Left;
            move = new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //GetComponent<SpriteRenderer>().sprite = Right;
            move = new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //GetComponent<SpriteRenderer>().sprite = Down;
            move = new Vector3(0, 0, -moveSpeed * Time.deltaTime);
        }

        //transform.position += moveSpeed;

    }

    // Fixed update updates on a fixed timer (better for physics)
    private void FixedUpdate()
    {
        // handles movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
