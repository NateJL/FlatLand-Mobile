using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorldEditor : MonoBehaviour
{
    public GameObject objParent;
    public GameObject selectedPrefab;
    [Space(20)]
    public GameObject currentGameObject;
    public Vector3 position;
    public Vector3 rotation;
    [Space(20)]
    public List<PlaceableObject> prefabs;

    public GameObject SpawnPrefab(int index)
    {
        if (index < prefabs.Count)
            selectedPrefab = prefabs[index].prefab;

        if (selectedPrefab != null)
        {
            Debug.Log("Spawning Prefab: " + prefabs[index].tag);
            //GameObject newPrefab = PrefabUtility.InstantiatePrefab(selectedPrefab as GameObject) as GameObject;
            GameObject newPrefab = Instantiate(selectedPrefab as GameObject) as GameObject;
            currentGameObject = newPrefab;

            if (objParent != null)
                newPrefab.transform.SetParent(objParent.transform);
            else
                Debug.LogWarning("PrefabPlacer: no parent for spawned GameObject.");

            newPrefab.transform.localPosition = position;
            newPrefab.transform.localEulerAngles = rotation;
        }

        return currentGameObject;
    }

    /// <summary>
    /// Safetly destroy object in edit mode.
    /// </summary>
    public static T SafeDestroy<T>(T obj) where T : Object
    {
        if (Application.isEditor)
            Object.DestroyImmediate(obj);
        else
            Object.Destroy(obj);

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(position, 0.25f);
    }
}

[System.Serializable]
public class PlaceableObject
{
    public string tag;
    public GameObject prefab;

    public PlaceableObject(string newTag, GameObject newObj)
    {
        tag = newTag;
        prefab = newObj;
    }
}
