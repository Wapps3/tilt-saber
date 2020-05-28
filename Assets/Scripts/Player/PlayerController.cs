using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public Rigidbody2D rigidbody;

    public float speed;
    public float timeToAcceleration;
    public float startTimeAcceleration;
    public float factorDecelerate;
    public float factorChangeDirection;

    public float jumpForce;

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

    void OnJump(InputValue value)
    {
        Debug.Log("je saute");
        gameObject.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * jumpForce);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = gameObject.transform.position;

        //When player move to right
        if (moveToRight)
        {
            //If the player move to left
            if(currentTimeToAcceleration < 0)
            {
                currentTimeToAcceleration += factorChangeDirection * Time.deltaTime;
                if (currentTimeToAcceleration > startTimeAcceleration)
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
                if (currentTimeToAcceleration < (-1)*startTimeAcceleration)
                    currentTimeToAcceleration = (-1)*startTimeAcceleration;
            }

            //add acceleration
            currentTimeToAcceleration -= Time.deltaTime;

            //clamp acceleration
            if (currentTimeToAcceleration < (-1)*timeToAcceleration)
                currentTimeToAcceleration = -timeToAcceleration;
        }

        //if no movement or both direction
        if(!(moveToLeft ^ moveToRight))
        {
            //if player move to right
            if(currentTimeToAcceleration > 0)
            {
                currentTimeToAcceleration -= factorDecelerate * Time.deltaTime;
                if (currentTimeToAcceleration < 0)
                    currentTimeToAcceleration = 0;
            }
            //if player move to left
            if(currentTimeToAcceleration < 0)
            {
                currentTimeToAcceleration += factorDecelerate * Time.deltaTime;
                if (currentTimeToAcceleration > 0)
                    currentTimeToAcceleration = 0;
            }
        }

        position.x += speed * Time.deltaTime * (currentTimeToAcceleration / timeToAcceleration);

        gameObject.transform.position = position;

    }
}
