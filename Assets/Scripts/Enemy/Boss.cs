using UnityEngine;

public class Boss : Enemy
{
    protected override void Start()
    {
        // Initialise tout comme un Enemy
        base.Start();

        // Surcharge les stats du boss
        startingHealth = 500f;        // plus de PV
        damage = 5f;                  // dégâts plus élevés
        attackDistance = 2.5f;        // portée attaque
        detectionDistance = 10f;      // détection du joueur
        attackRate = 2f;              // fréquence attaque
    }

    public override void Die()
    {
        if (dead) return;

        base.Die();

        Debug.Log("Boss mort !");
        // Ici tu peux déclencher des choses spéciales : musique, portes, etc.
        // Les animations de mort sont déjà gérées par Enemy.cs via anim.SetBool("IsDead", true)
    }
}