using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthBar;
    public TextMeshProUGUI hpText;
    public Image healthBarFill;
    public TextMeshProUGUI statusText; // Shows "POISONED" or "SAFE"

    [Header("Colors")]
    public Color healthyColor = new Color(0.2f, 0.9f, 0.3f);
    public Color damagedColor = new Color(0.9f, 0.6f, 0.1f);
    public Color criticalColor = new Color(0.9f, 0.1f, 0.1f);

    private bool isPoisoned = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        UpdateUI();
        if (currentHealth <= 0f) Die();
    }

    public void SetPoisoned(bool poisoned)
    {
        isPoisoned = poisoned;
        UpdateUI();
    }

    void Die()
    {
        Debug.Log("Player died.");
    }

    void UpdateUI()
    {
        float pct = currentHealth / maxHealth;

        if (healthBar != null)
            healthBar.value = pct;

        if (hpText != null)
            hpText.text = $"HP: {Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";

        if (healthBarFill != null)
        {
            if (pct > 0.5f)
                healthBarFill.color = Color.Lerp(damagedColor, healthyColor, (pct - 0.5f) * 2f);
            else
                healthBarFill.color = Color.Lerp(criticalColor, damagedColor, pct * 2f);
        }

        if (statusText != null)
            statusText.text = isPoisoned ? "POISONED" : "SAFE";
    }
}