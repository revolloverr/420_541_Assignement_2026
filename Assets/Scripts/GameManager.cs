using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [Header("Score Settings")]
    private int currentScore = 0;

    public int CurrentScore
    {
        get => currentScore;
        private set => currentScore = value;
    }

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        
        DontDestroyOnLoad(gameObject);
    }

    
    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log("GameManager Score: " + currentScore);
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

   
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

   
    public void LoadLevelByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}