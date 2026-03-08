using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Image _healthbarSprite;
    [SerializeField] private float _reduceSpeed = 2;

    private float _target = 1;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
        _target = 1f;
        _healthbarSprite.fillAmount = 1f;
    }

    public void UpdateHealthBarEnemy(float maxHealth, float currentHealth)
    {
        // Met à jour la cible (la santé actuelle par rapport à la santé maximale)
        _target = currentHealth / maxHealth;
    }

    private void Update()
    {
        // Fais en sorte que la barre de vie suive progressivement la cible
        _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, _target, _reduceSpeed * Time.deltaTime);
    }
}