using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonGenerator))]
public class PolygonEditor : Editor
{
    int toolbarIndex = 0;
    string[] toolbarStrings = { "Custom", "Default"};

    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        PolygonGenerator controller = (PolygonGenerator)target;

        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (toolbarIndex == 0)
        {
            controller.polygonMaterial = (Material) EditorGUILayout.ObjectField("Outer Material", controller.polygonMaterial, typeof(Material), true);
            EditorGUILayout.LabelField(" ");
            controller.numberOfEdges = EditorGUILayout.IntField("Number of Edges: ", controller.numberOfEdges);
            controller.showGizmos = EditorGUILayout.Toggle("Show Gizmos", controller.showGizmos);
            /*if (GUILayout.Button("Calculate Vertices"))
            {
                controller.CalculatePolygonVertices(controller.numberOfEdges + 1, 1);
            }

            EditorGUILayout.LabelField("Polygon Vertices");
            for (int i = 0; i < controller.vertices.Length; i++)
            {
                EditorGUILayout.Vector3Field(i.ToString(),controller.vertices[i]);
            }
            */
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Generate Mesh"))
            {
                controller.GenerateFullPolygon(controller.numberOfEdges + 1, 0.7f);
            }
            if(GUILayout.Button("Clear Mesh"))
            {
                controller.ClearPolygon();
            }
            EditorGUILayout.EndHorizontal();
        }
        else if (toolbarIndex == 1)
        {
            DrawDefaultInspector();
        }
    }

    private void OnSceneGUI()
    {
        PolygonGenerator controller = (PolygonGenerator)target;

        if (controller.showGizmos)
            DrawVertexIndices(controller, GizmoType.Active);
    }

    static void DrawVertexIndices(PolygonGenerator scr, GizmoType gizmoType)
    {
        if (scr.vertices != null)
        {
            Handles.color = Color.red;
            for (int i = 0; i < scr.vertices.Length; i++)
            {
                Handles.Label(scr.vertices[i] + Vector3.up*0.3f, i.ToString());
                //Handles.SphereHandleCap(i, scr.polygonPoints[i], Quaternion.identity, 0.3f, EventType.ScrollWheel);
            }
        }
    }
}
