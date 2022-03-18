using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // Sound의 종류별 배열 분류
    AudioSource[] _audioSources = new AudioSource[(int)Defines.Sounds.MaxCount];
    // Sound 음원을 담은 컨테이너
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Defines.Sounds));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.GetOrAddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Defines.Sounds.Bgm].loop = true;

        }
    }

    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        _audioClips.Clear();
    }

    public void Play(string path, Defines.Sounds type = Defines.Sounds.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Defines.Sounds type = Defines.Sounds.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        switch(type)
        {
            case Defines.Sounds.Bgm:
                PlayBgm(audioClip, pitch);
                break;
            case Defines.Sounds.Effect:
                PlayEffect(audioClip, pitch);
                break;
        }
    }

    AudioClip GetOrAddAudioClip(string path, Defines.Sounds type = Defines.Sounds.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if(type == Defines.Sounds.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            // 자주 사용될 여지가 있는 타입의 Clip은 Load 최소화
            // 한번 사용되었던 Clip은 컨테이너에 담겨있으므로 가져와서 사용한다.
            if(_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing : {path}");

        return audioClip;
    }

    private void PlayBgm(AudioClip audioClip, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)Defines.Sounds.Bgm];

        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.pitch = pitch;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void PlayEffect(AudioClip audioClip, float pitch = 1.0f)
    {
        AudioSource audioSource = _audioSources[(int)Defines.Sounds.Effect];
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip); // 원하는 Clip 중첩 재생
    }
}
