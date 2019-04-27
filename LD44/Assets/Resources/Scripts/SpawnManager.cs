using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPointParent;

    Transform[] allChildren;

    [SerializeField]
    private GameObject[] enemyTypes;

    private int enemyAmountMax = 3;
    private int enemyAmount = 0;
    private int enemyTypeMaxRange = 0;

    private int randomSpawnPointNum;
    private int randomEnemyType;

    [SerializeField]
    private bool spawnEnemyOn = true;

    [SerializeField]
    private bool inWave;
    private int waveNumber = 1;

    // timer
    public float spawnDelay = 3.0f;
    public float timer = 0.0f;

    private void Awake()
    {
        allChildren = spawnPointParent.GetComponentsInChildren<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void Update()
    {
        DestroyEnemyCount();
        timer += Time.deltaTime;

        if (timer >= spawnDelay)
        {
            SpawnEnemy();
            timer = 0.0f;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnEnemyOn)
        {
            randomSpawnPointNum = Random.Range(0, allChildren.Length);
            randomEnemyType = Random.Range(0, enemyTypeMaxRange);

            // Spawning enemies in random locations
            Instantiate(enemyTypes[randomEnemyType], allChildren[randomSpawnPointNum].transform.position, Quaternion.identity);
            enemyAmount++;
            if (enemyAmount == enemyAmountMax)
            {
                spawnEnemyOn = false;
            }
        }

    }

    private void DestroyEnemyCount ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            enemyAmount = 0;
            WaveSpawn();
            Debug.Log("Enemy amount = " + enemyAmount);
        }
    }

    private void WaveSpawn()
    { 
            enemyAmountMax *= 2;
            waveNumber++;
            spawnEnemyOn = true;
            //Debug.Log("Spawning wave " + waveNumber);
    }

}
