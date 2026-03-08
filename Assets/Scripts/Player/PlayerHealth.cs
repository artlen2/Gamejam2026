using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.fillAmount = maxHealth;
        healthSlider.fillAmount = currentHealth;
    }

    private void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //healthSlider.value = currentHealth;
        healthSlider.fillAmount = 1f;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");
    }

    //[SerializeField] private Image _healthbarSprite;
    //[SerializeField] private float _reduceSpeed = 2;

    //private float _target = 1;
    //private Camera _cam;

    ////private void Start()
    ////{
    ////    _cam = Camera.main;
    ////    _target = 1f;
        
    ////}

    //public void UpdateHealthBarEnemy(float maxHealth, float currentHealth)
    //{
    //    // Met à jour la cible (la santé actuelle par rapport à la santé maximale)
    //    _target = currentHealth / maxHealth;
    //}

    //private void Update()
    //{
    //    // Fais en sorte que la barre de vie suive progressivement la cible
    //    _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, _target, _reduceSpeed * Time.deltaTime);
    //}
}