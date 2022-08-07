using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolExample : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private IObjectPool<GameObject> pool;
    [SerializeField] private Vector3 position;
    private bool collectionChecks = true;
    private int maxPoolSize = 3;
    private int currentsize;
    private List<GameObject> listOfCoins;

    private void Start()
    {
        listOfCoins = new List<GameObject>();
    }
    public IObjectPool<GameObject> Pool 
    {
        get
        {
            if (pool == null) 
            {
               pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
            }
            return pool;

        }
    }

    GameObject CreatePooledItem() 
    {
        GameObject newPooledItem = Instantiate(objectToPool, position, Quaternion.identity);
        
        return newPooledItem;
     
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject pullGameObject)
    {
        pullGameObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject pullGameObject)
    {
        pullGameObject.SetActive(true);
        listOfCoins.Add(pullGameObject);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GameObject pullGameObject)
    {
        Destroy(pullGameObject);
    }

    public void On_GetButtonPressed() 
    {
        Pool.Get();
    }
    public void On_ReleaseButtonPressed()
    {
        if (listOfCoins.Count > 0) 
        {


            GameObject obj = listOfCoins[0];
            listOfCoins.Remove(obj);
            pool.Release(obj);
            
        }
    }
}
