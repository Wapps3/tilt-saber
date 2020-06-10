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
        Debug.Log(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           Debug.Log(collision.gameObject);
           collision.gameObject.GetComponent<Controller2D>().Hit(owner);
        }

        else if( collision.gameObject.CompareTag("Projectile") )
        {
            Debug.Log("je renvoi la balle");
        }
    }
}
