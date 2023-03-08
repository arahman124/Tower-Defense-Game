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
    public class Wave
    {
        //Name of type of wave - boss or normal
        public string waveName;
        //position of spawned
        public Transform enemy;
        //shouldn't be necessary while using object pooling - number of monsters to be spawned
        public int count;
        //Might need to be moved to object pooling script - dictates rate that monsters spawn in
        public float spawnRate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    public SpawnerState state = SpawnerState.COUNTING;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
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



}
