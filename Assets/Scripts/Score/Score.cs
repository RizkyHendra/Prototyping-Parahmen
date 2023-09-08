using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text score;
    public static int scoreValue = 0;
    public int maxScore;
    public string comboText1;
    public string comboText5;
    public string comboText10;
    public static Animator animCombo;
    void Start()
    {
        score = GetComponent<TMP_Text>();
        animCombo = GetComponent<Animator>();
        scoreValue = 0;
    }
    
 
    void Update()
    {
        if(scoreValue >= 1)
        {
            score.text = scoreValue + "X" + "\n" + comboText1;
            score.color = Color.white;
        }
         if (scoreValue >= 5)
        {
            score.text = scoreValue + "X" + "\n" + comboText5;
            score.color = Color.yellow;
        }
         if(scoreValue >= 10)
        {
            score.text = scoreValue + "X" + "\n" + comboText10;
            score.color = Color.red;
        }

    }
}
