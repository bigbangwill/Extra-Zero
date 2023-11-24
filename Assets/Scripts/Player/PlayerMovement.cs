using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;

    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        Vector2 movementVector = MovementManager.Instance.MovementInput() * movementSpeed;
        rb.velocity = movementVector * Time.fixedDeltaTime;
    }


}