using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        ScoreManager.Instance.OnAddScore += UpdateScore;
        UpdateScore();
    }
    private void UpdateScore()
    {
        scoreText.text = "" + ScoreManager.Instance.Score;
        // if (ScoreManager.Instance.Score == 6)
        // {
        //     SceneManager.LoadScene("endExplanation");
        // }
    }
}