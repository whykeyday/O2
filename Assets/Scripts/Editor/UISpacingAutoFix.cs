using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public class UISpacingAutoFix : EditorWindow
{
    private int paddingLeft = 50;
    private int paddingRight = 50;
    private int paddingTop = 80;
    private int paddingBottom = 80;
    private int spacing = 60;
    
    [MenuItem("Tools/Fix UI Spacing In All Scenes")]
    static void ShowWindow()
    {
        GetWindow<UISpacingAutoFix>("UI Spacing Fixer");
    }
    
    void OnGUI()
    {
        GUILayout.Label("UI Spacing Settings", EditorStyles.boldLabel);
        
        paddingLeft = EditorGUILayout.IntField("Padding Left", paddingLeft);
        paddingRight = EditorGUILayout.IntField("Padding Right", paddingRight);
        paddingTop = EditorGUILayout.IntField("Padding Top", paddingTop);
        paddingBottom = EditorGUILayout.IntField("Padding Bottom", paddingBottom);
        spacing = EditorGUILayout.IntField("Spacing", spacing);
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Apply to All Scenes", GUILayout.Height(40)))
        {
            ApplyToAllScenes();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Apply to Current Scene Only", GUILayout.Height(30)))
        {
            ApplyToCurrentScene();
        }
    }
    
    void ApplyToAllScenes()
    {
        // Save current scene
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        
        // Find all scene files
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        
        int processed = 0;
        foreach (string guid in sceneGUIDs)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            
            // Skip Template scene
            if (sceneName == "Template")
                continue;
            
            Debug.Log($"Processing scene: {sceneName}");
            
            // Open scene
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            // Apply spacing
            if (ApplySpacingToScene())
            {
                // Save scene
                EditorSceneManager.SaveScene(scene);
                processed++;
                Debug.Log($"✓ Updated spacing in: {sceneName}");
            }
            else
            {
                Debug.LogWarning($"⚠ Could not find suitable parent in: {sceneName}");
            }
        }
        
        EditorUtility.DisplayDialog("Complete", 
            $"UI spacing applied to {processed} scenes!", "OK");
    }
    
    void ApplyToCurrentScene()
    {
        if (ApplySpacingToScene())
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorUtility.DisplayDialog("Complete", 
                "UI spacing applied to current scene!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Error", 
                "Could not find suitable Canvas or parent object in this scene.", "OK");
        }
    }
    
    bool ApplySpacingToScene()
    {
        // Find Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No Canvas found in scene");
            return false;
        }
        
        // Strategy 1: Find existing panel/container
        Transform container = FindContainer(canvas.transform);
        
        if (container != null)
        {
            AddOrUpdateLayoutGroup(container.gameObject);
            return true;
        }
        
        // Strategy 2: Create a container if Canvas has direct UI children
        if (canvas.transform.childCount > 0)
        {
            GameObject newContainer = CreateContainer(canvas.gameObject);
            if (newContainer != null)
            {
                AddOrUpdateLayoutGroup(newContainer);
                return true;
            }
        }
        
        return false;
    }
    
    Transform FindContainer(Transform parent)
    {
        // Look for common container names
        string[] containerNames = { "Panel", "Container", "Content", "UI", "Layout" };
        
        foreach (Transform child in parent)
        {
            string lowerName = child.name.ToLower();
            foreach (string name in containerNames)
            {
                if (lowerName.Contains(name.ToLower()))
                {
                    // Check if this container has UI children
                    if (HasUIChildren(child))
                        return child;
                }
            }
        }
        
        // If no named container, look for any Transform with multiple UI children
        foreach (Transform child in parent)
        {
            if (HasUIChildren(child) && child.childCount >= 2)
                return child;
        }
        
        return null;
    }
    
    bool HasUIChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<UnityEngine.UI.Image>() != null ||
                child.GetComponent<TMPro.TextMeshProUGUI>() != null ||
                child.GetComponent<Button>() != null ||
                child.GetComponent<UnityEngine.UI.Text>() != null)
            {
                return true;
            }
        }
        return false;
    }
    
    GameObject CreateContainer(GameObject canvas)
    {
        // Check if there are enough direct children to warrant a container
        int uiChildCount = 0;
        foreach (Transform child in canvas.transform)
        {
            if (child.GetComponent<UnityEngine.UI.Image>() != null ||
                child.GetComponent<TMPro.TextMeshProUGUI>() != null ||
                child.GetComponent<Button>() != null)
            {
                uiChildCount++;
            }
        }
        
        if (uiChildCount < 2)
            return null;
        
        // Create container
        GameObject container = new GameObject("UI Container");
        container.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = container.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        
        // Move UI children into container
        Transform[] children = new Transform[canvas.transform.childCount];
        int index = 0;
        foreach (Transform child in canvas.transform)
        {
            if (child != container.transform)
                children[index++] = child;
        }
        
        foreach (Transform child in children)
        {
            if (child != null && child.gameObject != container)
            {
                child.SetParent(container.transform, true);
            }
        }
        
        return container;
    }
    
    void AddOrUpdateLayoutGroup(GameObject obj)
    {
        VerticalLayoutGroup layout = obj.GetComponent<VerticalLayoutGroup>();
        
        if (layout == null)
        {
            layout = obj.AddComponent<VerticalLayoutGroup>();
            Debug.Log($"Added VerticalLayoutGroup to {obj.name}");
        }
        else
        {
            Debug.Log($"Updated existing VerticalLayoutGroup on {obj.name}");
        }
        
        // Configure layout
        layout.padding = new RectOffset(paddingLeft, paddingRight, paddingTop, paddingBottom);
        layout.spacing = spacing;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        
        EditorUtility.SetDirty(obj);
    }
}
