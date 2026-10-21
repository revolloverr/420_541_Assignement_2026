using UnityEngine;
using TMPro; // TMP_Text

// Shows the speed boost icon and seconds left while the boost is on.
public class PowerUpHUD : MonoBehaviour
{
    [SerializeField] private PlayerPowerUps powerUps;
    [SerializeField] private GameObject speedIndicator; // icon + time
    [SerializeField] private TMP_Text speedTimeText;

    private void Update()
    {
        
        if (powerUps == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                powerUps = player.GetComponent<PlayerPowerUps>();
            }
            
            
            if (powerUps == null) return;
        }

        float timeLeft = powerUps.SpeedTimeLeft;

        
        if (speedIndicator != null)
        {
            speedIndicator.SetActive(timeLeft > 0f);
        }

        
        if (speedTimeText != null)
        {
            speedTimeText.text = Mathf.CeilToInt(timeLeft) + "s";
        }
    }
}