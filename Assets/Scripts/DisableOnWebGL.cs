using UnityEngine;

public class DisableOnWebGL : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        gameObject.SetActive(false);
#endif
    }
}
