using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct ManagedSound
    {
        public SFXType type;
        [Range(0f, 1f)]
        public float volume;
    }
    [System.Serializable]
    struct TundedClip
    {
        public AudioClip clip;
        public float volume;
    }
    static public AudioManager Instance { get; private set; }
    AudioSource _source;
    bool[] _soundBuffer = new bool[Enum.GetNames(typeof(SFXType)).Length];

    [SerializeField, Range(0f, 1f)] float _volumeSetting = 1f;
    [SerializeField] SFXType _debug_lookup;
    [SerializeField] TundedClip[] _clips;
    public enum SFXType
    {
        None = 999,
        Gunshot = 0,
        Gunhit = 1,
        Jump = 2,
    }
    private void LateUpdate()
    {
        for (int i = 0; i < _soundBuffer.Length; i++)
        {
            _soundBuffer[i] = true;
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _source = GetComponent<AudioSource>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        
    }
    public void PlaySound(SFXType type, float volume)
    {
        if (type != SFXType.None && _soundBuffer[(int)type])
        {
            _soundBuffer[(int)type] = false;
            TundedClip tc = _clips[(int)type];
            _source.PlayOneShot(tc.clip, tc.volume * volume * _volumeSetting);
        }
    }
    public void PlaySound(ManagedSound sound)
    {
        if (sound.type != SFXType.None && _soundBuffer[(int)sound.type])
        {
            _soundBuffer[(int)sound.type] = false;
            TundedClip tc = _clips[(int)sound.type];
            _source.PlayOneShot(tc.clip, tc.volume * sound.volume * _volumeSetting);
        }
    }
}
