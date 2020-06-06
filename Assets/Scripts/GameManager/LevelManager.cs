using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> listSpwaner;
    public float timeBetweenRespawn;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 RespawnPos(GameObject player)
    {
        int indexSpawner = Random.Range(0, listSpwaner.Count);

        return listSpwaner[indexSpawner].transform.position;
    }
}
