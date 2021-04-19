using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private int score;

    private void Awake()
    {
        score = 0;
    }

    public void UpdateScore(int amount)
    {
        score += amount;

        scoreText.text = "Score: " + score.ToString();
    }
}
