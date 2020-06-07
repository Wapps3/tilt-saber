using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public ParticleSystem PS_BulletImpact;
    
    public int owner;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
        
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

        Instantiate(PS_BulletImpact, transform.position, Quaternion.identity).Play();
   
        Destroy(gameObject);
    }
}
