using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> listSpwaner;
    public List<Text> listScore;

    public int playerNumber;

    public Canvas UI;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < listScore.Count; i++)
        {
            listScore[i].GetComponent<ScoreBindingText>().ID = i;
        }

        for(int i = playerNumber; i < listScore.Count; i++)
        {
            listScore[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementScore(int playerID)
    {
        listScore[playerID].GetComponent<ScoreBindingText>().IncrementScore();
    }

    public Vector3 RespawnPos(GameObject player)
    {
        int indexSpawner = Random.Range(0, listSpwaner.Count);

        return listSpwaner[indexSpawner].transform.position;
    }
}
