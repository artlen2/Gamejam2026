using UnityEngine;

public class ToxicFog : MonoBehaviour
{
    [Header("Fog Settings")]
    public Color fogColor = new Color(0.1f, 0.3f, 0.05f, 1f);
    public float fogDensity = 0.08f;
    public FogMode fogMode = FogMode.ExponentialSquared;

    private int safeZoneCount = 0;
    private PoisonEffect trackedPoison; // cache the player's PoisonEffect

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogMode = fogMode;
    }

    public void OnPlayerEnterSafeZone()
    {
        safeZoneCount++;
        UpdateFogAndPoison();
    }

    public void OnPlayerExitSafeZone()
    {
        safeZoneCount = Mathf.Max(0, safeZoneCount - 1);
        UpdateFogAndPoison();
    }

    void UpdateFogAndPoison()
    {
        bool inSafe = safeZoneCount > 0;
        RenderSettings.fogDensity = inSafe ? 0.005f : fogDensity;

        // Use cached reference instead of FindWithTag every call
        if (trackedPoison != null)
            trackedPoison.SetSuppressed(inSafe);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trackedPoison = other.GetComponent<PoisonEffect>();

            if (trackedPoison != null)
            {
                trackedPoison.Poison();
                // Apply suppression immediately if already in a safe zone
                trackedPoison.SetSuppressed(safeZoneCount > 0);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (trackedPoison != null)
            {
                trackedPoison.Cure();
                trackedPoison = null; // clear cache on exit
            }
        }
    }
}