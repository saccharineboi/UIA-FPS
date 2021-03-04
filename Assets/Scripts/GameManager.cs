using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
        public Transform enemySpawnPos;

        [SerializeField] GameObject enemyPrefab;
        GameObject enemy;

        void Start()
        {
                
        }

        void Update()
        {
                SpawnEnemy();
        }

        void SpawnEnemy()
        {
                if (enemy == null)
                {
                        enemy = Instantiate(enemyPrefab, enemySpawnPos);
                        float angle = Random.Range(0f, 360f);
                        enemy.transform.Rotate(0f, angle, 0f);
                }
        }
}
