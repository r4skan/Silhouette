using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ESoundType
{
    Bgm,
    Effect,
    MaxCount,
}

[System.Serializable]
public class SoundGroup
{
    public string groupName;
    public List<AudioClip> audioClipList;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager s_instance;
    public static SoundManager Instance => s_instance;

    public SoundOptionData soundOption;
    
    private AudioSource[] m_audioSources = new AudioSource[(int)ESoundType.MaxCount];
    private Dictionary<string, AudioClip> m_audioClips = new Dictionary<string, AudioClip>();

    public UnityAction onSoundValueChangeEvent;

    private void Awake()
    {
        if (!s_instance)
        {
            s_instance = this;
        }
        else if (s_instance != this)
        {
            return;
        }
        
        Init();
    }

    private void OnEnable()
    {
        SceneController.Instance.onSceneInEvent += Clear;

        SceneController.Instance.onSceneOutEvent -= ClearRegisteredEvents;
        SceneController.Instance.onSceneOutEvent += ClearRegisteredEvents;
    }

    public void Init()
    {
        GameObject _root = GameObject.FindGameObjectWithTag("AudioRoot");

        if (_root) return;
        
        _root = new GameObject {name = "@Audio"};
        _root.tag = "AudioRoot";
        DontDestroyOnLoad(_root);

        string[] _soundNames = Enum.GetNames(typeof(ESoundType));
        for (int i = 0; i < _soundNames.Length - 1; ++i)
        {
            GameObject _sounds = new GameObject { name = _soundNames[i] };

            m_audioSources[i] = _sounds.AddComponent<AudioSource>();
            SoundSynchronizer _soundSync = _sounds.AddComponent<SoundSynchronizer>();

            _soundSync.type = _soundNames[i] == "Bgm" ? ESoundType.Bgm : ESoundType.Effect;
            _soundSync.source = m_audioSources[i];

            _sounds.transform.parent = _root.transform;
        }

        // 배경음악은 무한 반복
        m_audioSources[(int)ESoundType.Bgm].loop = true;
    }

    public void Clear()
    {
        // 재생기 전부 재생 스탑 및 음원 빼기
        foreach (AudioSource _audioSource in m_audioSources)
        {
            _audioSource.clip = null;
            _audioSource.Stop();
        }
        
        // 딕셔너리 비워주기
        m_audioClips.Clear();
    }

    public void Play(AudioClip audioClip, ESoundType type = ESoundType.Effect, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!audioClip) return;

        if (type == ESoundType.Bgm)
        {
            AudioSource _audioSource = m_audioSources[(int)ESoundType.Bgm];
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            _audioSource.volume = volume * (soundOption.volume_BGM / 100);
            _audioSource.pitch = pitch;
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }
        else
        {
            AudioSource _audioSource = m_audioSources[(int)ESoundType.Effect];
            _audioSource.volume = volume * (soundOption.volume_Effect / 100);
            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(audioClip);
        }
    }

    public void PlayAt(AudioClip audioClip, AudioSource audioSource, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!audioClip) return;

        audioSource.clip = audioClip;
        audioSource.volume = volume * (soundOption.volume_Effect / 100);
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    public void Play(string path, ESoundType type = ESoundType.Effect, float volume = 1f, float pitch = 1f)
    {
        AudioClip _audioClip = GetOrAddAudioClip(path, type);
        Play(_audioClip, type, volume, pitch);
    }

    private AudioClip GetOrAddAudioClip(string path, ESoundType type = ESoundType.Effect)
    {
        if (!path.Contains("Sounds/"))
        {
            path = $"Sounds/{path}";
        }

        AudioClip _audioClip = null;

        if (type == ESoundType.Bgm)
        {
            _audioClip = Resources.Load<AudioClip>(path);
        }
        else // 효과음은 자주 사용되므로 Dictionary에 저장해두고 불러와서 씀
        {
            if (!m_audioClips.TryGetValue(path, out _audioClip))
            {
                _audioClip = Resources.Load<AudioClip>(path);
                m_audioClips.Add(path, _audioClip);
            }
        }

        // 오디오 클립을 못 찾았다면 로그 출력
        if (!_audioClip)
        {
            Debug.LogWarning($"Missing AudioClip! {path}");
        }

        return _audioClip;
    }

    public void OnSoundValueChange()
    {
        onSoundValueChangeEvent?.Invoke();
    }

    private void ClearRegisteredEvents()
    {
        print("SoundManager's Event Cleared!");
        onSoundValueChangeEvent = null;
    }
}
