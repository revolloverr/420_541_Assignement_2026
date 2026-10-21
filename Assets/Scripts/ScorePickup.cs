using UnityEngine;


// A coin that adds points to the score when the player touches it.
// Needs a collider with Is Trigger ticked on the same GameObject.
public class ScorePickup : MonoBehaviour
{
    // How many points this coin is worth (change it in the Inspector)
    [SerializeField] private int points = 10;
    [SerializeField] private AudioClip scoreSound;
    

    // Called automatically when something enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only react to the player, ignore everything else
        if (other.CompareTag("Player"))
        {
            int pointsToAdd = points;

            // Check if the player has the power-ups component and apply multiplier
            PlayerPowerUps powerUps = other.GetComponent<PlayerPowerUps>();
            if (powerUps != null)
            {
                pointsToAdd = Mathf.RoundToInt(points * powerUps.ScoreMultiplier);
            }

            // Add our points through the ScoreManager singleton
            if (GameManager.Instance != null)
            {
                //ScoreManager.Instance.AddScore(pointsToAdd);
                //GetComponent<AudioSource>().PlayOneShot(scoreSound);
                //Instantiate(transform.position, Quaternion.identity);

                GameManager.Instance.AddScore(pointsToAdd);
            }

            if (scoreSound != null)
            {
                AudioSource.PlayClipAtPoint(scoreSound, transform.position);
            }

            // Remove the coin from the scene: it's collected once and gone
            Destroy(gameObject);
        }
    }
}