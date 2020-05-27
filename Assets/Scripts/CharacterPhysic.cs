using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CharacterPhysics : MonoBehaviour
{
    public LayerMask collisionMask;

    private new BoxCollider collider;
    private Vector3 size;
    private Vector3 center;

    private float skin = 0.005f;

    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool movementStop;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        size = collider.size;
        center = collider.center;
    }

    public void Move(Vector2 moveAmount)
    {

        float deltaY = moveAmount.y;
        float deltaX = moveAmount.x;

        Vector2 position = transform.position;

        //check collision lower and below
        grounded = false;

        for (int i = 0; i < 3; i++)
        {
            float dir = Mathf.Sign(deltaY);
            float x = (position.x + center.x - size.x / 2.0f) + size.x / 2.0f * i; //left, center and then rightmost point of collider
            float y = position.y + center.y + size.y / 2.0f * dir; //bottom of collider

            ray = new Ray(new Vector2(x, y), new Vector2(0, dir));
            Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask))
            {


                //get distance betweend player and ground
                float dst = Vector3.Distance(ray.origin, hit.point);

                //stop player downward mvt after coming within skin width of collider
                if (dst > skin)
                    deltaY = dst * dir - skin * dir;
                else
                    deltaY = 0;

                grounded = true;
                //break;
            }
        }

        //check collision left and right
        movementStop = false;
        for (int i = 1; i < 4; i++)
        {
            float dir = Mathf.Sign(deltaX);
            float x = position.x + center.x + size.x / 2.0f * dir;
            float y = position.y + center.y - size.y + size.y / 2.0f * i;

            ray = new Ray(new Vector2(x, y), new Vector2(dir, 0));
            Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask))
            {


                //get distance betweend player and ground
                float dst = Vector3.Distance(ray.origin, hit.point);

                //stop player downward mvt after coming within skin width of collider
                if (dst > skin)
                    deltaX = dst * dir - skin * dir;
                else
                    deltaX = 0;

                movementStop = true;
                //break;
            }
        }

        Vector2 finalTransform = new Vector2(deltaX, deltaY);


        transform.Translate(finalTransform);

    }
}
