using UnityEngine;
using System.Collections.Generic;

public class SpawnerPuceron : MonoBehaviour
{
    public Enemy puceronPrefab;
    public Transform[] spawnPoints;

    public int maxAlive = 5;
    public int totalToKill = 15;

    int aliveEnemies;
    int totalSpawned;
    int totalKilled;

    void Start()
    {
        SpawnUntilFull();
    }

    void SpawnUntilFull()
    {
        while (aliveEnemies < maxAlive && totalSpawned < totalToKill)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Crée une liste des spawn points **libres**
        List<Transform> freePoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            // On regarde si un enemy existe déjà à ce point (proche de 0.1 unité)
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

        // S'il n'y a pas de spawn libre → stop
        if (freePoints.Count == 0)
            return;

        // Choisir un spawn libre au hasard
        Transform spawnPoint = freePoints[Random.Range(0, freePoints.Count)];

        Enemy enemy = Instantiate(puceronPrefab, spawnPoint.position, Quaternion.identity);
        enemy.OnDeath += OnEnemyDeath;

        aliveEnemies++;
        totalSpawned++;

        Debug.Log("Spawn enemy | Alive: " + aliveEnemies + " Spawned: " + totalSpawned);
    }

    void OnEnemyDeath()
    {
        aliveEnemies--;
        totalKilled++;

        Debug.Log("Enemy died | Alive: " + aliveEnemies + " Killed: " + totalKilled);

        if (totalSpawned < totalToKill)
        {
            SpawnUntilFull();
        }
    }
}