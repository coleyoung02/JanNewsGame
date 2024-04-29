using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Channel
{
    SFX,
    MUSIC
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private float pitchShiftAmmount;
    [SerializeField] private List<AudioSource> SFXsources;
    private int sfxIndex;
    [SerializeField] private AudioSource MusicSource;
    private float trackTimer;

    [Header("Clips")]
    [SerializeField] private AudioClip buttonHover;
    [SerializeField] private AudioClip buttonPress;
    [SerializeField] private AudioClip correct;
    [SerializeField] private AudioClip incorrect;
    [SerializeField] private AudioClip[] songs;
    private bool[] songsPlayed;
    private int numSongsPlayed;

    [Header("Sound Control")]
    [SerializeField] private float musicVolume;

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            songsPlayed = new bool[songs.Length];
            for (int i = 0; i < songsPlayed.Length; i++)
                songsPlayed[i] = false;
        }
    }

    public void PlayClip(Channel channel, AudioClip clip, float sfxShiftOveride = 1f)
    {
        AudioSource s = null;
        if (channel == Channel.MUSIC)
        {
            s = MusicSource;
        }
        if (channel == Channel.SFX)
        {
            s = SFXsources[sfxIndex];
            sfxIndex = ++sfxIndex % SFXsources.Count;
            s.pitch = UnityEngine.Random.Range(1f - pitchShiftAmmount * sfxShiftOveride, 1 + pitchShiftAmmount * sfxShiftOveride);
        }
        s.clip = clip;
        s.Play();
    }

    public void ChangeSong(int songIndex)
    {
        trackTimer = 0;
        PlayClip(Channel.MUSIC, songs[songIndex]);
    }

    public void PlayButtonHover()
    {
        PlayClip(Channel.SFX, buttonHover);
    }

    public void PlayButtonPressed()
    {
        PlayClip(Channel.SFX, buttonPress);
    }

    public void PlaySuccess()
    {
        StartCoroutine(PlayAfterDelay(correct));
    }

    public void PlayError()
    {
        StartCoroutine(PlayAfterDelay(incorrect));
    }

    private IEnumerator PlayAfterDelay(AudioClip toPlay)
    {
        yield return new WaitForSeconds(.3f);
        PlayClip(Channel.SFX, toPlay, .25f);
    }

    void Update()
    {
        MusicSource.volume = musicVolume;
        if (songs.Length > 0)
        {
            if (MusicSource.isPlaying)
                trackTimer += 1 * Time.deltaTime;

            if (!MusicSource.isPlaying || (MusicSource.clip != null && trackTimer >= MusicSource.clip.length))
            {
                int songIndex = UnityEngine.Random.Range(0, songs.Length);
                while (songsPlayed[songIndex])
                    songIndex = UnityEngine.Random.Range(0, songs.Length);
                numSongsPlayed++;
                songsPlayed[songIndex] = true;
                Debug.Log("Song Played: " + songIndex);
                ChangeSong(songIndex);
            }

            if (numSongsPlayed >= songs.Length)
            {
                numSongsPlayed = 0;
                for (int i = 0; i < songsPlayed.Length; i++)
                    songsPlayed[i] = false;
            }
        }
    }
}
