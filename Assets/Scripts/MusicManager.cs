using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }
}
