using UnityEngine;

public class RedTrap : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;

   
    private void OnCollisionEnter(Collision collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        TryDamagePlayer(other.gameObject);
    }

    private void TryDamagePlayer(GameObject hitObject)
    {
        if (hitObject.CompareTag("Player"))
        {
            HealthManager health = hitObject.GetComponent<HealthManager>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}