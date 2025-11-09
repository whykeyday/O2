using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 0.75f;
    public bool loop = true;
    public bool playOnStart = true;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.playOnAwake = false;
        _source.loop = loop;
    }

    private void Start()
    {
        if (clip != null)
        {
            _source.clip = clip;
            _source.volume = volume;
            if (playOnStart)
            {
                _source.Play();
            }
        }
        else
        {
            Debug.LogWarning("BGMPlayer has no AudioClip assigned on " + name);
        }
    }
}
