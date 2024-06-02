using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using TMPro;

public class ReplaceAllTMPUGUI2RubyTMPUGUI : EditorWindow
{
    [MenuItem("Tools/Replace TMP to RubyTMP")]
    static void ReplaceComponents()
    {
        string[] scenePaths = AssetDatabase.FindAssets("t:Scene");
        foreach (string scenePath in scenePaths)
        {
            string sceneFilePath = AssetDatabase.GUIDToAssetPath(scenePath);
            Scene scene = EditorSceneManager.OpenScene(sceneFilePath);

            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObject in rootObjects)
            {
                ReplaceComponentsInGameObject(rootObject);
            }

            // Save the scene
            EditorSceneManager.SaveScene(scene);
        }

        Debug.Log("Replacement completed.");
    }

    static void ReplaceComponentsInGameObject(GameObject gameObject)
    {
        TextMeshProUGUI[] components = gameObject.GetComponents<TextMeshProUGUI>();
        foreach (TextMeshProUGUI component in components)
        {
            ReplaceComponent(component);
        }

        // Recursively replace components in child game objects
        foreach (Transform child in gameObject.transform)
        {
            ReplaceComponentsInGameObject(child.gameObject);
        }
    }

    static void ReplaceComponent(TextMeshProUGUI oldComponent)
    {
        GameObject gameObject = oldComponent.gameObject;

        // Add the new component (B) and copy values from the old component (A)
        RubyTextMeshProUGUI newComponent = gameObject.AddComponent<RubyTextMeshProUGUI>();

        // Copy values from old component to new component
        EditorUtility.CopySerialized(oldComponent, newComponent);

        // Destroy the old component
        DestroyImmediate(oldComponent, true);
    }
}
