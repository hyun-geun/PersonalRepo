using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    private int count = 6;

    private GameObject[] enemies;
    private bool[] isSpawned;
    private Vector2[] enemySpawnPosition = { new Vector2(-5f, 2f), new Vector2(-6.8f, 3.5f), new Vector2(-1.75f, 0f),
                                            new Vector2(2.5f, 0), new Vector2(2.5f, 1.7f), new Vector2(-0.5f, 1.7f) };
    int index;

    void Start()
    {
        enemies = new GameObject[count];
        isSpawned = new bool[6];

        for(int i = 0; i < count; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, enemySpawnPosition[i], Quaternion.identity);
            isSpawned[i] = false;
        }
    }

    void Update()
    {
        for(int i = 0; i < count; i++)
        {
            if (enemies[i] == null && !isSpawned[i])
            {
                isSpawned[i] = true;
                index = i;

                Invoke("ReSpawnEnemy", 5);
            }
            else if (isSpawned[i] && enemies[i] != null)
            {
                isSpawned[i] = false;
            }
        }
    }

    void ReSpawnEnemy()
    {
        enemies[index]= Instantiate(enemyPrefab, enemySpawnPosition[index], Quaternion.identity);
        isSpawned[index] = false;
    }
}
