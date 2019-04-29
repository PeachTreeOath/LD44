using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField]
    private GameObject spawnPointParent;

    Transform[] allChildren;

    [SerializeField]
    private GameObject[] enemyTypes;

    private int enemyAmountMax = 3;
    private int enemyAmount = 0;
    private int enemyTypeMaxRange = 2;
    private int enemiesDead = 0;

    private int randomSpawnPointNum;
    private int randomEnemyType;

    [SerializeField]
    private bool spawnEnemyOn = true;

    [SerializeField]
    private bool inWave;
    private int waveNumber = 1;

    // timer
    public float spawnDelay = 3.0f;
    public float spawnTimer = 0.0f;
    public float waveDelay = 0.0f;
    public float waveTimer = 5.0f;

    [SerializeField]
    private float waveTimerReset = 5.0f;

    protected override void Awake()
    {
        base.Awake();
        allChildren = spawnPointParent.GetComponentsInChildren<Transform>();
        List<Transform> tempList = new List<Transform>(allChildren);

        Transform toRemove = null;
        foreach (Transform child in allChildren)
        {
            if (child.gameObject.GetInstanceID() == spawnPointParent.gameObject.GetInstanceID())
            {
                toRemove = child;
                break;
            }
        }
        tempList.Remove(toRemove);
        allChildren = tempList.ToArray();
    }

    private void Update()
    {
        DestroyEnemyCount();
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            SpawnEnemy();
            spawnTimer = 0.0f;
        }

        StartNewWave();
    }

    private void SpawnEnemy()
    {
        if (spawnEnemyOn)
        {
            randomSpawnPointNum = Random.Range(0, allChildren.Length);
            randomEnemyType = Random.Range(0, enemyTypes.Length);

            // Spawning enemies in random locations
            Instantiate(enemyTypes[randomEnemyType], allChildren[randomSpawnPointNum].transform.position, Quaternion.identity);
            enemyAmount++;
            Debug.Log("Number of enemies: " + enemyAmount);
            if (enemyAmount >= enemyAmountMax)
            {
                spawnEnemyOn = false;
            }
        }
    }

    public void NotifyEnemyDead()
    {
        enemiesDead++;
        // enemyAmount--;
        UITextManager.instance.SetEnemiesLeft(enemyAmountMax - enemiesDead);
    }

    private void DestroyEnemyCount()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            enemyAmount = 0;
            enemiesDead = enemyAmountMax;
            Debug.Log("Enemy amount = " + enemyAmount);
        }
    }

    private void StartNewWave()
    {
        if (enemiesDead >= enemyAmountMax)
        {
            waveTimer -= Time.deltaTime;
            //Debug.Log("Wave Delay countdown = " + waveTimer);

            if (waveTimer <= waveDelay)
            {
                WaveSpawn();
                waveTimer = 5.0f;
            }
        }
    }

    private void WaveSpawn()
    {
        enemyAmountMax += 2;
        waveNumber++;
        if (waveNumber == 11)
        {
            GameManager.instance.GotoVictoryScene();
            return;
        }
        spawnEnemyOn = true;
        spawnDelay -= 0.25f;
        enemyAmount = 0;
        enemiesDead = 0;

        UITextManager.instance.SetWave(waveNumber);
        UITextManager.instance.SetEnemiesLeft(enemyAmountMax);
        //Debug.Log("Spawning wave " + waveNumber);
    }

}
