using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private ToxicFog toxicFog;

    void Start()
    {
        toxicFog = FindObjectOfType<ToxicFog>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && toxicFog != null)
            toxicFog.OnPlayerEnterSafeZone();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && toxicFog != null)
            toxicFog.OnPlayerExitSafeZone();
    }
}