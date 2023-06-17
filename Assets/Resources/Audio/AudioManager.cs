using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    static public AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource effectSource;
    static Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    public List<AudioWithName> audioPresets = new List<AudioWithName>();
    [Serializable]
    public class AudioWithName{
        public string name;
        public AudioClip clip;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
            return;
        }
        foreach(var audio in audioPresets)
        {
            if (!audioClips.ContainsKey(audio.name))
                audioClips.Add(audio.name, audio.clip);
        }
    }
    void Start()
    {
        musicSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PlayBGM()
    {
        Instance.musicSource.Play();
    }
    public static void PlayMusicByName(string name, bool loop = true)
    {
        AudioClip clip = audioClips[name];
        if (clip == null) {
            Debug.Log("Clip not found!");
            return;
        }
        if (Instance.musicSource.clip == clip) return;
        Instance.musicSource.loop = loop;
        Instance.musicSource.clip = clip;
        Instance.musicSource.Play();
    }
}
