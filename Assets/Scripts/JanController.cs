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
    private int idleIndex;
    private bool hasStarted = false;

    private void Awake()
    {
        idleIndex = 0;
        player.clip = introClip;
        player.Play();
        hasStarted = true;
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
            Debug.Log(player.time);
        if (true && (player.isPrepared && !player.isPlaying && player.clockTime >= .2f))
        {
            player.clip = GetRandomClip(idle);
            player.Play();
        }
    }

    public void PlayReaction(bool isReal, bool isSuccess)
    {
        if (isReal)
        {
            if (isSuccess)
            {
                player.clip = GetRandomClip(realSuccess);
            }
            else
            {
                player.clip = GetRandomClip(realFailure);
            }
        }
        else
        {
            if (isSuccess)
            {
                player.clip = GetRandomClip(fakeSuccess);
            }
            else
            {
                player.clip = GetRandomClip(fakeFailure);
            }
        }
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
