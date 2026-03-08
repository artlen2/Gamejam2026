using UnityEngine;
using System.Collections;

public class SafeZone : MonoBehaviour
{
    [Header("Health Regeneration")]
    public float healAmount = 10f;
    public float healInterval = 1f;

    [Header("Visual Growth")]
    public float growDuration = 1.5f;       // How long it takes to fully grow
    public float shrinkDuration = 1f;       // How long it takes to shrink away
    public Vector3 fullScale = new Vector3(5f, 5f, 5f);   // Max size of the zone
    private Vector3 zeroScale = Vector3.zero;

    private float healTimer;
    private PlayerHealthPoison playerHealth;
    private PoisonEffect playerPoison;
    private bool playerInZone = false;

    void OnEnable()
    {
        // Start small, grow to full size
        transform.localScale = zeroScale;
        StartCoroutine(ScaleTo(fullScale, growDuration));

        // Check if player is already inside when zone activates
        Collider col = GetComponent<Collider>();
        if (col is SphereCollider sphere)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, sphere.radius);
            foreach (var hit in hits)
                if (hit.CompareTag("Player")) EnterZone(hit);
        }
    }

    void OnDisable()
    {
        if (playerInZone) ExitZone();
        StopAllCoroutines();
        transform.localScale = zeroScale;
    }

    // Call this from ActivatePillar before disabling, so player sees shrink before zone turns off
    public IEnumerator ShrinkAndDisable()
    {
        if (playerInZone) ExitZone();
        yield return StartCoroutine(ScaleTo(zeroScale, shrinkDuration));
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!playerInZone || playerHealth == null) return;

        healTimer -= Time.deltaTime;
        if (healTimer <= 0f)
        {
            playerHealth.Heal(healAmount);
            healTimer = healInterval;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) EnterZone(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) ExitZone();
    }

    void EnterZone(Collider other)
    {
        playerInZone = true;
        healTimer = 0f;
        playerHealth = other.GetComponent<PlayerHealthPoison>();
        playerPoison = other.GetComponent<PoisonEffect>();
        if (playerPoison != null) playerPoison.SetSuppressed(true);
    }

    void ExitZone()
    {
        playerInZone = false;
        playerHealth = null;
        if (playerPoison != null) playerPoison.SetSuppressed(false);
        playerPoison = null;
    }

    IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration); // smooth easing
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}