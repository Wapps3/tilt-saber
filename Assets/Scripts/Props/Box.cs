using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float minScale = .2f;
    public float maxScale = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        sr.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        float size = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(size, size, size);
        rb.mass = size;
    }

}
