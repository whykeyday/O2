using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自动调整Canvas缩放，确保UI适应不同窗口大小
/// Automatically scales Canvas to fit different window sizes
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class UIAutoScaler : MonoBehaviour
{
    [Header("参考分辨率 Reference Resolution")]
    [Tooltip("设计UI时使用的分辨率")]
    public Vector2 referenceResolution = new Vector2(1920, 1080);

    [Header("缩放模式 Scale Mode")]
    [Tooltip("0.5 = 适应宽度优先, 1 = 适应高度优先")]
    [Range(0f, 1f)]
    public float matchWidthOrHeight = 0.5f;

    [Header("最小缩放 Minimum Scale")]
    [Tooltip("UI最小缩放比例，防止UI过小")]
    [Range(0.1f, 1f)]
    public float minScale = 0.5f;

    private CanvasScaler canvasScaler;
    private Canvas canvas;

    void Awake()
    {
        InitializeComponents();
    }

    void Start()
    {
        SetupCanvasScaler();
    }

    void InitializeComponents()
    {
        canvas = GetComponent<Canvas>();
        canvasScaler = GetComponent<CanvasScaler>();

        if (canvasScaler == null)
        {
            canvasScaler = gameObject.AddComponent<CanvasScaler>();
        }
    }

    void SetupCanvasScaler()
    {
        // 设置Canvas为Screen Space - Overlay模式
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 配置CanvasScaler
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
        canvasScaler.referencePixelsPerUnit = 100;

        Debug.Log($"UI Auto Scaler initialized: Reference Resolution = {referenceResolution}, Match = {matchWidthOrHeight}");
    }

    // 运行时可以调用此方法改变参考分辨率
    public void SetReferenceResolution(float width, float height)
    {
        referenceResolution = new Vector2(width, height);
        if (canvasScaler != null)
        {
            canvasScaler.referenceResolution = referenceResolution;
        }
    }

    // 调整宽度/高度匹配优先级
    public void SetMatchMode(float match)
    {
        matchWidthOrHeight = Mathf.Clamp01(match);
        if (canvasScaler != null)
        {
            canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
        }
    }

#if UNITY_EDITOR
    // 在编辑器中实时预览效果
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetupCanvasScaler();
        }
    }
#endif
}
