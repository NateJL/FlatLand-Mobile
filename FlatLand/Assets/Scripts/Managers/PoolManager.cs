using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameManager manager;

    public bool isReady = false;

    [Space(20)]
    [Header("Pool Parents")]
    public Transform NpcPoolParent;
    public Transform itemPoolParent;
    public Transform uiPoolParent;

    public List<Pool> pools;
    public static Dictionary<string, List<GameObject>> poolDictionary;
    public static Dictionary<string, List<ElementUI>> poolDictionaryUI;
    public static Dictionary<string, List<GameObject>> poolDictionaryNPC;
    public static Dictionary<string, GameObject> poolPrefabs;

    /// <summary>
    /// Function called by the GameManager to initialize the object pool before starting the game.
    /// </summary>
    public void Initialize(GameManager manager)
    {
        Debug.Log("Initialize Pool Manager...");
        isReady = false;
        this.manager = manager;
        poolDictionary = new Dictionary<string, List<GameObject>>();
        poolDictionaryUI = new Dictionary<string, List<ElementUI>>();
        poolDictionaryNPC = new Dictionary<string, List<GameObject>>();
        poolPrefabs = new Dictionary<string, GameObject>();

        foreach (Pool pool in pools)
        {
            List<GameObject> objectPool = new List<GameObject>();
            List<ElementUI> objectPoolUI = new List<ElementUI>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.prefab.name;

                switch (pool.objType)
                {
                    case Pool.ObjectPoolType.UI:
                        objectPoolUI.Add(new ElementUI(false, obj));
                        obj.transform.SetParent(uiPoolParent);
                        break;

                    case Pool.ObjectPoolType.NPC:
                        objectPool.Add(obj);
                        obj.transform.SetParent(NpcPoolParent);
                        break;

                    case Pool.ObjectPoolType.None:
                    default:
                        objectPool.Add(obj);
                        obj.transform.SetParent(itemPoolParent);
                        break;
                }

                obj.SetActive(false);
            }

            switch (pool.objType)
            {
                case Pool.ObjectPoolType.UI:
                    poolDictionaryUI.Add(pool.objName, objectPoolUI);
                    poolPrefabs.Add(pool.objName, pool.prefab);
                    break;

                case Pool.ObjectPoolType.NPC:
                case Pool.ObjectPoolType.None:
                default:
                    poolDictionary.Add(pool.objName, objectPool);
                    poolPrefabs.Add(pool.objName, pool.prefab);
                    break;
            }
        }
        isReady = true;
        Debug.Log("Pool Manager Initialized!");
    }

    /// <summary>
    /// Spawns a UI object from the pool of object given the parameters.
    /// </summary>
    public ElementUI SpawnObjectUI(string objName, Vector3 position, Quaternion rotation, Transform newParent)
    {
        GameObject objectToSpawn = null;
        if (!poolDictionaryUI.ContainsKey(objName))
        {
            Debug.LogWarning("Pool with tag: " + objName + " doesn't exist");
            return new ElementUI(false, objectToSpawn);
        }

        List<ElementUI> objectPool = poolDictionaryUI[objName];

        foreach (ElementUI obj in objectPool)
        {
            if (!obj.isCurrentlyActive)
            {
                objectToSpawn = obj.element;
                obj.isCurrentlyActive = true;
                objectToSpawn.SetActive(true);
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                objectToSpawn.transform.SetParent(newParent, false);
                return obj;
            }
        }

        if (objectToSpawn == null)
        {
            objectToSpawn = Instantiate(poolPrefabs[objName]);
            objectToSpawn.name = poolPrefabs[objName].name;
            ElementUI newElement = new ElementUI(true, objectToSpawn);
            objectPool.Add(newElement);
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.transform.SetParent(newParent, false);
            return newElement;
        }

        return new ElementUI(false, objectToSpawn);
    }

    /// <summary>
    /// Takes the given UI object parameter and returns it to the pool.
    /// </summary>
    public void ReturnObjectUI(ElementUI obj)
    {
        obj.isCurrentlyActive = false;
        obj.element.SetActive(false);
        obj.element.transform.SetParent(uiPoolParent);
        obj.element.transform.position = Vector3.zero;
        obj.element.transform.rotation = Quaternion.identity;
        obj.element.transform.localScale = new Vector3(1, 1, 1);
    }

}


[System.Serializable]
public class Pool
{
    public enum ObjectPoolType
    {
        None = 0,
        UI,
        NPC
    }

    public string objName;
    public GameObject prefab;
    public ObjectPoolType objType = ObjectPoolType.None;
    public int size;

    public Pool(string objName, GameObject prefab, ObjectPoolType objType, int size)
    {
        this.objName = objName;
        this.prefab = prefab;
        this.objType = objType;
        this.size = size;
    }
}

[System.Serializable]
public class ElementUI
{
    public bool isCurrentlyActive;
    public GameObject element;

    public ElementUI(bool startActive, GameObject obj)
    {
        isCurrentlyActive = startActive;
        element = obj;
    }
}
