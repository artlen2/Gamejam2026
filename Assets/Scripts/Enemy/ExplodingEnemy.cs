using UnityEngine;

public class ExplodingEnemy : Enemy
{
    public float explosionRadius = 3f;
    public float explosionDamage = 20f;

    public GameObject explosionEffect;

    public override void Die()
    {
        Explode();

        base.Die();
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(explosionDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, explosionRadius);
}
}