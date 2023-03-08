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

        readonly int NumSkeleton;
        readonly int NumGoblin;
        readonly int NumBoss;
        readonly float SpawnDelay;
    }

    private int m_currentWave;

    private List<Wave> m_waves;

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    public SpawnerState state = SpawnerState.COUNTING;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        ReadWaveDataFromCSV();
    }

    private void Update()
    {
        if (waveCountdown <= 0)
        {
            if (state != SpawnerState.SPAWNING)
            {
                //Start the wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnerState.SPAWNING;

        //spawn

        state = SpawnerState.WAITING;


        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //Spawn enemy - object pooling implementation occurs here
        Debug.Log("Spawning Enemy");

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
}
