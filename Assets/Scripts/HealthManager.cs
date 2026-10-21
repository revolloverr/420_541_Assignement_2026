using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    //[SerializeField] private string endSceneName = "EndScene";
    public int CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        Debug.Log("Player Health: " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            //GameManager.ResetScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //private void RestartLevel()
    //{
    //    ScoreManager.Instance.SaveFinalScore();
    //    SceneManager.LoadScene(SceneManager.GetActiveScene(sceneName));
    //}
}