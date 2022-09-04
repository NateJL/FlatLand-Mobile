using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class IsoscelesController : MonoBehaviour
{
    public Material polygonMaterial;

    public Vector3[] vertices;
    public int[] triangles;
    

    private void Start()
    {
        InitializePolygonModel();
    }

    // Start is called before the first frame update
    public void InitializePolygonModel()
    {
        vertices = new Vector3[3];
        vertices[0] = new Vector3(0, 0.01f, 1.0f);
        vertices[1] = new Vector3(0.5f, 0.01f, -0.5f);
        vertices[2] = new Vector3(-0.5f, 0.01f, -0.5f);

        triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        ClearMesh();
        GetComponent<MeshRenderer>().sharedMaterial = polygonMaterial;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void ClearMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if(mesh != null)
            mesh.Clear();
    }

    public void SetMaterials(Material newMaterial)
    {
        polygonMaterial = newMaterial;
    }
}
