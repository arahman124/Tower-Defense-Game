using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //Enums for the different states of the wave spawning
    public enum SpawnerState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };

    [System.Serializable]
    //Struct is like a class but allows for public access
    public struct Wave
    {
        //Constructor for wave numbers
        public Wave(int numSkeleton, int numGoblin, int numBoss, float spawnDelay)
        {
            NumSkeleton = numSkeleton;
            NumGoblin = numGoblin;
            NumBoss = numBoss;
            SpawnDelay = spawnDelay;
        }
        //Creates read only variables for each number of different types of monsters to spawn
        public readonly int NumSkeleton;
        public readonly int NumGoblin;
        public readonly int NumBoss;
        public readonly float SpawnDelay;
    }

    //Struct to hold the different monster stats that are changing with each wave
    public struct MonsterStats
    {
        //Constructor
        public MonsterStats(int health, float speed, int damage, int gold, int points)
        {
            Health = health;
            Speed = speed;
            Damage = damage;
            Gold = gold;
            Points = points;
        }

        public readonly int Health;
        public readonly float Speed;
        public readonly int Damage;
        public readonly int Gold;
        public readonly int Points;

    }


    // Hack to start at 0 
    //Variable for the current wave
    private int m_currentWave = -1;
    //Variable to hold the current number of skeletons left to spawn
    private int m_currentSkeletonCount;
    //Variable to hold the current number of goblins left to spawn
    private int m_currentGoblinCount;
    //Variable to hold the current number of boss monsters left to spawn
    private int m_currentBossCount;

    //List to hold the different number of monsters to spawn for each type in each wave
    private List<Wave> m_waves;
    //List of stats of the monsters to change to at each wave
    private List<MonsterStats> m_skeletonStats = new List<MonsterStats>();
    private List<MonsterStats> m_goblinStats = new List<MonsterStats>();
    private List<MonsterStats> m_bossStats = new List<MonsterStats>();

    //Variable for the spawn delay between monsters in a wave
    private float m_currentSpawnDelay;
    //Variable to hold the countdown between spawning a new monster - changing always
    private float m_spawnDelayCountdown;

    //Variable for holding the current state of the wave system - set to default counting
    public SpawnerState m_state = SpawnerState.COUNTING;

    //Variable to hold the tower object
    [SerializeField] private Tower Tower;

    //List of the different potential spawnpoints
    [SerializeField] private List<Transform> m_spawnPoints;
    //Reference to the player
    [SerializeField] private Player m_player;

    private void Start()
    {
        ReadWaveDataFromCSV();

        //m_skeletonStats = ReadStatsDataFromCSV(Enemy.EnemyType.Skeleton);
        //m_goblinStats = ReadStatsDataFromCSV(Enemy.EnemyType.Goblin);
        //m_bossStats = ReadStatsDataFromCSV(Enemy.EnemyType.Boss);
    }

    private void Update()
    {
        //If the wave system is not spawning then the rest of the update shouldnt be called - returns null
        if (m_state != SpawnerState.SPAWNING)
        {
            return;
        }

        //Condition for the spawn delay
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

                //Randomly spawns the different types from the object pooled queue
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
            //Checks if all monsters for the wave have been spawned
            if(HasEverythingSpawned())
            {
                m_state = SpawnerState.WAITING;
            }
        }
        else
        { 
            //If the previous statement isn't true then the game must be spawning the delay is counted down
            m_spawnDelayCountdown -= Time.deltaTime;
        }
    }
    //Called at start of waves
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

    //Method for finding the total monsters left to spawn in the wave
    public int GetMonsterCount()
    {
        return m_currentBossCount + m_currentGoblinCount + m_currentSkeletonCount;
    }

    //Checks if all monsters have been spawned or not
    public bool HasEverythingSpawned()
    {
        return m_currentBossCount == 0 && m_currentSkeletonCount == 0 && m_currentGoblinCount == 0;
    }

    //File reading for the Wave CSV
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

    //Spawns an enemy - type is passed into parameters
    private void SpawnEnemy(Enemy.EnemyType enemyType)
    {
        //picks a random spawnpoint randomly from the list of spawnpoints
        Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        //Outputs the spawnpoint that the monster was spawned from - testing
        Debug.Log(spawnPoint.gameObject.name);

        GameObject enemyObject = null;

        //Grabs the required enemy type from the queued objects (pool)
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

        //Safety check
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

    //Method for returning the next wave number
    public int GetWaveCount()
    {
        return m_currentWave + 1;
    }

    private List<MonsterStats> ReadStatsDataFromCSV(Enemy.EnemyType typeOfMonster)
    {
        List<MonsterStats> stats = new();
        string fileToOpen = typeOfMonster switch
        {
            Enemy.EnemyType.Skeleton => "SkeletonStats",
            Enemy.EnemyType.Goblin => "Goblin",
            Enemy.EnemyType.Boss => "BossStats",
            _ => "SkeletonStats"
        };

        TextAsset movesFile = (TextAsset)Resources.Load(fileToOpen);
        string[] linesFromFile = movesFile.text.Split('\n');

        for (int i = 1; i < linesFromFile.Length; ++i)
        {
            // Split the line into an array based on the commas
            string[] contents = linesFromFile[i].Split(',');

            int health = int.Parse(contents[1]);
            float speed = float.Parse(contents[2]);
            int damage = int.Parse(contents[3]);
            int gold = int.Parse(contents[4]);
            int points = int.Parse(contents[5].TrimEnd('\r'));


            stats.Add(new MonsterStats(health, speed, damage, gold, points));
        }
        return stats;
    }
}
