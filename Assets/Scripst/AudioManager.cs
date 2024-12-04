using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundEffectsSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] [Range(0f, 1f)] public float backgroundMusicVolume = 0.7f;

    [Header("Sound Effects")]
    [SerializeField] private SoundData[] soundEffectsData;
    [SerializeField] [Range(0f, 1f)] public float soundEffectsVolume = 0.7f;

    private Dictionary<string, SoundData> soundEffectsDictionary;

    [System.Serializable]
    public struct SoundData
    {
        public string name;
        public AudioClip clip;
        public float pitchRange;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundEffectsDictionary = new Dictionary<string, SoundData>();
        foreach (var soundData in soundEffectsData)
        {
            if (!soundEffectsDictionary.ContainsKey(soundData.name))
            {
                soundEffectsDictionary.Add(soundData.name, soundData);
            }
            else
            {
                Debug.LogWarning($"Duplicate sound name found: {soundData.name}. Only the first instance will be used.");
            }
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateAudioSources();
    }

    private void UpdateAudioSources()
    {
        if (backgroundMusicSource == null)
        {
            backgroundMusicSource = GameObject.Find("Background Music Source")?.GetComponent<AudioSource>();
            if (backgroundMusicSource != null) SetupBackgroundMusic();

        }
        if (soundEffectsSource == null)
        {
            soundEffectsSource = GameObject.Find("Sound Effect Source")?.GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        // Setup and play background music
        SetupBackgroundMusic();
    }

    private void SetupBackgroundMusic()
    {
        if (backgroundMusicSource == null || backgroundMusic == null)
        {
            Debug.LogWarning("Background music source or clip not assigned. Background music will not play.");
            return;
        }

        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true; // Typically, background music loops
        backgroundMusicSource.volume = backgroundMusicVolume;
        backgroundMusicSource.Play();
    }

    public void PlaySoundEffect(string soundName)
    {
        if (soundEffectsSource == null)
        {
            Debug.LogError("Sound effects source not assigned. Cannot play sound effect.");
            return;
        }

        if (soundEffectsDictionary.TryGetValue(soundName, out SoundData soundData))
        {
            if (soundData.clip == null)
            {
                Debug.LogWarning($"Clip not assigned for sound effect '{soundName}'.");
                return;
            }

            float randomPitch = GetRandomPitch(soundData.pitchRange);
            soundEffectsSource.pitch = randomPitch;
            soundEffectsSource.PlayOneShot(soundData.clip);
        }
        else
        {
            Debug.LogWarning($"Sound effect with name '{soundName}' not found.");
        }
    }
    private float GetRandomPitch(float pitchRange)
    {

        if (pitchRange == 0) return 1f;

        float randomPitch = Random.Range(-pitchRange, pitchRange);

        return 1f + randomPitch;
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicVolume = Mathf.Clamp01(volume);
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = backgroundMusicVolume;
            PlayerPrefs.SetFloat("MusicVolume", backgroundMusicVolume);

        }
    }

    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = Mathf.Clamp01(volume);
        if (soundEffectsSource != null)
        {
            soundEffectsSource.volume = soundEffectsVolume;
            PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolume);
        }
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying && backgroundMusic != null)
        {
            backgroundMusicSource.Play();
        }
    }
    public void ToggleBackgroundMusic()
    {
        if (backgroundMusicSource != null)
        {
            if (backgroundMusicSource.isPlaying)
            {
                StopBackgroundMusic();
            }
            else
            {
                PlayBackgroundMusic();
            }
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}