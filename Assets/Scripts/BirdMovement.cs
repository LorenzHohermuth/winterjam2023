using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public Camera camera;
    public Material lineMaterial;
    private float rayDepth = 5f;
    public LinerRenderer lineRend;

    public float speed = 5f;
    public float flapForce = 100f;
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
        lineRend = drawLine();
    }


    void Update()
    {
        // Get horizontal input (A/D or Left Arrow/Right Arrow)
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (moveHorizontal > 0 && !isLookingRight)
        {
            flipBird();
        }
        else if (moveHorizontal < 0 && isLookingRight)
        {
            flipBird();
        }

        if (Input.GetMouseButtonDown(0))
        {
            drawLine();
        }

        // Set the velocity based on input
        rb2d.velocity = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
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
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        return ray.origin + ray.direction * rayDepth;
    }

    private void drawLine(LineRenderer lineRenderer)
    {
        Vector3 vector = getMouseCameraPoint();
        GameObject line = new GameObject();
        lineRenderer.SetPositions(new Vector3[] { transform.position, vector });
    }
}