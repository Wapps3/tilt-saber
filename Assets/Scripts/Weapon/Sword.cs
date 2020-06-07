using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Controller2D>())
        {
           collision.gameObject.GetComponent<Controller2D>().Hit(owner);
        }

        if( collision.gameObject.GetComponent<Bullet>())
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().
            Debug.Log("je renvoi la balle");
        }
    }
}
