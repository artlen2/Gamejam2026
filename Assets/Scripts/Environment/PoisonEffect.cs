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

        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            playerHealth?.TakeDamage(damagePerTick);
            tickTimer = tickInterval;
        }
    }

    public void SetSuppressed(bool suppressed)
    {
        isSuppressed = suppressed;
        playerHealth?.SetPoisoned(isPoisoned && !suppressed);
    }

    public void Poison()
    {
        isPoisoned = true;
        playerHealth?.SetPoisoned(!isSuppressed);
    }

    public void Cure()
    {
        isPoisoned = false;
        isSuppressed = false;
        playerHealth?.SetPoisoned(false);
    }
}
