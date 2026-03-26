#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// One-click repair when Player has duplicate PlayerMovement and/or missing PlayerFuel.
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
        {
            Debug.LogWarning("Void Hauler: No PlayerMovement in the scene.");
            return;
        }

        foreach (var go in roots)
        {
            var list = go.GetComponents<PlayerMovement>();
            for (var i = list.Length - 1; i >= 1; i--)
                Undo.DestroyObjectImmediate(list[i]);

            if (go.GetComponent<PlayerFuel>() == null)
                Undo.AddComponent<PlayerFuel>(go);

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

        Debug.Log("Void Hauler: Player fixed — one PlayerMovement, PlayerFuel present, camera assigned if it was empty.");
    }
}
#endif
