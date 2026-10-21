using UnityEngine;
using TMPro; // TMP_Text
 
// Updates the HUD every frame from the ScoreManager and PlayerPowerUps.
public class HUDController : MonoBehaviour
{
    [Header("Score & Health UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private HealthManager playerHealth;
 
    private void Update()
    {
        // Automatically find the player's HealthManager if it's missing (e.g. after a scene load)
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<HealthManager>();
            }
        }

        UpdateScore();
        UpdateHealth();
    }
 
    private void UpdateScore()
    {
        if (scoreText != null && GameManager.Instance != null)
        {
            scoreText.text = "Score: " + GameManager.Instance.CurrentScore;
        }
    }

    private void UpdateHealth()
    {
        if (healthText != null && playerHealth != null)
        {
            healthText.text = "Health: " + playerHealth.CurrentHealth;
        }
    }
}