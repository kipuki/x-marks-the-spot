using UnityEngine;
using UnityEditor;

public class TerrainTreeToPrefabConverter : EditorWindow
{
    Terrain terrain;
    bool removeOriginalTrees = false;

    [MenuItem("Tools/Convert Terrain Trees to Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<TerrainTreeToPrefabConverter>("Tree Converter");
    }

    void OnGUI()
    {
        terrain = (Terrain) EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);
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

        for (int i = 0; i < instances.Length; i++)
        {
            TreeInstance instance = instances[i];
            GameObject prefab = prototypes[instance.prototypeIndex].prefab;

            if (!prefab)
            {
                Debug.LogWarning("Missing tree prefab.");
                continue;
            }

            // Convert local terrain coords to world space
            Vector3 position = Vector3.Scale(instance.position, terrainData.size) + terrain.transform.position;
            Quaternion rotation = Quaternion.Euler(0, instance.rotation * Mathf.Rad2Deg, 0);
            Vector3 scale = new Vector3(instance.widthScale, instance.heightScale, instance.widthScale);

            GameObject treeGO = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
            treeGO.transform.SetPositionAndRotation(position, rotation);
            treeGO.transform.localScale = scale;
            treeGO.transform.SetParent(parent);

        }

        if (removeOriginalTrees)
            terrainData.treeInstances = new TreeInstance[0];

        Debug.Log($"Converted {instances.Length} trees to GameObjects.");
    }
}
