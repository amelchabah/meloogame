using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject win;
    [SerializeField] private float delayBeforeMainMenu = 2f; // Temps d'attente avant de charger la scène MainMenu

    private void Awake()
    {
        ScoreManager.Instance.ResetScore();
        win.SetActive(false);
        scoreText = GetComponent<TextMeshProUGUI>();
        ScoreManager.Instance.OnAddScore += UpdateScore;
        UpdateScore();
    }

    private void UpdateScore()
    {
        // Vérifie si le scoreManager est toujours présent
        if (ScoreManager.Instance != null)
        {
            scoreText.text = "" + ScoreManager.Instance.Score;
            if (ScoreManager.Instance.Score == 4)
            {
                win.SetActive(true);
                StartCoroutine(WaitAndLoadMainMenu());
            }
        }
    }

    IEnumerator WaitAndLoadMainMenu()
    {
        yield return new WaitForSeconds(delayBeforeMainMenu);
        SceneManager.LoadScene("MainMenu");
    }

}