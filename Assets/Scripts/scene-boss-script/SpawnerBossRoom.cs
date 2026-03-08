using System.Collections.Generic;
using UnityEngine;

public class SpawnerBossRoom : MonoBehaviour
{
    public Enemy enemyPrefab;           // L’ennemi à spawn
    public Transform[] spawnPoints;     // Points de spawn
    public Boss boss;                   // Le boss de la salle
    public int maxAlive = 5;            // Max ennemis vivants

    int aliveEnemies;

    void Update()
    {
        // Si boss mort → stop spawn
        if (boss == null || boss.Health <= 0)
            return;

        // Spawn si moins que maxAlive
        if (aliveEnemies < maxAlive)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Crée une liste des spawn points libres
        List<Transform> freePoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            Collider[] hit = Physics.OverlapSphere(point.position, 0.5f);
            bool occupied = false;

            foreach (Collider col in hit)
            {
                if (col.GetComponent<Enemy>() != null)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
                freePoints.Add(point);
        }

        if (freePoints.Count == 0)
            return; // Tous les points occupés → rien spawn

        Transform spawnPoint = freePoints[Random.Range(0, freePoints.Count)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        enemy.OnDeath += OnEnemyDeath;

        aliveEnemies++;

        Debug.Log("Spawn enemy | Alive: " + aliveEnemies);
    }

    void OnEnemyDeath()
    {
        aliveEnemies--;
        Debug.Log("Enemy died | Alive: " + aliveEnemies);
    }
}