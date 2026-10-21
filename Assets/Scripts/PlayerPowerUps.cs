using UnityEngine;

// Manages player power-ups like Speed Boost, Score Boost, and Jump Boost.
public class PlayerPowerUps : MonoBehaviour
{
    [SerializeField] private float speedBoostMultiplier = 1.5f; // 50% faster
    [SerializeField] private float scoreBoostMultiplier = 2f;   // Double score from coins

    // Seconds of boost left. The HUD reads it; only this script changes it.
    public float SpeedTimeLeft { get; private set; }
    public float ScoreTimeLeft { get; private set; }
    public float JumpTimeLeft { get; private set; }

    // Expose states/multipliers
    public float ScoreMultiplier => ScoreTimeLeft > 0f ? scoreBoostMultiplier : 1f;
    public bool HasJumpBoost => JumpTimeLeft > 0f;

    private PlayerMovement movement;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public void ActivateSpeedBoost(float duration)
    {
        SpeedTimeLeft = duration;
        if (movement != null)
        {
            movement.SpeedMultiplier = speedBoostMultiplier;
        }
    }

    public void ActivateScoreBoost(float duration)
    {
        ScoreTimeLeft = duration;
    }

    // Called by the jump boost pickup
    public void ActivateJumpBoost(float duration)
    {
        JumpTimeLeft = duration;
    }

    private void Update()
    {
        // 1. Handle Speed Boost Timer
        if (SpeedTimeLeft > 0f)
        {
            SpeedTimeLeft -= Time.deltaTime;
            if (SpeedTimeLeft <= 0f)
            {
                if (movement != null)
                {
                    movement.SpeedMultiplier = 1f;
                }
            }
        }

        // 2. Handle Score Boost Timer
        if (ScoreTimeLeft > 0f)
        {
            ScoreTimeLeft -= Time.deltaTime;
        }

        // 3. Handle Jump Boost Timer
        if (JumpTimeLeft > 0f)
        {
            JumpTimeLeft -= Time.deltaTime;
        }
    }
}