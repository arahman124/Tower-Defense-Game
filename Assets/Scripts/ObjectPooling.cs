using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    [System.Serializable]
    //Class template for each pool that can be created - contains all relevant variables
    public class Pool
    {
        //tag variable holding name of the monster pool
        public string tag;
        //The prefab object that will be placed in the pool
        public GameObject prefab;
        //Variable holding size of the pool
        public int size;
    }

    //A list of pools
    public List<Pool> pools;
    //A dictionary that holds the different pools
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    private static ObjectPooling Instance = null;

    public static ObjectPooling getInstance()
    {
        
        return Instance;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            RegenPools();
        }
    }
    
    #endregion


    //Start is called before the first frame update
    void Start()
    {
        RegenPools();
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        //Testing check to see if the pool tag does exist
        if (!poolDictionary.ContainsKey(tag))
        {
            //If not then the error is displayed to console
            Debug.LogWarning("Pool with tag" + tag + "doesn't exist.");
            Debug.DebugBreak();
            return null;
        }

        //A individual object within the queue is accessed
        GameObject objectToSpawn = poolDictionary[tag].dequeue().Data;

        //Sets the object as active - visible on scene
        objectToSpawn.SetActive(true);
        //Gives the objects correct spawn positions - to be changed
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        //Adds the object back to the queue such that it can recycled
        poolDictionary[tag].enqueue(objectToSpawn);

        //Just a safety procedure to return the individual object enemy
        return objectToSpawn;
    }

    private void RegenPools()
    {
        //Generates a new dictionary to hold the pools
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //for loop that iterates through each individual pool in the list of pools
        foreach (Pool pool in pools)
        {
            //Creates a queue for the different pooled objects to be created and added
            Queue<GameObject> objectPool = new Queue<GameObject>();
            //List<GameObject> pooledObjects = new List<GameObject>();

            //iterates through the individual pool to the number of times in size
            for (int i = 0; i < pool.size; i++)
            {
                //Creates the object (enemy)
                GameObject obj = Instantiate(pool.prefab);

                obj.name = $"{pool.tag}_{i}";

                //Sets the object as an inactive object - invisible
                obj.SetActive(false);

                //Adds the object to the queue of pooledObjects
                objectPool.enqueue(obj);
                //pooledObjects.Add(obj);
            }
            //The pool of instantiated objects are kept in the queue stored in the dictionary
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

}
