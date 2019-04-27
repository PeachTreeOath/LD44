﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
   [SerializeField]
   private GameObject[] spawnPoints;

   [SerializeField]
   private GameObject[] enemyTypes;

   [SerializeField]
   private float spawnWaitTime = 8.0f;

   [SerializeField]
   private float waveWaitTime = 20.0f;

   [SerializeField]
   private int enemyAmount = 2;
   private int enemyTypeMaxRange = 0;

   private int randomSpawnPointNum;
   private int randomEnemyType;
   [SerializeField]
   private bool spawnEnemyOn = true;
   [SerializeField]
   private bool waveSpawnOn = true;
   private int waveNumber = 1;

   // Start is called before the first frame update
   void Start()
   {
      
   }

   private void Update()
   {
      WaveSpawn(); 
   }

   private void SpawnEnemy()
   {
      if (spawnEnemyOn)
      {
         randomSpawnPointNum = Random.Range(0, spawnPoints.Length);
         randomEnemyType = Random.Range(0, enemyTypeMaxRange);
         for (int i = 0; i < enemyAmount; i++)
         {
            Instantiate(enemyTypes[randomEnemyType], spawnPoints[randomSpawnPointNum].transform.position, Quaternion.identity);
         }
         spawnEnemyOn = false;
         StartCoroutine(SpawnPowerUpRoutine());
      }

   }

   private void WaveSpawn()
   {
      if (waveSpawnOn)
      {
         SpawnEnemy();
      }

      if (enemyAmount == 0)
      {
         waveSpawnOn = false;
         StartCoroutine(WavePowerUpRoutine());
      }

      if (!waveSpawnOn)
      {
         waveNumber++;
      }
   }

   private IEnumerator SpawnPowerUpRoutine()
   {
      yield return new WaitForSeconds(spawnWaitTime);
      spawnEnemyOn = true;
   }

   private IEnumerator WavePowerUpRoutine()
   {
      yield return new WaitForSeconds(waveWaitTime);
      waveSpawnOn = true;
   }

}
