using UnityEngine;

public class PillarManager : MonoBehaviour
{
    public static PillarManager Instance;

    [Header("Pillars")]
    public ActivatePillar[] pillars; // Drag all 4 Activate pillar objects here

    private int activatedCount = 0;
    private bool allActivated = false;

    void Awake()
    {
        Instance = this;
    }

    public void PillarActivated()
    {
        activatedCount++;
        Debug.Log($"Pillars activated: {activatedCount}/4");

        if (activatedCount >= 4 && !allActivated)
        {
            allActivated = true;
            OnAllPillarsActivated();
        }
    }

    void OnAllPillarsActivated()
    {
        Debug.Log("All 4 pillars activated! Player is cured.");

        // Find player and cure poison
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PoisonEffect poison = player.GetComponent<PoisonEffect>();
            if (poison != null) poison.Cure();

            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null) health.Heal(health.maxHealth); // Full heal
        }

        // Dissipate the fog
        RenderSettings.fogDensity = 0f;
    }
}