using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    public GameManager manager;
    public Material polygonMaterial;

    [Header("Only used for non-player entities")]
    public int numberOfEdges = 4;
    [Space(20)]
    public Vector3[] vertices;
    public int[] triangles;

    public bool showGizmos = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMaterials(Material newMaterial)
    {
        polygonMaterial = newMaterial;
    }

    public void GenerateFullPolygon(int numberOfVertices, float scale)
    {
        CalculatePolygonVertices(numberOfVertices, scale);
        GeneratePolygon(polygonMaterial);
    }

    public void GeneratePolygon(Material material)
    {
        if (GetComponent<MeshFilter>() != null)
            DestroyImmediate(GetComponent<MeshFilter>());
        if (GetComponent<MeshRenderer>() != null)
            DestroyImmediate(GetComponent<MeshRenderer>());

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }

    public void ClearPolygon()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();
        if(GetComponent<MeshFilter>() != null)
            DestroyImmediate(GetComponent<MeshFilter>());
        if (GetComponent<MeshRenderer>() != null)
            DestroyImmediate(GetComponent<MeshRenderer>());
    }

    /// <summary>
    /// Calculate the vertices for a regular polygon with (n+1) vertices, accounting for the origin.
    /// </summary>
    public void CalculatePolygonVertices(int numberOfVertices, float scale)
    {
        vertices = new Vector3[numberOfVertices];
        vertices[0] = transform.InverseTransformPoint(transform.position);
        Vector2 tempVert = Random.insideUnitCircle.normalized * scale;
        vertices[1] = new Vector3(tempVert.x, transform.position.y, tempVert.y);

        GameObject tempObj = new GameObject("tempObj");
        tempObj.transform.position = vertices[1];
        for(int i = 2; i < numberOfVertices; i++)
        {
            tempObj.transform.RotateAround(vertices[0], Vector3.up, 360 / (numberOfVertices - 1));
            vertices[i] = new Vector3(tempObj.transform.position.x, tempObj.transform.position.y, tempObj.transform.position.z);
        }
        SafeDestroy(tempObj);

        triangles = new int[3 * (vertices.Length - 1)];

        int counter = 0;
        for (int i = counter; i < triangles.Length; i+=3)
        {
            triangles[i] = 0;
        }
        counter = 1;
        for (int i = counter; i < triangles.Length; i += 3)
        {
            triangles[i] = counter;
            counter++;
        }
        counter = 2;
        for (int i = counter; i < triangles.Length; i += 3)
        {
            if (i >= triangles.Length - 1)
                triangles[i] = 1;
            else
            {
                triangles[i] = counter;
                counter++;
            }
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

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            Gizmos.color = Color.blue;
            for(int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(transform.position + vertices[i], 0.1f);
            }
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
