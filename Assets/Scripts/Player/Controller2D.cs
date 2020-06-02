using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour
{
    public BoxCollider2D boxCollider;

    public LayerMask collisionMask;

    public float speed;
    public float timeToAcceleration;
    public float startTimeAcceleration;
    public float factorDecelerate;
    public float factorChangeDirection;

    public float jumpForce;

    public float maxWallJump;
    private float leftWalljump;

    private float currentTimeToAcceleration = 0;

    private bool moveToRight = false;
    private bool moveToLeft = false;


    void OnMoveRight(InputValue value)
    {
        moveToRight = !moveToRight;
        // currentTimeToAcceleration = startTimeAcceleration;
    }

    void OnMoveLeft(InputValue value)
    {
        moveToLeft = !moveToLeft;
        //  currentTimeToAcceleration = -startTimeAcceleration;
    }

    bool Grounded()
    {
        Ray ray;
        RaycastHit2D hit;

        Vector2 size = boxCollider.size;
        Vector2 center = boxCollider.offset;

        for (int i = 0; i < 3; i++)
        {
            float x = (transform.position.x + center.x - size.x / 2.0f) + size.x / 2.0f * i; //left, center and then rightmost point of collider
            float y = (transform.position.y + center.y + size.y / 2.0f * (-1)) + 0.5f; //bottom of collider

            ray = new Ray(new Vector2(x, y), new Vector2(0, -1));

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 5.0f);

            if (hit = Physics2D.Raycast(ray.origin, ray.direction, 1.0f, collisionMask) )
            {
                return true;
            }
        }

        return false;
    }

    bool Walled()
    {
        Ray ray;
        RaycastHit hit;

        Vector2 size = boxCollider.size;
        Vector2 center = boxCollider.offset;

        for (int i = 0; i < 3; i++)
        {
            float x = gameObject.transform.position.x + center.x - (size.x / 2.0f); //bottom left collider
            float y = gameObject.transform.position.y + center.y - (size.y / 2.0f) + (size.y / 2.0f) * i; //bottom middle top collider

            ray = new Ray(new Vector2(x, y), new Vector2(-1, 0));

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 5.0f);

            if (Physics2D.Raycast(ray.origin, ray.direction, 1.0f, collisionMask))
            {
                return true;
            }

            x = gameObject.transform.position.x + center.x + (size.x / 2.0f); //bottom left collider
            y = gameObject.transform.position.y + center.y - (size.y / 2.0f) + (size.y / 2.0f) * i; //bottom middle top collider

            ray = new Ray(new Vector2(x, y), new Vector2(1, 0));

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 5.0f);

            if (Physics2D.Raycast(ray.origin, ray.direction, 1.0f, collisionMask) )
            {
                return true;
            }
        }

        return false;
    }

    void OnJump(InputValue value)
    {
        if (Grounded())
        {
            leftWalljump = maxWallJump;

            Jump();

        }
        else
        {
            if (leftWalljump > 0)
            {
                if (Walled())
                {
                    leftWalljump--;
                    Jump();
                }
            }
        }
    }

    void Jump()
    {
       
        gameObject.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * jumpForce, ForceMode2D.Impulse);
    }


    void Start()
    {
        leftWalljump = maxWallJump;
    }

    void Update()
    {
        //When player move to right
        if (moveToRight)
        {
            //If the player move to left
            if (currentTimeToAcceleration <= 0)
            {
                currentTimeToAcceleration += factorChangeDirection * Time.deltaTime;
                if (currentTimeToAcceleration < startTimeAcceleration)
                    currentTimeToAcceleration = startTimeAcceleration;
            }

            //Add Acceleration
            currentTimeToAcceleration += Time.deltaTime;

            //Clamp Acceleration
            if (currentTimeToAcceleration > timeToAcceleration)
                currentTimeToAcceleration = timeToAcceleration;
        }

        //When player move to left
        if (moveToLeft)
        {
            //if the player move to right
            if (currentTimeToAcceleration > 0)
            {
                currentTimeToAcceleration -= factorChangeDirection * Time.deltaTime;
                if (currentTimeToAcceleration > (-1) * startTimeAcceleration)
                    currentTimeToAcceleration = (-1) * startTimeAcceleration;
            }

            //add acceleration
            currentTimeToAcceleration -= Time.deltaTime;

            //clamp acceleration
            if (currentTimeToAcceleration < (-1) * timeToAcceleration)
                currentTimeToAcceleration = -timeToAcceleration;
        }

        //if no movement or both direction
        if (!(moveToLeft ^ moveToRight))
        {
            //if player move to right
            if (currentTimeToAcceleration > 0)
            {
                currentTimeToAcceleration -= factorDecelerate * Time.deltaTime;
                if (currentTimeToAcceleration < 0)
                    currentTimeToAcceleration = 0;
            }
            //if player move to left
            if (currentTimeToAcceleration < 0)
            {
                currentTimeToAcceleration += factorDecelerate * Time.deltaTime;
                if (currentTimeToAcceleration > 0)
                    currentTimeToAcceleration = 0;
            }
        }

        //gameObject.GetComponent<Rigidbody2D>().AddForce( new Vector3( speed * Time.deltaTime * (currentTimeToAcceleration / timeToAcceleration) , 0, 0) );
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * (currentTimeToAcceleration / timeToAcceleration), 0));

    }
}
