using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject Tower;
    [SerializeField] private List<GameObject> m_enemyList;
    [SerializeField] private List<Transform> m_spawnPoints;
    [SerializeField] private float spawnDelay;
    private bool hasSpawned;

    ObjectPooling ObjectPooling;

    private void Start()
    {
        //ObjectPooling = ObjectPooling.Instance;
        //ObjectPooling.SpawnFromPool("Default", transform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        //ObjectPooling.SpawnFromPool("Default", transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!hasspawned)
        //{
        //    startcoroutine("SpawnEnemy");
        //}

        if (!hasSpawned)
        {
            StartCoroutine("SpawnEnemy");
        }


    }



    //private IEnumerator SpawnEnemy()
    //{
    //    //Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
    //    //Debug.Log(spawnPoint.gameObject.name);
    //    //GameObject enemy = Instantiate(m_enemyList[Random.Range(0, m_enemyList.Count)], spawnPoint.position, Quaternion.identity);
    //    //enemy.GetComponent<Enemy>().SetTarget(Tower.transform);
    //    hasSpawned = true;
    //    yield return new WaitForSeconds(spawnDelay);
    //    hasSpawned = false;
    //}

 
    private IEnumerator SpawnEnemy()
    {
        GameObject enemy = ObjectPool.instance.GetPooledObject();

        if (enemy != null)
        {
            Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
            enemy.transform.position = spawnPoint.position;
            enemy.GetComponent<Enemy>().SetTarget(Tower.transform);
            hasSpawned = true;
            yield return new WaitForSeconds(spawnDelay);
            hasSpawned = false;
        }
    }
}
