using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundClip
{
    bowPull,
    arrowRelease,
    daggerHit,
    playerKilled
}

public class AudioPlayer : MonoBehaviour
{
    public static void PlayCrickets(bool _play) { if (_play) { instance.CricketsSource.Play(); } else { instance.CricketsSource.Stop(); } }

    public static void PlaySfxClip(SoundClip _clip) { instance.sfxSource.PlayOneShot(instance.clips[(int)_clip]); }

    [SerializeField]
    AudioSource sfxSource;
    public AudioSource SfxSource { get { return sfxSource; } }

    [SerializeField]
    AudioSource cricketsSource;
    public AudioSource CricketsSource { get { return cricketsSource; } }

    public AudioClip[] clips = new AudioClip[3];

    private static AudioPlayer instance = null;
    public static AudioPlayer Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
