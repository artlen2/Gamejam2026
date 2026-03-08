using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxHealth = 200f;
    public float Health { get; private set; }

    public bool IsDead => Health <= 0;

    void Start()
    {
        Health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        Health -= damage;
        Debug.Log("Boss health: " + Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss mort !");
        // Ici tu peux déclencher animations, portes, musique, etc.
    }
}