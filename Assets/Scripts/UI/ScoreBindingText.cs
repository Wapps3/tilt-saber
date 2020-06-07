using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBindingText : MonoBehaviour
{
    private int score = 0;
    public int ID;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Player " + ID +" : "+ score;
    }

    public void IncrementScore()
    {
        score++;
    }
}
