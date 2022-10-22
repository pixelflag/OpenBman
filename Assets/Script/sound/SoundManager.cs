using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private SoundResource resource = default;

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    private const int SE_CHANNEL = 4;

    private AudioSource bgmAudioSource;
    private AudioSource jingleAudioSource;
    private AudioSource[] seAudioSource;

    public float fadeSpeed = 0.001f;
    public float bgmVolume = 0.5f;
    public float jingleVolume = 0.5f;

    public float currentBgmVolume;
    public float seVolume = 1.0f;

    private bool isPause = false;
    public BgmType currentBgm { get; private set; }
    private BgmType nextBgm;

    private FadeState fadeState;

    public void Initialize()
    {
        currentBgmVolume = bgmVolume;

        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;

        jingleAudioSource = gameObject.AddComponent<AudioSource>();
        jingleAudioSource.loop = false;

        seAudioSource = new AudioSource[SE_CHANNEL];
        for(int i=0; i < SE_CHANNEL; i++)
            seAudioSource[i] = gameObject.AddComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(!isPause)
        {
            switch(fadeState)
            {
                case FadeState.None:
                    break;
                case FadeState.FadeInOut:
                    bgmAudioSource.volume -= fadeSpeed;
                    if(bgmAudioSource.volume <= 0)
                    {
                        BGMFadeIn(nextBgm);
                    }
                    break;
                case FadeState.FadeIn:
                    bgmAudioSource.volume += fadeSpeed;
                    if (bgmVolume < bgmAudioSource.volume)
                    {
                        bgmAudioSource.volume = bgmVolume;
                        fadeState = FadeState.None;
                    }
                    break;
                case FadeState.FadeOut:
                    bgmAudioSource.volume -= fadeSpeed;
                    if (bgmAudioSource.volume <= 0)
                    {
                        bgmAudioSource.Stop();
                        fadeState = FadeState.None;
                    }
                    break;
            }
        }
    }

    public void PlayBgm(BgmType type)
    {
        currentBgm = type;

        bgmAudioSource.clip = resource.GetBgm(type);
        bgmAudioSource.volume = bgmVolume;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();

        fadeState = FadeState.None;
    }

    public void BGMFadeOutAndPlayIn(BgmType type)
    {
        nextBgm = type;
        fadeState = FadeState.FadeInOut;
    }

    public void BGMFadeOutAndStop()
    {
        fadeState = FadeState.FadeOut;
    }

    public void BGMFadeIn(BgmType type)
    {
        currentBgm = type;

        bgmAudioSource.clip = resource.GetBgm(type);
        bgmAudioSource.volume = 0;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();

        fadeState = FadeState.FadeIn;
    }

    public void StopBgm()
    {
        currentBgm = BgmType.None;
        bgmAudioSource.Stop();
    }

    public void PauseBgm()
    {
        bgmAudioSource.Pause();
        isPause = true;
    }

    public void ResumeBgm()
    {
        bgmAudioSource.UnPause();
        isPause = false;
    }

    // Jingle -----

    public void PlayJingle(BgmType type, float volumeGain)
    {
        jingleAudioSource.clip = resource.GetBgm(type);
        jingleAudioSource.volume = jingleVolume * volumeGain;
        jingleAudioSource.Play();

        fadeState = FadeState.None;
    }

    public void StopJingle()
    {
        jingleAudioSource.Stop();
    }

    // Effect -----

    public void PlayOneShot(SeType type, float volume)
    {
        seAudioSource[0].PlayOneShot(resource.GetEffect(type), volume * seVolume);
    }

    public void PlayOneShotOnChannel(int channel, SeType type, float volume)
    {
        seAudioSource[channel].Stop();
        seAudioSource[channel].PlayOneShot(resource.GetEffect(type), volume * seVolume);
    }
}
