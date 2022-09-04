using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class FenceEditorController : MonoBehaviour
{
    public GameObject fencePrefab;
    public GameObject fenceAnglePrefab;

    public List<GameObject> fences;

    public Vector3 currentFenceStartPos;
    public Vector3 currentFenceEndPos;
    public Vector3 currentFenceRot;

    public int currentFenceIndex = 0;

    public bool showGizmos = false;

    /// <summary>
    /// Get the current children fences in parent controller.
    /// </summary>
    public void GetCurrentFences()
    {
        if (fences == null)
            fences = new List<GameObject>();

        fences.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            fences.Add(transform.GetChild(i).gameObject);
        }

        if(fences.Count > 0)
        {
            currentFenceIndex = fences.Count - 1;
            currentFenceStartPos = fences[currentFenceIndex].GetComponent<FenceController>().startTransform.position;
            currentFenceRot = fences[currentFenceIndex].GetComponent<FenceController>().startTransform.rotation.eulerAngles;
            currentFenceEndPos = fences[currentFenceIndex].GetComponent<FenceController>().endTransform.position;
        }
    }

    public void AddFence()
    {
        if (fencePrefab != null)
        {
            Debug.Log("Spawning Prefab: " + fencePrefab.name);
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(fencePrefab as GameObject) as GameObject;
            //GameObject newPrefab = Instantiate(selectedPrefab as GameObject) as GameObject;

            newPrefab.transform.position = fences[currentFenceIndex].GetComponent<FenceController>().endTransform.position;
            newPrefab.transform.rotation = fences[currentFenceIndex].GetComponent<FenceController>().startTransform.rotation;
            newPrefab.transform.SetParent(transform);
            GetCurrentFences();
        }
    }

    public void AddAngleFence()
    {
        if(fenceAnglePrefab != null)
        {

            Debug.Log("Spawning Angled Prefab: " + fenceAnglePrefab.name);
            GameObject newPrefab = PrefabUtility.InstantiatePrefab(fenceAnglePrefab as GameObject) as GameObject;

            newPrefab.transform.position = fences[currentFenceIndex].GetComponent<FenceController>().endTransform.position;
            newPrefab.transform.rotation = fences[currentFenceIndex].GetComponent<FenceController>().startTransform.rotation;
            newPrefab.transform.SetParent(transform);
            GetCurrentFences();
        }
    }

    public void RemoveFence()
    {
        if(fences.Count > 1 && fences.Count > currentFenceIndex)
        {
            int tempIndex = currentFenceIndex;
            currentFenceIndex -= 1;
            if(currentFenceIndex < 0)
            {
                currentFenceIndex = 0;
            }
            GameObject removedObj = fences[tempIndex];
            removedObj.transform.SetParent(null);
            GetCurrentFences();
            SafeDestroy(removedObj);
        }
        else
        {
            Debug.Log("Error: Must have at least 1 fence in parent.");
        }
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

    private void OnValidate()
    {
        currentFenceStartPos = fences[currentFenceIndex].GetComponent<FenceController>().startTransform.position;
        currentFenceRot = fences[currentFenceIndex].GetComponent<FenceController>().startTransform.rotation.eulerAngles;
        currentFenceEndPos = fences[currentFenceIndex].GetComponent<FenceController>().endTransform.position;
    }

    private void OnDrawGizmosSelected()
    {
        float heightScale = 0.3f;
        if(showGizmos)
        {
            Gizmos.color = Color.blue;
            if (270 - Mathf.Abs(fences[currentFenceIndex].transform.rotation.eulerAngles.y) < 1 || 90 - Mathf.Abs(fences[currentFenceIndex].transform.rotation.eulerAngles.y) < 1)
                Gizmos.DrawWireCube(fences[currentFenceIndex].GetComponent<FenceController>().midTransform.position + new Vector3(0, heightScale, 0), new Vector3(5f, 1f, 1f));
            else
                Gizmos.DrawWireCube(fences[currentFenceIndex].GetComponent<FenceController>().midTransform.position + new Vector3(0, heightScale, 0), new Vector3(1f, 1f, 5));

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(fences[currentFenceIndex].GetComponent<FenceController>().startTransform.position + new Vector3(0, heightScale, 0), 0.1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(fences[currentFenceIndex].GetComponent<FenceController>().endTransform.position + new Vector3(0, heightScale, 0), 0.1f);
            
        }
    }
}

