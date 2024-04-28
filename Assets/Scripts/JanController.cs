using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Linq;

public class JanController : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private VideoClip introClip;
    [SerializeField] private List<VideoClip> realSuccess;
    [SerializeField] private List<VideoClip> realFailure;
    [SerializeField] private List<VideoClip> fakeSuccess;
    [SerializeField] private List<VideoClip> fakeFailure;
    [SerializeField] private List<VideoClip> idle;
    [SerializeField] private GameProcessor processor;
    [SerializeField] private VideoClip staticClip;
    private int idleIndex;
    private VideoClip nextClip;
    private bool playingStatic = false;
    private float staticDelay = 0f;

    private void Awake()
    {
        idleIndex = 0;
        player.clip = introClip;
        player.Play();
        StartCoroutine(StartGame());
        idle = idle.OrderBy(x => Random.value).ToList();
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds((float)introClip.length);
        processor.SetDoUpdate(true);
    }

    private void Update()
    {
        if (playingStatic)
        {
            if (player.clockTime >= staticDelay)
            {
                player.clip = nextClip;
                playingStatic = false;
            }
        }
        else if (true && (player.isPrepared && !player.isPlaying && player.clockTime >= .2f))
        {
            nextClip = GetRandomClip(idle);
            PlayStatic();
        }
    }

    public void PlayReaction(bool isReal, bool isSuccess)
    {
        if (isReal)
        {
            if (isSuccess)
            {
                nextClip = GetRandomClip(realSuccess);
            }
            else
            {
                nextClip = GetRandomClip(realFailure);
            }
        }
        else
        {
            if (isSuccess)
            {
                nextClip = GetRandomClip(fakeSuccess);
            }
            else
            {
                nextClip = GetRandomClip(fakeFailure);
            }
        }
        PlayStatic();
    }

    private void PlayStatic()
    {
        player.clip = staticClip;
        playingStatic = true;
        staticDelay = UnityEngine.Random.Range(.25f, .35f);
        if (!player.isPlaying)
        {
            player.Play();
        }

    }

    private VideoClip GetRandomClip(List<VideoClip> clips)
    {
        if (clips == idle)
        {
            ++idleIndex;
            idleIndex %= idle.Count;
            return clips[idleIndex];
        }
        return clips[UnityEngine.Random.Range(0, clips.Count)];
    }
    

}
