using UnityEngine;
using UnityEditor;

public class TerrainTreeResizer : EditorWindow
{
    Terrain terrain;
    int targetPrototypeIndex = 0;
    float newWidth = 1f;
    float newHeight = 1f;
    bool applyToAllPrototypes = false;

    [MenuItem("Tools/Resize Terrain Trees")]
    public static void ShowWindow()
    {
        GetWindow<TerrainTreeResizer>("Resize Terrain Trees");
    }

    void OnGUI()
    {
        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);
        applyToAllPrototypes = EditorGUILayout.Toggle("Apply To All Tree Types", applyToAllPrototypes);
        if (!applyToAllPrototypes)
            targetPrototypeIndex = EditorGUILayout.IntField("Target Tree Type Index", targetPrototypeIndex);

        newWidth = EditorGUILayout.FloatField("New Width Scale", newWidth);
        newHeight = EditorGUILayout.FloatField("New Height Scale", newHeight);

        if (GUILayout.Button("Resize Trees"))
        {
            ResizeTrees();
        }
    }

    void ResizeTrees()
    {
        if (!terrain)
        {
            Debug.LogError("No terrain assigned.");
            return;
        }

        TerrainData data = terrain.terrainData;
        TreeInstance[] instances = data.treeInstances;

        Undo.RegisterCompleteObjectUndo(data, "Resize Terrain Trees");

        for (int i = 0; i < instances.Length; i++)
        {
            TreeInstance instance = instances[i];

            if (applyToAllPrototypes || instance.prototypeIndex == targetPrototypeIndex)
            {
                instance.widthScale = newWidth;
                instance.heightScale = newHeight;
                instances[i] = instance;
            }
        }

        data.treeInstances = instances;

        Debug.Log("Tree resizing complete.");
    }
}
