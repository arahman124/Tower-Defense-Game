using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public enum SpawnerState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };

    [System.Serializable]
    public struct Wave
    {
        public Wave(int numSkeleton, int numGoblin, int numBoss, float spawnDelay)
        {
            NumSkeleton = numSkeleton;
            NumGoblin = numGoblin;
            NumBoss = numBoss;
            SpawnDelay = spawnDelay;
        }

        public readonly int NumSkeleton;
        public readonly int NumGoblin;
        public readonly int NumBoss;
        public readonly float SpawnDelay;
    }

    // Hack to start at 0 
    private int m_currentWave = -1;
    private int m_currentSkeletonCount;
    private int m_currentGoblinCount;
    private int m_currentBossCount;

    private List<Wave> m_waves;

    private float m_currentSpawnDelay;
    private float m_spawnDelayCountdown;

    public SpawnerState m_state = SpawnerState.COUNTING;

    //Variable to hold the tower object
    [SerializeField] private Tower Tower;

    //List of the different potential spawnpoints
    [SerializeField] private List<Transform> m_spawnPoints;

    [SerializeField] private Player m_player;

    private void Start()
    {
        ReadWaveDataFromCSV();
    }

    private void Update()
    {
        if (m_state != SpawnerState.SPAWNING)
        {
            return;
        }

        if (m_spawnDelayCountdown <= 0)
        {
            if (m_currentBossCount > 0 || m_currentSkeletonCount > 0 || m_currentGoblinCount > 0)
            {
                // If we still have stuff to spawn, then we want to reset the timer
                m_spawnDelayCountdown = m_currentSpawnDelay;
            }

            //Start the wave
            while(m_currentBossCount > 0 || m_currentSkeletonCount > 0 || m_currentGoblinCount > 0)
            {
                int random = Random.Range(0, 3);

                if((Enemy.EnemyType)random == Enemy.EnemyType.Boss)
                {
                    if(m_currentBossCount > 0)
                    {
                        m_currentBossCount--;
                        SpawnEnemy(Enemy.EnemyType.Boss);
                        break;
                    }
                }
                else if((Enemy.EnemyType)random == Enemy.EnemyType.Goblin)
                {
                    if (m_currentGoblinCount > 0)
                    {
                        m_currentGoblinCount--;
                        SpawnEnemy(Enemy.EnemyType.Goblin);
                        break;
                    }
                }
                else
                {
                    if (m_currentSkeletonCount > 0)
                    {
                        m_currentSkeletonCount--;
                        SpawnEnemy(Enemy.EnemyType.Skeleton);
                        break;
                    }
                }
            }

            if(HasEverythingSpawned())
            {
                m_state = SpawnerState.WAITING;
            }
        }
        else
        {
            m_spawnDelayCountdown -= Time.deltaTime;
        }
    }

    public void OnStartWave()
    {
        m_currentWave++;

        m_state = SpawnerState.SPAWNING;

        Wave newWave = m_waves[m_currentWave];

        m_currentSkeletonCount = newWave.NumSkeleton;
        m_currentGoblinCount = newWave.NumGoblin;
        m_currentBossCount = newWave.NumBoss;
        m_currentSpawnDelay = newWave.SpawnDelay;
        m_spawnDelayCountdown = m_currentSpawnDelay;
    }

    public int GetMonsterCount()
    {
        return m_currentBossCount + m_currentGoblinCount + m_currentSkeletonCount;
    }

    public bool HasEverythingSpawned()
    {
        return m_currentBossCount == 0 && m_currentSkeletonCount == 0 && m_currentGoblinCount == 0;
    }

    private void ReadWaveDataFromCSV()
    {
        m_waves = new List<Wave>();

        TextAsset movesFile = (TextAsset)Resources.Load("waves");
        string[] linesFromFile = movesFile.text.Split('\n');

        for (int i = 1; i < linesFromFile.Length; ++i)
        {
            // Split the line into an array based on the commas
            string[] contents = linesFromFile[i].Split(',');

            int numSkeletons = int.Parse(contents[1]);
            int numGoblins = int.Parse(contents[2]);
            int numBosses = int.Parse(contents[3]);
            float spawnDelay = float.Parse(contents[4].TrimEnd('\r'));


            m_waves.Add(new Wave(numSkeletons, numGoblins, numBosses, spawnDelay));
        }
    }

    private void SpawnEnemy(Enemy.EnemyType enemyType)
    {
        //picks a random spawnpoint randomly from the list of spawnpoints
        Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        //Outputs the spawnpoint that the monster was spawned from - testing
        Debug.Log(spawnPoint.gameObject.name);

        GameObject enemyObject = null;

        switch (enemyType)
        {
            case Enemy.EnemyType.Boss:
                enemyObject = ObjectPooling.getInstance().SpawnFromPool(Constants.BOSS_TAG, spawnPoint.position, Quaternion.identity);
                break;
            case Enemy.EnemyType.Goblin:
                enemyObject = ObjectPooling.getInstance().SpawnFromPool(Constants.GOBLIN_TAG, spawnPoint.position, Quaternion.identity);
                break;
            case Enemy.EnemyType.Skeleton:
                enemyObject = ObjectPooling.getInstance().SpawnFromPool(Constants.SKELETON_TAG, spawnPoint.position, Quaternion.identity);
                break;
        }

        if (enemyObject == null)
        {
            Debug.Log("EEK!");
        }

        //GameObject enemy = ObjectPooling.getInstance().SpawnFromPool("Default", spawnPoint.position, Quaternion.identity);
        //Uses the method from the Enemy Script to make the enemy move towards the tower

        // TODO: Read in from another file for the stats per monster per wave, pass this info into Reset
        // Either do it as a struct or as separate floats 
        enemyObject.GetComponent<Enemy>().Reset();
        enemyObject.GetComponent<Enemy>().SetTarget(Tower);
        enemyObject.GetComponent<Enemy>().SetPlayerRef(m_player);
    }

    public int GetWaveCount()
    {
        return m_currentWave + 1;
    }
}
