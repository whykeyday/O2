using UnityEngine;

/// <summary>
/// Manages the global cursor appearance throughout the game.
/// Place this script on a GameObject in the first scene (Title scene).
/// The cursor will persist across all scenes.
/// </summary>
public class GlobalCursorManager : MonoBehaviour
{
    [Header("Cursor Settings")]
    [Tooltip("The texture to use as cursor (e.g., CursorCover)")]
    public Texture2D cursorTexture;
    
    [Tooltip("Hot spot - point of the cursor that is considered the click point")]
    public Vector2 hotspot = Vector2.zero;
    
    [Tooltip("Should this cursor manager persist across scenes?")]
    public bool persistAcrossScenes = true;

    private void Awake()
    {
        // Ensure only one instance exists
        if (persistAcrossScenes)
        {
            var existing = FindObjectsByType<GlobalCursorManager>(FindObjectsSortMode.None);
            if (existing.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
        }
        
        // Set the cursor immediately
        SetGlobalCursor();
    }

    private void Start()
    {
        // Set cursor again in Start to ensure it's applied
        SetGlobalCursor();
    }

    private void SetGlobalCursor()
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
            Debug.Log($"Global cursor set to: {cursorTexture.name}");
        }
        else
        {
            Debug.LogWarning("GlobalCursorManager: No cursor texture assigned!");
        }
    }

    /// <summary>
    /// Call this method to temporarily change the cursor (e.g., for hover effects)
    /// </summary>
    public void SetTemporaryCursor(Texture2D texture, Vector2 hotspotOverride)
    {
        if (texture != null)
        {
            Cursor.SetCursor(texture, hotspotOverride, CursorMode.Auto);
        }
    }

    /// <summary>
    /// Call this method to restore the global cursor
    /// </summary>
    public void RestoreGlobalCursor()
    {
        SetGlobalCursor();
    }
}
