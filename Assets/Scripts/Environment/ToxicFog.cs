using UnityEngine;

public class ToxicFog : MonoBehaviour
{
    [Header("Fog Settings")]
    public Color fogColor = new Color(0.1f, 0.3f, 0.05f, 1f);
    public float fogDensity = 0.08f;
    public FogMode fogMode = FogMode.ExponentialSquared;


    private bool playerInSafeZone = false;
    private int safeZoneCount = 0; // Tracks overlapping safe zones

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogMode = fogMode;

    }

    // Called by SafeZone.cs when player enters/exits
    public void OnPlayerEnterSafeZone()
    {
        safeZoneCount++;
        UpdateFog();
    }

    public void OnPlayerExitSafeZone()
    {
        safeZoneCount = Mathf.Max(0, safeZoneCount - 1);
        UpdateFog();
    }

    void UpdateFog()
    {
        bool inSafe = safeZoneCount > 0;

        // Thin the fog when inside a safe zone
        RenderSettings.fogDensity = inSafe ? 0.005f : fogDensity;


        // Poison the player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PoisonEffect poison = player.GetComponent<PoisonEffect>();
            if (poison != null) poison.SetSuppressed(inSafe);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            PoisonEffect poison = player.GetComponent<PoisonEffect>();
            if (poison != null) poison.Poison();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoisonEffect poison = other.GetComponent<PoisonEffect>();
            if (poison != null) poison.Cure();
        }
    }
}