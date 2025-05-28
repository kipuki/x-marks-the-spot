using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Collections.Generic;

public class FindBrokenUnityEvents : EditorWindow
{
    [MenuItem("Tools/Find Broken UnityEvents")]
    static void FindMissingUnityEventMethods()
    {
        int brokenCount = 0;
        var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var go in allGameObjects)
        {
            if (EditorUtility.IsPersistent(go)) continue; // skip prefabs not in scenes

            var components = go.GetComponents<MonoBehaviour>();
            foreach (var comp in components)
            {
                if (comp == null) continue;

                var fields = comp.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    if (typeof(UnityEventBase).IsAssignableFrom(field.FieldType))
                    {
                        var unityEvent = field.GetValue(comp) as UnityEventBase;
                        if (unityEvent == null) continue;

                        for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                        {
                            var target = unityEvent.GetPersistentTarget(i);
                            var method = unityEvent.GetPersistentMethodName(i);

                            if (target == null || string.IsNullOrEmpty(method))
                            {
                                Debug.LogWarning($"[Missing Method] GameObject '{go.name}' has a UnityEvent with a missing method.", go);
                                brokenCount++;
                            }
                            else
                            {
                                var methodInfo = target.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                                if (methodInfo == null)
                                {
                                    Debug.LogWarning($"[Broken Method] '{method}' not found on target '{target.name}' (GameObject: '{go.name}')", go);
                                    brokenCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        Debug.Log($"Finished scan. Found {brokenCount} broken UnityEvent references.");
    }
}
