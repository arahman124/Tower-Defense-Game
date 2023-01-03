using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooling Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            //Queue<GameObject> objectPool = new Queue<GameObject>();
            List<GameObject> pooledObjects = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                //objectPool.Enqueue(obj);
                pooledObjects.Add(obj);
            }

            poolDictionary.Add(pool.tag, pooledObjects);
        }
    }

    //public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    //{
    //    if (!poolDictionary.ContainsKey(tag))
    //    {
    //        Debug.LogWarning("Pool with tag" + tag + "doesn't exist.");
    //        return null;
    //    }
    //    GameObject objectToSpawn = poolDictionary[tag].Dequeue();

    //    objectToSpawn.SetActive(true);
    //    objectToSpawn.transform.position = position;
    //    objectToSpawn.transform.rotation = rotation;

    //    poolDictionary[tag].Enqueue(objectToSpawn);

    //    return objectToSpawn;
    //}

    private void RegenPools()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools)
        {
            //Queue<GameObject> objectPool = new Queue<GameObject>();
            List<GameObject> pooledObjects = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                //objectPool.Enqueue(obj);
                pooledObjects.Add(obj);
            }

            poolDictionary.Add(pool.tag, pooledObjects);
        }
    }

}
