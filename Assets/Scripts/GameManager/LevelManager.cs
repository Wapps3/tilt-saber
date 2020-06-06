using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> listSpwaner;
    public float timeBetweenRespawn;
    public List<float> timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < timeRemaining.Count; i++)
        {
            if(timeRemaining[i] > 0)
            {
                timeRemaining[i] -= Time.deltaTime;
            }
        }
    }

    public void Respawn(GameObject player)
    {
       // player.transform.position;
    }
}
