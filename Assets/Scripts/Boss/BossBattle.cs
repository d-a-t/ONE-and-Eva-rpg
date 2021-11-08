using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using CodeMonkey Boss fight in Unity
// see end of video for Boss Battle Events (like key, door, etc)

public class BossBattle : MonoBehaviour
{
    // stages
    public enum Stage
    {
        WaitingToStart,
        Stage_1,
        Stage_2,
        Stage_3,
    }

    [SerializeField] private BossTrigger bossTrigger;
    //[SerializeField] private EnemySpawn pfEnemySpawn;
    //[SerializeField] private EnemyTurretLogic enemyTurret;

    //private List<EnemySpawn> enemySpawnList;      // for every time we spawn an enemy
    private List<Vector3> spawnPointList;
    private Stage stage;

    // random enemy spawnpoint
    private void Awake()
    {
        spawnPointList = new List<Vector3>();

        foreach (Transform spawnPoint in transform.Find("spawnPoints"))
        {
            spawnPointList.Add(spawnPoint.position);
        }

        stage = Stage.WaitingToStart;
    }

    private void Start()
    {
        bossTrigger.OnPlayerEnterTrigger += BossTrigger_OnPlayerEnterTrigger;

        /*
        enemyTurret.GetHealthSystem().OnDamaged += BossBattle_OnDamaged;
        enemyTurret.GetHealthSystem().OnDead += BossBattle_OnDead;
        */
    }

    private void BossBattle_OnDead(object sender, System.EventArgs e)
    {
        // boss dead!!
        // DestroyAllEnemies();
    }

    private void BossBattle_OnDamaged(object sender, System.EventArgs e)
    {
        // boss took damage
        switch (stage)
        {
            case Stage.Stage_1:
                /*if (enemyTurret.GetHealthSystem().GetHealthNormalized() <= .7f)
                {
                    // if the boss is under 70% health
                    StartNextStage();
                }*/
                break;
            case Stage.Stage_2:
                /*if (enemyTurret.GetHealthSystem().GetHealthNormalized() <= .3f)
                {
                    // if the boss is under 30% health
                    StartNextStage();
                }*/
                break;
        }
    }

    private void BossTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e)
    {
        StartBattle();
        bossTrigger.OnPlayerEnterTrigger -= BossTrigger_OnPlayerEnterTrigger;   // so we only start once
    }
    private void StartBattle()
    {
        Debug.Log("StartBattle");
        StartNextStage();
        stage = Stage.Stage_1;
        // SpawnEnemy();            // modify code to spawn enemies only at certain times
    }

    private void StartNextStage()
    {
        switch(stage)
        {
            case Stage.WaitingToStart:
                stage = Stage.Stage_1;
                break;
            case Stage.Stage_1:
                stage = Stage.Stage_2;
                break;
            case Stage.Stage_2:
                stage = Stage.Stage_3;
                break;
        }
    }

    /*
    private void SpawnEnemy()
    {
        // limit the amount of enemies that can be spawned
        int aliveCount = 0;
        foreach (EnemySpawn enemySpawn in enemySpawnList)
        {  
            if (enemySpawned.IsAlive())
            {
                aliveCount++;
                if (aliveCount >= 6)
                    return;             // stop spawning enemies
            }
        }

        Vector3 spawnPosition = spawnPositionList[Random.Range(0, spawnPositionList.Count)];

        EnemySpawn enemySpawn = Instantiate(pfEnemySpawn, spawnPosition, Quaternion.identity);
        enemySpawn.Spawn();

        enemySpawnList.Add(enemySpawn);
    }

    private void DestroyAllEnemies()
    {
        foreach (EnemySpawn enemySpawn in enemySpawnList)
        {
            if (enemySpawn.IsAlive())
            {
                enemySpawn.KillEnemy();
            }
        }
    }
    */
}
