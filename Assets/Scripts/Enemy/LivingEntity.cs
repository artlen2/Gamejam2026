using System.Collections;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 50;
    public float health { get; protected set; }
    protected bool dead;

    [SerializeField] private float maxHealth = 50;
    [SerializeField] private GameObject hitEffect;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, hitPoint, Quaternion.LookRotation(hitDirection));
        }

        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        if (dead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (dead) return;

        dead = true;

        OnDeath?.Invoke();
    }
}