using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldEditor))]
public class WorldEditorWindow : Editor
{
    int toolbarIndex = 0;
    string[] toolbarStrings = { "Default", "Editor", "Prefabs" };

    public PlaceableObject newObject;

    public int prefabIndex;

    Texture2D prefabPreview = null;

    private void OnEnable()
    {
        newObject = new PlaceableObject("Enter Tag", null);
        prefabIndex = 0;
    }

    public override void OnInspectorGUI()
    {
        WorldEditor controller = (WorldEditor)target;

        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if(toolbarIndex == 0)
        {
            DrawDefaultInspector();
        }
        else if(toolbarIndex == 1)
        {
            DisplayEditorPage(controller);
        }
        else if(toolbarIndex == 2)
        {
            DisplayPrefabPage(controller);
        }
    }

    /// <summary>
    /// Method called when the editor tab is selected.
    /// </summary>
    private void DisplayEditorPage(WorldEditor controller)
    {
        EditorGUILayout.LabelField("World Editor", EditorStyles.boldLabel);
        controller.objParent = controller.gameObject;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("World Parent", controller.objParent, typeof(GameObject), true);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.ObjectField("Selected Prefab", controller.selectedPrefab, typeof(GameObject), true);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Previous"))
        {
            if (prefabIndex <= 0)
                prefabIndex = controller.prefabs.Count - 1;
            else
                prefabIndex -= 1;
        }
        if (GUILayout.Button("Next"))
        {
            if (prefabIndex >= controller.prefabs.Count-1)
                prefabIndex = 0;
            else
                prefabIndex += 1;
        }
        controller.selectedPrefab = controller.prefabs[prefabIndex].prefab;
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        prefabPreview = (Texture2D)AssetPreview.GetAssetPreview(controller.selectedPrefab);
        if (prefabPreview != null)
            GUILayout.Label(prefabPreview);
    }

    /// <summary>
    /// Method called when the prefab tab is selected.
    /// </summary>
    private void DisplayPrefabPage(WorldEditor controller)
    {
        EditorGUILayout.LabelField("Add New Prefab", EditorStyles.boldLabel);
        newObject.tag = EditorGUILayout.TextField("Tag: ", newObject.tag);
        newObject.prefab =(GameObject) EditorGUILayout.ObjectField("Prefab:", newObject.prefab, typeof(GameObject), true);
        if(GUILayout.Button("Add Prefab"))
        {
            if(newObject.prefab != null)
            {
                controller.prefabs.Add(new PlaceableObject(newObject.tag, newObject.prefab));
                newObject.tag = "Enter Tag";
                newObject.prefab = null;
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Prefab Collection", EditorStyles.boldLabel);
        for(int i = 0; i < controller.prefabs.Count; i++)
        {
            EditorGUILayout.ObjectField(controller.prefabs[i].tag, controller.prefabs[i].prefab, typeof(GameObject), true);
        }
    }
}
