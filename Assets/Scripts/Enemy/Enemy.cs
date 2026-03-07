using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    NavMeshAgent agent; //Système de navigation du l'ennemi
    Transform target; //joueur
    Animator anim;

    public float detectionDistance = 2f;
    public float attackDistance = 1.5f;
    public float damage = 1f;
    public float attackRate = 1f;

    float nextAttackTime;


    public event System.Action OnDeath;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // trouver le joueur dans la scène
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (dead) return;
        if (target == null) return;

        //animation de course
        anim.SetFloat("Speed", agent.velocity.magnitude);

        float distance = Vector3.Distance(transform.position, target.position);

        //joueur trop loin donc statut de idle
        if (distance > detectionDistance)
        {
            agent.ResetPath();
            anim.SetFloat("Speed", 0);
            return;
        }

        //joueur détécter donc attaque
        agent.SetDestination(target.position);
        anim.SetFloat("Speed", agent.velocity.magnitude);

        // attaquer si le player est assez proche
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


    public override void Die()
    {
        if(OnDeath != null)
        {
            OnDeath();
        }
        base.Die();
    }


}
