using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICursorHover : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    public void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseExit()
    {
        // Try to restore global cursor instead of setting to null
        var globalCursor = FindObjectOfType<GlobalCursorManager>();
        if (globalCursor != null)
        {
            globalCursor.RestoreGlobalCursor();
        }
        else
        {
            // Fallback: set to null if no global cursor manager exists
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
