using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 100;
    
    private Rigidbody2D rb;
    private Vector2 direction;

    public bool CanMove { get => canMove; set { canMove = value; } }
    private bool canMove = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
            return;

        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (direction.magnitude > 1) direction = direction.normalized;
    }

    private void FixedUpdate()
    {
        Vector2 change = Vector2.zero;
        if (canMove && direction.magnitude > 0)
        {
            var desiredMovement = direction * walkSpeed * Time.fixedDeltaTime;
            change += desiredMovement - rb.velocity;
        }

        rb.velocity += change;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        canMove = true;
    }
}
