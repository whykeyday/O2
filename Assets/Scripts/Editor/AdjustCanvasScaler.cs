using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class AdjustCanvasScaler : EditorWindow
{
    private float referenceResolutionX = 1920;
    private float referenceResolutionY = 1080;
    private float matchValue = 0.5f;
    
    [MenuItem("Tools/Adjust Canvas Scaler Settings")]
    static void ShowWindow()
    {
        GetWindow<AdjustCanvasScaler>("Canvas Scaler Adjuster");
    }
    
    void OnGUI()
    {
        GUILayout.Label("Canvas Scaler Settings", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        referenceResolutionX = EditorGUILayout.FloatField("Reference Width", referenceResolutionX);
        referenceResolutionY = EditorGUILayout.FloatField("Reference Height", referenceResolutionY);
        matchValue = EditorGUILayout.Slider("Match (Width/Height)", matchValue, 0f, 1f);
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("Apply to All Scenes", GUILayout.Height(40)))
        {
            ApplyToAllScenes();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("Apply to Current Scene", GUILayout.Height(30)))
        {
            ApplyToCurrentScene();
        }
    }
    
    void ApplyToAllScenes()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        
        int processed = 0;
        foreach (string guid in sceneGUIDs)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            
            if (ConfigureCanvasScaler())
            {
                EditorSceneManager.SaveScene(scene);
                processed++;
                Debug.Log($"✓ Configured Canvas Scaler in: {scene.name}");
            }
        }
        
        EditorUtility.DisplayDialog("Complete", 
            $"Canvas Scaler configured in {processed} scenes!", "OK");
    }
    
    void ApplyToCurrentScene()
    {
        if (ConfigureCanvasScaler())
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorUtility.DisplayDialog("Complete", 
                "Canvas Scaler configured!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Error", 
                "No Canvas found in scene!", "OK");
        }
    }
    
    bool ConfigureCanvasScaler()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
            return false;
        
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        if (scaler == null)
            scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(referenceResolutionX, referenceResolutionY);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = matchValue;
        scaler.referencePixelsPerUnit = 100;
        
        EditorUtility.SetDirty(canvas.gameObject);
        
        return true;
    }
}
