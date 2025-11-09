using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public void OnClickExit()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("Exit is not supported on WebGL. Please close the tab.");
#else
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
#endif
    }
}
