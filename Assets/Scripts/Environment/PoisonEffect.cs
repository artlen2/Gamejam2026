using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    [Header("Poison Settings")]
    public float damagePerTick = 5f;
    public float tickInterval = 1f;
    public bool isPoisoned = false;

    private bool isSuppressed = false;
    private float tickTimer;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        tickTimer = tickInterval;
    }

    void Update()
    {
        if (!isPoisoned || isSuppressed) return;

        // Apply damage over time
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            playerHealth?.TakeDamage(damagePerTick);
            tickTimer = tickInterval;
        }
    }

    public void SetSuppressed(bool suppressed)
    {
        // If we're suppressing, we want to stop the poison effect
        isSuppressed = suppressed;
        playerHealth?.SetPoisoned(isPoisoned && !suppressed);
    }

    public void Poison()
    {
        // Only apply poison if we're not already poisoned
        isPoisoned = true;
        playerHealth?.SetPoisoned(!isSuppressed);
    }

    public void Cure()
    {
        // Cure the poison and reset all related states (when level is completed)
        isPoisoned = false;
        isSuppressed = false;
        playerHealth?.SetPoisoned(false);
    }
}
