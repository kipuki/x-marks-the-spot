using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TerrainTreeToPrefabConverter : EditorWindow
{
    Terrain terrain;
    bool removeOriginalTrees = false;
    int targetPrototypeIndex = 0;
    bool applyToAllPrototypes = false;

    [MenuItem("Tools/Convert Terrain Trees to Prefab")]
    public static void ShowWindow()
    {
        GetWindow<TerrainTreeToPrefabConverter>("Tree to GameObject (Prefab)");
    }

    void OnGUI()
    {
        terrain = (Terrain) EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);
        applyToAllPrototypes = EditorGUILayout.Toggle("Convert All Tree Types", applyToAllPrototypes);
        if (!applyToAllPrototypes)
            targetPrototypeIndex = EditorGUILayout.IntField("Target Tree Type Index", targetPrototypeIndex);
        removeOriginalTrees = EditorGUILayout.Toggle("Remove Original Trees", removeOriginalTrees);

        if (GUILayout.Button("Convert Trees"))
            ConvertTrees();
    }

    void ConvertTrees()
    {
        if (!terrain)
        {
            Debug.LogError("Please assign a terrain.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;
        TreeInstance[] instances = terrainData.treeInstances;
        TreePrototype[] prototypes = terrainData.treePrototypes;

        Transform parent = new GameObject("ConvertedTrees").transform;
        parent.position = terrain.transform.position;

        List<TreeInstance> remainingInstances = new List<TreeInstance>();

        Undo.RegisterCompleteObjectUndo(terrainData, "Convert Terrain Trees to Prefabs");
        

        for (int i = 0; i < instances.Length; i++)
        {
            TreeInstance instance = instances[i];

            if (!applyToAllPrototypes && instance.prototypeIndex != targetPrototypeIndex)
            {
                remainingInstances.Add(instance);
                continue;
            }

            GameObject prefab = prototypes[instance.prototypeIndex].prefab;

            if (!prefab)
            {
                Debug.LogWarning("Missing tree prefab.");
                remainingInstances.Add(instance);
                continue;
            }

            // Convert local terrain coords to world space
            Vector3 position = Vector3.Scale(instance.position, terrainData.size) + terrain.transform.position;
            Quaternion rotation = Quaternion.Euler(0, instance.rotation * Mathf.Rad2Deg, 0);
            Vector3 scale = new Vector3(instance.widthScale, instance.heightScale, instance.widthScale);

            GameObject treeGO = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Undo.RegisterCreatedObjectUndo(treeGO, "Convert Terrain Tree");
            
            treeGO.transform.SetPositionAndRotation(position, rotation);
            
            Vector3 prefabScale = prefab.transform.localScale;
            treeGO.transform.localScale = new Vector3(
                prefabScale.x * instance.widthScale,
                prefabScale.y * instance.heightScale,
                prefabScale.z * instance.widthScale
            );
            treeGO.transform.SetParent(parent);

        }

        if (removeOriginalTrees)
            terrainData.treeInstances = remainingInstances.ToArray();

        Debug.Log($"Converted {instances.Length} trees to GameObjects.");
    }
}
