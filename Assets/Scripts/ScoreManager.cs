using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    private static ScoreManager m_Instance;
    public static ScoreManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<ScoreManager>();
                if (m_Instance == null)
                {
                    GameObject obj = new GameObject("ScoreManager");
                    m_Instance = obj.AddComponent<ScoreManager>();
                }
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public static readonly int MAXIMUM_SCORE = 4;

    public event Action OnAddScore;

    public int Score { get; private set; }

    public void AddScore()
    {
        Score++;
        OnAddScore?.Invoke();
    }
}
