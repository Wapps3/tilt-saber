using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;

        gameObject.transform.position = new Vector3(pos.x + speed * Time.deltaTime, pos.y, pos.z);
    }
}
