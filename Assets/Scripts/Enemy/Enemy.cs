using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : LivingEntity
{
    NavMeshAgent agent;
    Transform target;
    Animator anim;

    [SerializeField] HealthBarEnemy healthBar;
    [SerializeField] Renderer modelRenderer;

    Color originalColor;

    public float detectionDistance = 2f;
    public float attackDistance = 1.5f;
    public float damage = 1f;
    public float attackRate = 1f;

    float nextAttackTime;

    protected override void Start()

    {

        base.Start();

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        originalColor = modelRenderer.material.color;
    }

    void Update()
    {
        if (dead) return;
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > detectionDistance)
        {
            agent.ResetPath();
            anim.SetFloat("Speed", 0);
            return;
        }

        agent.SetDestination(target.position);
        anim.SetFloat("Speed", agent.desiredVelocity.magnitude);

        if (distance < attackDistance && Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            Attack();
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        LivingEntity player = target.GetComponent<LivingEntity>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log("Took damage" + damage);

        base.TakeDamage(damage);

        if (dead) return;

        healthBar.UpdateHealthBarEnemy(startingHealth, health);

        anim.SetTrigger("Hit");

        StartCoroutine(DamageFlash());

        StartCoroutine(Stun());
    }

    IEnumerator DamageFlash()
    {
        modelRenderer.material.color = Color.white;

        yield return new WaitForSeconds(0.3f);

        modelRenderer.material.color = originalColor;
    }

    IEnumerator Stun()
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(0.4f);

        agent.isStopped = false;
    }

public override void Die()
{
    if (dead) return;

    dead = true;
    
    anim.SetTrigger("Death");

    // Désactive l'Animator pour éviter toute nouvelle animation après la mort
    anim.enabled = false;

    agent.isStopped = true;

    // Éventuellement, supprimer l'objet après la durée de l'animation de mort
    Destroy(gameObject, 2f); // ajuste la durée pour correspondre à celle de l'animation
}

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

