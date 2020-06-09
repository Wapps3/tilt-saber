using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour
{
    public LevelManager levelRef;

    public int playerID;

    public CapsuleCollider2D colliderCustom;

    public Rigidbody2D rigidBody;

    public LayerMask collisionMask;

    public float scaleGravity;

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

    public float timeBetweenFire;
    private float timeBeforeFire;

    public float timeBetweenAttack;
    private float timeBeforeAttack;

    public float fastFallForce;
    private bool fastFall;

    public Animator animator;

    public GameObject bulletPrefab;
    public float bulletForce;
    public GameObject gun;
    public float recoilForce;


    void OnFire(InputValue value)
    {
        if (timeBeforeFire <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, gun.transform.position + new Vector3(gameObject.transform.localScale.x,0,0) , new Quaternion() );
            bullet.GetComponent<Bullet>().owner = playerID;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(gameObject.transform.localScale.x * bulletForce, 0), ForceMode2D.Impulse);

            //Add recoil after a shoot
            rigidBody.velocity = new Vector2(0, 0);
            rigidBody.AddForce(new Vector2(gameObject.transform.localScale.x * (-1) * recoilForce, 0), ForceMode2D.Force );

            animator.SetTrigger("Fire");
            timeBeforeFire = timeBetweenFire;
        }
    }

    void OnAttack(InputValue value)
    {
        if (timeBeforeAttack <= 0)
        {

            animator.SetTrigger("Attack");
            timeBeforeAttack = timeBetweenAttack;

        }
    }

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

    void OnFastFall(InputValue value)
    {
        fastFall = true;
    }

    bool Grounded()
    {
        Ray ray;
        RaycastHit2D hit;

        Vector2 size = colliderCustom.size;
       // size.x = size.x * 0.5f
       // size.y = size.y * 0.5f;
        Vector2 center = colliderCustom.offset;

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

    bool WalledLeft()
    {
        Ray ray;
        RaycastHit2D hit;

        Vector2 size = colliderCustom.size;
        Vector2 center = colliderCustom.offset;

        for (int i = 0; i < 3; i++)
        {
            float x = gameObject.transform.position.x + center.x - (size.x / 2.0f); //bottom left collider
            float y = gameObject.transform.position.y + center.y - (size.y / 2.0f) + (size.y / 2.0f) * i; //bottom middle top collider

            ray = new Ray(new Vector2(x, y), new Vector2(-1, 0));

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 5.0f);

            if (hit = Physics2D.Raycast(ray.origin, ray.direction, 0.5f, collisionMask))
            {
                return true;
            }
        }
            return false;
    }

    bool WalledRight()
    {
        Ray ray;
        RaycastHit2D hit;

        Vector2 size = colliderCustom.size;
        Vector2 center = colliderCustom.offset;

        for (int i = 0; i < 3; i++)
        {
            float x = gameObject.transform.position.x + center.x + (size.x / 2.0f); //bottom left collider
            float y = gameObject.transform.position.y + center.y - (size.y / 2.0f) + (size.y / 2.0f) * i; //bottom middle top collider

            ray = new Ray(new Vector2(x, y), new Vector2(1, 0));

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 5.0f);

            if (hit = Physics2D.Raycast(ray.origin, ray.direction, 0.5f, collisionMask))
            {
                return true;
            }
        }
            return false;
    }
    bool Walled()
    {
        if(WalledLeft() || WalledRight() )
        {
            return true;
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
        animator.SetTrigger("Jump");
        rigidBody.AddForce(gameObject.transform.up * jumpForce, ForceMode2D.Force);
    }

    public void Hit(int ID)
    {
        levelRef.IncrementScore(ID);
        gameObject.GetComponent<PlayerInput>().enabled = false;
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        animator.SetTrigger("Die");

        yield return new WaitForSeconds(1);
        
        rigidBody.position = levelRef.RespawnPos(gameObject);
        gameObject.GetComponent<PlayerInput>().enabled = true;
    }

    void Start()
    {
        leftWalljump = maxWallJump;
        timeBeforeFire = 0;
        timeBeforeAttack = 0;
        fastFall = false;
        GetComponentInChildren<Sword>().owner = playerID;
    }

    void Update()
    {
        if (timeBeforeFire > 0)
            timeBeforeFire -= Time.deltaTime;

        if (timeBeforeAttack > 0)
            timeBeforeAttack -= Time.deltaTime;

        //When player move to right
        if (moveToRight)
        {
            //If the player move to left
            if (currentTimeToAcceleration <= 0)
            {
                currentTimeToAcceleration += factorChangeDirection * Time.deltaTime;
                if (currentTimeToAcceleration < startTimeAcceleration)
                    currentTimeToAcceleration = startTimeAcceleration;

                rigidBody.velocity = new Vector3(0, rigidBody.velocity.y);
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
            if (currentTimeToAcceleration >= 0)
            {
                currentTimeToAcceleration -= factorChangeDirection * Time.deltaTime;
                if (currentTimeToAcceleration > (-1) * startTimeAcceleration)
                    currentTimeToAcceleration = (-1) * startTimeAcceleration;

                rigidBody.velocity = new Vector3(0, rigidBody.velocity.y,0);
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
        
        if((currentTimeToAcceleration / timeToAcceleration) < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        if((currentTimeToAcceleration / timeToAcceleration) > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        animator.SetFloat("Acceleration", Mathf.Abs( (currentTimeToAcceleration / timeToAcceleration) ) );
        rigidBody.AddForce(new Vector2(speed * (currentTimeToAcceleration / timeToAcceleration), 0), ForceMode2D.Force);

        bool walledRight = WalledRight();
        bool walledLeft = WalledLeft();
        bool grounded = Grounded();

        Debug.Log("wl" + walledLeft);
        Debug.Log("wr" + walledRight);
        Debug.Log("gr" + grounded);

        if ( walledRight &  !grounded & moveToRight )
        {
            animator.SetBool("Walled", true);
        }
        else if( walledLeft  & !grounded & moveToLeft )
        {
            Debug.Log("Alo?");
            animator.SetBool("Walled", true);
        }
        else
        {
            animator.SetBool("Walled", false);
        }

        if(grounded)
        {
            fastFall = false;
        }
        
        if(fastFall)
        {
            rigidBody.AddForce(new Vector2(0, fastFallForce * (-1) ) );
        }

        if( /*!Grounded()*/ true )
        {
            if( !(walledRight & moveToRight) )
            {
                if ( !(walledLeft & moveToLeft) )
                {
                    rigidBody.AddForce(new Vector2(0, (-1) * scaleGravity), ForceMode2D.Force);
                }
            }
        }
        
    }
}
