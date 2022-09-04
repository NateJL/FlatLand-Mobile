using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FenceEditorController))]
public class FenceEditor : Editor
{
    int toolbarIndex = 0;
    string[] toolbarStrings = { "Custom", "Default" };

    public bool showFences = true;

    public float rotationIncrement = 15.0f;
    public bool useIncrement = false;

    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        FenceEditorController controller = (FenceEditorController)target;

        // start checking for changes
        EditorGUI.BeginChangeCheck();

        if (controller.fences == null)
            controller.GetCurrentFences();

        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (toolbarIndex == 0)
        {
            if (GUILayout.Button("Reload", GUILayout.Width(100)))
            {
                controller.GetCurrentFences();
            }
            controller.fencePrefab = (GameObject)EditorGUILayout.ObjectField("Fence Prefab", controller.fencePrefab, typeof(GameObject), true);
            controller.fenceAnglePrefab = (GameObject)EditorGUILayout.ObjectField("Angled Fence Prefab", controller.fenceAnglePrefab, typeof(GameObject), true);

            controller.showGizmos = EditorGUILayout.Toggle("Show Gizmos", controller.showGizmos);
            showFences = EditorGUILayout.Toggle("Show Fences", showFences);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if(controller.currentFenceIndex < controller.fences.Count)
            {
                EditorGUILayout.ObjectField("Selected", controller.fences[controller.currentFenceIndex], typeof(GameObject), true);
                controller.fences[controller.currentFenceIndex].transform.position = EditorGUILayout.Vector3Field("Pos", controller.fences[controller.currentFenceIndex].transform.position);
                controller.fences[controller.currentFenceIndex].transform.eulerAngles = EditorGUILayout.Vector3Field("Rot", controller.fences[controller.currentFenceIndex].transform.eulerAngles);
                controller.fences[controller.currentFenceIndex].GetComponent<FenceController>().isEnd = EditorGUILayout.Toggle("Is End Piece", controller.fences[controller.currentFenceIndex].GetComponent<FenceController>().isEnd);
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            controller.currentFenceIndex = (int) EditorGUILayout.Slider(controller.currentFenceIndex, 0, controller.fences.Count);
            useIncrement = EditorGUILayout.Toggle("Use Rot Increment", useIncrement);
            rotationIncrement = EditorGUILayout.FloatField("Rot. Increment", rotationIncrement);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Fence"))
            {
                controller.AddFence();
                if(useIncrement)
                {
                    controller.fences[controller.currentFenceIndex].transform.Rotate(new Vector3(0, rotationIncrement, 0));
                }
            }
            if(GUILayout.Button("Add 45* Fence"))
            {
                controller.AddAngleFence();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Remove Fence"))
            {
                controller.RemoveFence();
            }

            if(showFences)
            {
                for(int i = 0; i < controller.fences.Count; i++)
                {
                    if(i == controller.currentFenceIndex)
                        controller.fences[i] = (GameObject)EditorGUILayout.ObjectField("Selected", controller.fences[i], typeof(GameObject), true);
                    else
                        controller.fences[i] = (GameObject)EditorGUILayout.ObjectField(i.ToString(), controller.fences[i], typeof(GameObject), true);
                }
            }
        }
        else if (toolbarIndex == 1)
        {
            DrawDefaultInspector();
        }

        // Force scene to repaint to update gizmos
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }
}
