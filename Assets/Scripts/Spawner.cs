using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   
    [SerializeField] private List<GameObject> m_enemyList;
    [SerializeField] private List<Transform> m_spawnPoints;
    [SerializeField] private float spawnDelay;
    private bool hasSpawned;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned)
        {
            StartCoroutine("SpawnEnemy");
        }
        
    }



    private IEnumerator SpawnEnemy()
    {
        Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        Debug.Log(spawnPoint.gameObject.name);
        Instantiate(m_enemyList[Random.Range(0, m_enemyList.Count)], spawnPoint.position, Quaternion.identity);
        hasSpawned = true;
        yield return new WaitForSeconds(spawnDelay);
        hasSpawned = false;
    }
}
