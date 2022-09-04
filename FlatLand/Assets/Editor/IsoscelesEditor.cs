using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsoscelesController))]
public class IsoscelesEditor : Editor
{
    int toolbarIndex = 0;
    string[] toolbarStrings = { "Custom", "Default" };

    public bool showFences = true;

    private void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        IsoscelesController controller = (IsoscelesController)target;

        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (toolbarIndex == 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Polygon"))
            {
                controller.InitializePolygonModel();
            }
            if (GUILayout.Button("Clear Polygon"))
            {
                controller.ClearMesh();
            }
            EditorGUILayout.EndHorizontal();

        }
        else if (toolbarIndex == 1)
        {
            DrawDefaultInspector();
        }
    }
}
