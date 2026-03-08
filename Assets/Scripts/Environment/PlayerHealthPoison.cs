using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealthPoison : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public TextMeshProUGUI hpText;

    [Header("Death Settings")]
    public float restartDelay = 2f;

    // Internal state
    private bool _isPoisoned = false;

    //public bool isAlive = true;
    //private float deathTimer = 0f;
    //private bool isDeathTimerRunning = false;
    //private Rigidbody rb;



    void Start()
    {
        currentHealth = maxHealth;
        //rb = GetComponent<Rigidbody>();
        UpdateUI();
    }

    //void Update()
    //{
    //    if (isDeathTimerRunning)
    //    {
    //        deathTimer -= Time.deltaTime;
    //        if (deathTimer <= 0f)
    //            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    }
    //}

    public void Heal(float amount)
    {
        //if (!isAlive) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0f);

        Debug.Log("Player HP: " + currentHealth);

        UpdateUI();

        if (currentHealth <= 0f) Die();
    }

    public void SetPoisoned(bool poisoned)
    {
        _isPoisoned = poisoned;
        UpdateUI();
    }

    void Die()
    {
        //if (!isAlive) return;
        //isAlive = false;

        Debug.Log("Player died.");

        //// Disable all MonoBehaviours on this object except this one
        //foreach (MonoBehaviour mb in GetComponents<MonoBehaviour>())
        //{
        //    if (mb != this) mb.enabled = false;
        //}

        //// Freeze rigidbody if present
        //if (rb != null)
        //{
        //    rb.linearVelocity = Vector3.zero;
        //    rb.isKinematic = true;
        //}

        //isDeathTimerRunning = true;
        //deathTimer = restartDelay;

        //if (hpText != null) hpText.text = "0 HP";
    }

    void UpdateUI()
    {
        float pct = currentHealth / maxHealth;


        if (hpText != null)
            hpText.text = $"{Mathf.CeilToInt(currentHealth)} HP";

        //if (hpText != null)
        //    hpText.text = _isPoisoned
        //        ? $"{Mathf.CeilToInt(currentHealth)} HP ☠"
        //        : $"{Mathf.CeilToInt(currentHealth)} HP";
    }
}
