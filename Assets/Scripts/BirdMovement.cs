using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public Camera camera;
    private float rayDepth = 5f;
    public LineRenderer lineRend;
    public float lineWidth = 0.01f;

    public float diveCap = 4f;
    public float diveSpeed = 0.2f;
    public float diveVelocityCap = 10f;

    public float movementCap = 1f;
    public float speed = 5f;
    public float flapForce = 5f;
    private bool isOnGround = false;
    private Rigidbody2D rb2d;
    private bool isLookingRight = true;

    // Gets called at the start of the collision 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    // Gets called when the object exits the collision
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        drawLine(lineRend);
        // Get horizontal input (A/D or Left Arrow/Right Arrow)
        float moveHorizontal = 0;
        if (isOnGround)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            if (Input.GetKeyDown("space"))
            {
                StartFlight();
            }
            rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        }
        else
        {
            if (Input.GetKeyDown("space"))
            {
                Flap();
            }
            MoveToMouse();
        }
        handelBirdFlipping(moveHorizontal);


        // Set the velocity based on input
    }

    void flipBird()
    {
        Vector3 theScale = transform.localScale;
        Vector3 thePosition = transform.localPosition;
        theScale.x *= -1;
        if (isLookingRight)
        {
            thePosition.x -= 1;
        }
        else
        {
            thePosition.x += 1;
        }
        transform.localPosition = thePosition;
        transform.localScale = theScale;
        isLookingRight = !isLookingRight;
    }

    private Vector3 getMouseCameraPoint()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * rayDepth;
    }

    private void drawLine(LineRenderer lineRenderer)
    {
        Vector3 vector = getMouseCameraPoint();
        lineRenderer.SetPositions(new Vector3[] { GetBirdPosition(), vector });
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startWidth = lineWidth;
    }

    private void handelBirdFlipping(float moveHorizontal)
    {
        if (moveHorizontal > 0 && !isLookingRight)
        {
            flipBird();
        }
        else if (moveHorizontal < 0 && isLookingRight)
        {
            flipBird();
        }
    }

    private void StartFlight()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, flapForce);
    }

    private void Flap()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, flapForce);
        //rb2d.linearDrag = 0.4; 
    }

    private void MoveToMouse()
    {
        Vector3 vector = DeltaVector3(getMouseCameraPoint(), GetBirdPosition());
        float yVelocity = HandleDive(vector);
        vector = CapVector3(vector, movementCap);
        handelBirdFlipping(vector.x);
        rb2d.velocity = new Vector2(vector.x * speed, yVelocity);
    }

    private float HandleDive(Vector3 vec)
    {
        float diveVelocity = rb2d.velocity.y;
        bool isBirdDiving = vec.y < -diveCap;
        if (isBirdDiving)
        {
            diveVelocity = Mathf.Abs(diveVelocity) * -diveSpeed;
            diveVelocity = Mathf.Clamp(diveVelocity, -diveVelocityCap, diveVelocityCap);
            Debug.Log("Dive");
        }
        return diveVelocity;
    }
    private Vector3 DeltaVector3(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    private Vector3 CapVector3(Vector3 vec, float cap)
    {
        return new Vector3(Mathf.Clamp(vec.x, -cap, cap), Mathf.Clamp(vec.y, -cap, cap), Mathf.Clamp(vec.z, -cap, cap));
    }

    private Vector3 GetBirdPosition()
    {
        //body has to be the first child of the bird
        GameObject body = transform.GetChild(0).gameObject;
        return body.transform.position;
    }
}