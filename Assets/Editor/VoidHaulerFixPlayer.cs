#if UNITY_EDITOR
using System.Collections.Generic;
using Attributes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// One-click repair when Player has duplicate PlayerMovement and/or missing Fuel.
/// </summary>
public static class VoidHaulerFixPlayer
{
    const string MenuPath = "Tools/Void Hauler/Fix Player (fuel + single movement)";

    [MenuItem(MenuPath)]
    static void FixPlayer()
    {
        var roots = new HashSet<GameObject>();
        foreach (var pm in Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
        {
            if (pm != null)
                roots.Add(pm.gameObject);
        }

        if (roots.Count == 0)
            return;

        foreach (var go in roots)
        {
            var list = go.GetComponents<PlayerMovement>();
            for (var i = list.Length - 1; i >= 1; i--)
                Undo.DestroyObjectImmediate(list[i]);

            if (go.GetComponent<Fuel>() == null)
                Undo.AddComponent<Fuel>(go);

            var main = Camera.main;
            if (main != null)
            {
                var so = new SerializedObject(go.GetComponent<PlayerMovement>());
                var camProp = so.FindProperty("_camera");
                if (camProp != null && camProp.objectReferenceValue == null)
                {
                    camProp.objectReferenceValue = main;
                    so.ApplyModifiedProperties();
                }
            }

            EditorSceneManager.MarkSceneDirty(go.scene);
        }
    }
}
#endif
