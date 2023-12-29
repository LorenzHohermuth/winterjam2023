using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovementScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float decelerationSpeed = 5.0f;
    [SerializeField] private float increasedMassFactor = 3.0f;
    [SerializeField] private float speedChangeThreshold = 2.0f; 
    [SerializeField] private GameObject MainCamera;

    private Rigidbody2D rb;
    private float horizontalMovement = 0f;
    private float originalMass;
    private Vector2 previousVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMass = rb.mass;
    }

    void Update()
    {
        HandleMovementInput();

        HandleMassInput();
    }

    void FixedUpdate()
    {
        HandleHorizontalMovement();

        TrackSpeedChange();
    }

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            horizontalMovement = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            horizontalMovement = 1;
        }
        else
        {
            horizontalMovement = 0;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Jump();
        }
    }

    private void HandleMassInput()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.mass *= increasedMassFactor;
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            rb.mass = originalMass;
        }
    }

    private void HandleHorizontalMovement()
    {
        if (horizontalMovement != 0)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x != 0)
        {
            float deceleration = decelerationSpeed * Time.fixedDeltaTime * Mathf.Sign(rb.velocity.x);
            if (Mathf.Abs(deceleration) >= Mathf.Abs(rb.velocity.x))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x - deceleration, rb.velocity.y);
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void TrackSpeedChange()
    {
        Vector2 currentVelocity = rb.velocity;
        float speedDifference = (currentVelocity - previousVelocity).magnitude;

        if (speedDifference >= speedChangeThreshold)
        {
            Debug.Log("Significant speed change detected: " + speedDifference);
            CameraShake script = MainCamera.GetComponent<CameraShake>();
            if (script != null)
            {
                script.sShake(speedDifference/10);
            }
        }

        previousVelocity = currentVelocity;
    }
}