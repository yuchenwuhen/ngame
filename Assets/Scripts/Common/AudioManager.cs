using UnityEngine;
using System.Collections;

public enum MenuSingleClip
{
    Start = 0,
    Menu,
    Scene,
    End
}

public class AudioManager : MonoBehaviour
{
    
    private static AudioManager _instance;


    public static AudioManager Instance
    {
        get { return _instance; }
    }
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public AudioSource _startSource;
    public AudioSource _efxSource;
    public AudioSource _bgSource;
    public AudioSource _musicSource;
    public AudioSource m_defaultSource;
    public AudioSource[] m_audioSource;
    public AudioSource m_waterSource;
    [Tooltip("Start,Menu,Scene,End")]
    public AudioClip[] m_menuSingleClip;

    void Awake()
    {
        _instance = this;
    }

    public void RandomPlay(params AudioClip[] clips)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        _efxSource.clip = clip;
        _efxSource.pitch = pitch;
        _efxSource.Play();
    }

    public void PlayMusicSingleAgain(AudioClip audioClip)
    {
        _musicSource.Stop();
        _musicSource.clip = audioClip;
        _musicSource.Play();
    }
    public void PlayMusicSingle(AudioClip audioClip)
    {
        _musicSource.Play();
    }

    public void PauseMusicSingle(AudioClip audioClip)
    {
        _musicSource.Pause();
    }

    public void PlayWaterAudio()
    {
        m_waterSource.Stop();
        m_waterSource.Play();
    }

    public float GetMusicWaterTime()
    {
        return m_waterSource.time;
    }

    public float GetMusicSourceTime()
    {
        return _musicSource.time;
    }

    public void StopMusicSingle()
    {
        _musicSource.Stop();
    }

    public void StopBgMusic()
    {
        _bgSource.Stop();
    }

    public void PlayMenuMusic(MenuSingleClip singleClip)
    {
        if (m_menuSingleClip[(int)singleClip])
        {
            _startSource.clip = m_menuSingleClip[(int)singleClip];
            _startSource.Play();
        }

    }

    public void StopStartMusic()
    {
        _startSource.Stop();
    }

    /// <summary>
    /// 播放多条音轨
    /// </summary>
    /// <param name="index"></param>
    public void PlayMulMusic(int[] index)
    {
        m_defaultSource.Play();
        for (int i=0;i<index.Length;i++)
        {
            m_audioSource[index[i]].Play();
        }
    }

    public void StopAudioMusic()
    {
        m_defaultSource.Stop();
        foreach (var i in m_audioSource)
        {
            i.Stop();
        }
    }

    public void PlayEffectMusic(AudioClip clip)
    {
        _efxSource.clip = clip;
        _efxSource.Play();
    }
    public void StopEffectMusic()
    {
        _efxSource.Stop();
    }

}