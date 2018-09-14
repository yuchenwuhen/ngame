using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;


    public static AudioManager Instance
    {
        get { return _instance; }
    }
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public AudioSource efxSource;
    public AudioSource bgSource;
    public AudioSource musicSource;
    public AudioSource[] m_audioSource;


    void Awake()
    {
        _instance = this;
    }

    public void RandomPlay(params AudioClip[] clips)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play();
    }

    public void PlayMusicSingle(AudioClip audioClip)
    {
        musicSource.Stop();
        musicSource.clip = audioClip;
        musicSource.Play();
        Debug.Log("总时长" + audioClip.length);
    }

    public float GetMusicSourceTime()
    {
        return musicSource.time;
    }

    public void StopMusicSingle()
    {
        musicSource.Stop();
    }

    public void StopBgMusic()
    {
        bgSource.Stop();
    }

    /// <summary>
    /// 播放多条音轨
    /// </summary>
    /// <param name="index"></param>
    public void PlayMulMusic(int[] index)
    {
        for(int i=0;i<index.Length;i++)
        {
            m_audioSource[index[i]].Play();
        }
    }

    public void StopAudioMusic()
    {
        foreach(var i in m_audioSource)
        {
            i.Stop();
        }
    }

    public void PlayEffectMusic(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }
    public void StopEffectMusic()
    {
        efxSource.Stop();
    }

}