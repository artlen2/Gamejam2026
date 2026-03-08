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

    [SerializeField] private HealthBarEnemy healthBar;
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
        anim.SetFloat("Speed", agent.velocity.magnitude);

        if (distance < attackDistance && Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;
            Attack();
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        PlayerHealthPoison player = target.GetComponent<PlayerHealthPoison>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    public override void TakeDamage(float damage)
    {
        if (dead) return;

        Debug.Log("Took damage: " + damage);

        base.TakeDamage(damage);

        // Mise à jour de la barre de vie
        healthBar.UpdateHealthBarEnemy(startingHealth, health);

        anim.SetTrigger("Hit");

        StartCoroutine(Stun());
    }

    IEnumerator Stun()
    {
        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        yield return new WaitForSeconds(0.4f);

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
    }

    public override void Die()
    {
        if (dead) return;

        base.Die();

        anim.SetBool("IsDead", true);

        agent.isStopped = true;
        agent.ResetPath();
        agent.enabled = false;

        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        // Attendre la fin de l'animation de mort (ajuste cette durée à celle de ton animation de mort)
        yield return new WaitForSeconds(2f);  // Ajuste la durée à celle de l'animation

        // Détruire l'objet une fois l'animation terminée
        Destroy(gameObject);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}

