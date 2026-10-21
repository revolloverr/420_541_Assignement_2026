using UnityEngine;

// A pickup that enables the double jump ability for a limited duration
public class JumpBoostPickup : MonoBehaviour
{
    [SerializeField] private float duration = 30f; 
    [SerializeField] private GameObject visuals;
    [SerializeField] private float respawnDelay = 30f; // time hidden

    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player
        if (other.CompareTag("Player"))
        {
            PlayerPowerUps powerUps = other.GetComponent<PlayerPowerUps>();
            if (powerUps != null)
            {
                powerUps.ActivateJumpBoost(duration);
            }

            // Remove the pickup from the scene
           visuals.SetActive(false);
           GetComponent<Collider>().enabled = false;
           Invoke(nameof(Reappear), respawnDelay);
        }
    }

    private void Reappear()
    {
        visuals.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }
}