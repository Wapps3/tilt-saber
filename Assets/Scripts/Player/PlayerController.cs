using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float timeToAcceleration;
    public float startTimeAcceleration;
    public float factorDecelerate;
    public float factorChangeDirection;

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

    void OnMoveJump(InputValue value)
    {

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = gameObject.transform.position;

        if (moveToRight)
        {
            if(currentTimeToAcceleration < 0)
            {
                currentTimeToAcceleration += factorChangeDirection * Time.deltaTime;
                if (currentTimeToAcceleration > startTimeAcceleration)
                    currentTimeToAcceleration = startTimeAcceleration;
            }

            currentTimeToAcceleration += Time.deltaTime;

            if (currentTimeToAcceleration > timeToAcceleration)
                currentTimeToAcceleration = timeToAcceleration;
        }

        if (moveToLeft)
        {
            if (currentTimeToAcceleration > 0)
            {
                currentTimeToAcceleration -= factorChangeDirection * Time.deltaTime;
                if (currentTimeToAcceleration < (-1)*startTimeAcceleration)
                    currentTimeToAcceleration = (-1)*startTimeAcceleration;
            }

            currentTimeToAcceleration -= Time.deltaTime;

            if (currentTimeToAcceleration < (-1)*timeToAcceleration)
                currentTimeToAcceleration = -timeToAcceleration;
        }

        if(!(moveToLeft ^ moveToRight))
        {
            if(currentTimeToAcceleration > 0)
            {
                currentTimeToAcceleration -= factorDecelerate * Time.deltaTime;
                if (currentTimeToAcceleration < 0)
                    currentTimeToAcceleration = 0;
            }
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
