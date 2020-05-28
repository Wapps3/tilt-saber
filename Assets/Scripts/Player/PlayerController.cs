using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float speed;

    private bool moveToRight = false;
    private bool moveToLeft = false;

    void OnMoveRight(InputValue value)
    {
        moveToRight = !moveToRight;
    }

    void OnMoveLeft(InputValue value)
    {
        moveToLeft = !moveToLeft;
    }

    void OnMoveJump(InputValue value)
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        moveToRight = false;
        moveToLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = gameObject.transform.position;

        if(moveToRight)
        {
            position.x += speed * Time.deltaTime;
        }
        if (moveToLeft)
        {
            position.x -= speed * Time.deltaTime;
        }

        gameObject.transform.position = position;

    }
}
