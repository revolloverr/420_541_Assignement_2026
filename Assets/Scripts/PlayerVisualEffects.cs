using UnityEngine;

public class PlayerVisualEffects : MonoBehaviour
{
    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem doubleJumpParticles; 

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;           
    [SerializeField] private AudioClip doubleJumpSound;        

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }


    public void PlayDoubleJumpEffects()
    {
        // 1. Play Particle Effect
        if (doubleJumpParticles != null)
        {
            doubleJumpParticles.Play();
        }

        // 2. Play Sound Effect
        if (audioSource != null && doubleJumpSound != null)
        {
            audioSource.PlayOneShot(doubleJumpSound);
        }
    }
}