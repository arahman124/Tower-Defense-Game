using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Variable to hold the tower object
    [SerializeField] private GameObject Tower;
    //Variable to hold list of enemy types that could be spawned
    [SerializeField] private List<GameObject> m_enemyList;
    //List of the different potential spawnpoints
    [SerializeField] private List<Transform> m_spawnPoints;
    //A variable for the time between each spawning
    [SerializeField] private float spawnDelay;
    //Boolean variable for when an individual monster is spawned
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
        //Condition that hasSpawned is false
        if (!hasSpawned)
        {
            //Initates a coroutine for the Ienumerator that calls the SpawnEnemy function as a loop
            StartCoroutine("SpawnEnemy");
        }


    }


    //IEnumerator that is the function for spawning the enemy - not used conventionally but rather to loop the code
    private IEnumerator SpawnEnemy()
    {
        //picks a random spawnpoint randomly from the list of spawnpoints
        Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
        //Outputs the spawnpoint that the monster was spawned from - testing
        Debug.Log(spawnPoint.gameObject.name);
        //This is the variable for the enemy being spawned - passes in the random enemy type, spawnpoint position in form [x,y,z] and the rotation
        //GameObject enemy = Instantiate(m_enemyList[Random.Range(0, m_enemyList.Count)], spawnPoint.position, Quaternion.identity);
        GameObject enemy = ObjectPooling.getInstance().SpawnFromPool("Default", spawnPoint.position, Quaternion.identity);
        //Uses the method from the Enemy Script to make the enemy move towards the tower
        enemy.GetComponent<Enemy>().SetTarget(Tower.transform);
        //Sets the variable to true
        hasSpawned = true;
        //Sets a delay before the function is called again
        yield return new WaitForSeconds(spawnDelay);
        //Sets the variable back to false
        hasSpawned = false;
    }


    //private IEnumerator SpawnEnemy()
    //{
    //    GameObject enemy = ObjectPool.instance.GetPooledObject();

    //    if (enemy != null)
    //    {
    //        Transform spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)];
    //        enemy.transform.position = spawnPoint.position;
    //        enemy.GetComponent<Enemy>().SetTarget(Tower.transform);
    //        hasSpawned = true;
    //        yield return new WaitForSeconds(spawnDelay);
    //        hasSpawned = false;
    //    }
    //}
}
