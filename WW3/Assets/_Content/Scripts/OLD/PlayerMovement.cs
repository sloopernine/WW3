using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer(Vector3 movement)
    {
        movement = movement.normalized;

        if (movement.sqrMagnitude > 0.05f)
        {
            transform.up = movement;
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

            rb.velocity = transform.up * speed;
        }

        //We don't need to stop because we have linear drag that will stop our player.
    }
}