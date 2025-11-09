using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class RemoveLayoutGroups : EditorWindow
{
    [MenuItem("Tools/Remove All Layout Groups")]
    static void ShowWindow()
    {
        if (EditorUtility.DisplayDialog("Remove Layout Groups", 
            "This will remove all Vertical Layout Groups from all scenes. Continue?", 
            "Yes", "Cancel"))
        {
            RemoveFromAllScenes();
        }
    }
    
    static void RemoveFromAllScenes()
    {
        // Save current scene
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        
        // Find all scene files
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        
        int processed = 0;
        foreach (string guid in sceneGUIDs)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            
            // Open scene
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            // Find and remove all VerticalLayoutGroup components
            VerticalLayoutGroup[] layouts = FindObjectsOfType<VerticalLayoutGroup>();
            foreach (var layout in layouts)
            {
                DestroyImmediate(layout);
                processed++;
            }
            
            if (layouts.Length > 0)
            {
                EditorSceneManager.SaveScene(scene);
                Debug.Log($"Removed {layouts.Length} layout groups from: {scene.name}");
            }
        }
        
        EditorUtility.DisplayDialog("Complete", 
            $"Removed {processed} layout groups from all scenes!", "OK");
    }
}
