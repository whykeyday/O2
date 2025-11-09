using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SceneBGM
{
    public string sceneName;
    public AudioClip clip;
}

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public SceneBGM[] mappings;
    public AudioClip defaultBGM;  // Default music for all scenes
    [Range(0f, 1f)] public float volume = 0.75f;

    private AudioSource _source;

    private void Awake()
    {
        var existing = FindObjectsByType<BGMManager>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = true;
        _source.volume = volume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // First, check for scene-specific BGM
        for (int i = 0; i < mappings.Length; i++)
        {
            if (mappings[i].clip != null && mappings[i].sceneName == scene.name)
            {
                if (_source.clip != mappings[i].clip)
                {
                    _source.clip = mappings[i].clip;
                    _source.Play();
                }
                return;
            }
        }
        
        // If no specific mapping found, use default BGM
        if (defaultBGM != null && _source.clip != defaultBGM)
        {
            _source.clip = defaultBGM;
            _source.Play();
        }
    }
}
