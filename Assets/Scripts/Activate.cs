using UnityEngine;

public class Activate : MonoBehaviour
{
    [Header("Pillar Settings")]
    public float riseUp = 3f;
    public float riseSpeed = 5f;
    public float activeDuration = 10f;
    public float cooldown = 40f;

    [Header("Health Regeneration")]
    public float healAmount = 10f;
    public float healInterval = 1f;

    [Header("Safe Zone")]
    public GameObject safeZoneCollider;  // A sphere/box collider trigger that blocks fog
    public Light pillarLight;            // A Point Light on the pillar
    public float lightIntensityActive = 5f;
    public float lightIntensityIdle = 0.2f;
    public Color lightColorActive = new Color(0.4f, 1f, 0.5f);
    public Color lightColorIdle = new Color(0.2f, 0.2f, 0.2f);
    public ParticleSystem activateParticles; // Optional sparkle/glow effect on activation

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timer;
    private float cooldownTimer;
    private bool isRising, isUp, isLowering;
    private bool onCooldown = false;
    private bool playerInZone = false;
    private float healTimer;
    private PlayerHealth playerHealth;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.up * riseUp;

        if (pillarLight != null)
        {
            pillarLight.intensity = lightIntensityIdle;
            pillarLight.color = lightColorIdle;
        }

        if (safeZoneCollider != null)
            safeZoneCollider.SetActive(false);
    }

    void Update()
    {
        // Cooldown countdown
        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                onCooldown = false;
                Debug.Log($"{gameObject.name} is ready again.");
            }
        }

        // Rise
        if (isRising)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, riseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isRising = false;
                isUp = true;
                timer = activeDuration;
                ActivateSafeZone();
            }
        }

        // Active phase
        if (isUp)
        {
            timer -= Time.deltaTime;

            if (playerInZone && playerHealth != null)
            {
                healTimer -= Time.deltaTime;
                if (healTimer <= 0f)
                {
                    playerHealth.Heal(healAmount);
                    healTimer = healInterval;
                }
            }

            if (timer <= 0f)
            {
                isUp = false;
                isLowering = true;
                DeactivateSafeZone();
            }
        }

        // Lower
        if (isLowering)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, riseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                transform.position = startPosition;
                isLowering = false;
                onCooldown = true;
                cooldownTimer = cooldown;
            }
        }
    }

    void ActivateSafeZone()
    {
        if (safeZoneCollider != null)
            safeZoneCollider.SetActive(true);

        if (pillarLight != null)
        {
            pillarLight.intensity = lightIntensityActive;
            pillarLight.color = lightColorActive;
        }

        if (activateParticles != null)
            activateParticles.Play();
    }

    void DeactivateSafeZone()
    {
        if (safeZoneCollider != null)
            safeZoneCollider.SetActive(false);

        if (pillarLight != null)
        {
            pillarLight.intensity = lightIntensityIdle;
            pillarLight.color = lightColorIdle;
        }

        if (activateParticles != null)
            activateParticles.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            healTimer = 0f;
            playerHealth = other.GetComponent<PlayerHealth>();

            PoisonEffect poison = other.GetComponent<PoisonEffect>();
            if (poison != null) poison.SetSuppressed(true);

            // Activate if not on cooldown and not already active
            if (!onCooldown && !isRising && !isUp && !isLowering)
                isRising = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            playerHealth = null;

            // Only re-expose to poison if the safe zone is not active
            if (!isUp)
            {
                PoisonEffect poison = other.GetComponent<PoisonEffect>();
                if (poison != null) poison.SetSuppressed(false);
            }
        }
    }
}