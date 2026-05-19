using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AmbientMusicManager : MonoBehaviour
{
    [Header("Music")]
    public List<AudioClip> musicTracks = new List<AudioClip>();

    [Header("Timing")]
    public float minWaitTime = 30f;
    public float maxWaitTime = 120f;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Debug")]
    public Key debugPlayKey = Key.M;

    private AudioClip lastTrack;
    private Coroutine musicRoutine;

    private bool isPlayingTrack = false;

    private void Start()
    {
        musicRoutine = StartCoroutine(MusicLoop());
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[debugPlayKey].wasPressedThisFrame)
        {
            ForcePlayRandomTrack();
        }
    }

    private IEnumerator MusicLoop()
    {
        while (true)
        {
            // Wait between tracks (silence window)
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Hard gate: ensure nothing is playing
            yield return new WaitUntil(() => !isPlayingTrack);

            PlayRandomTrack();

            // Wait until track fully finishes
            yield return new WaitUntil(() => !audioSource.isPlaying);

            isPlayingTrack = false;
        }
    }

    private void ForcePlayRandomTrack()
    {
        StopAllCoroutines();
        isPlayingTrack = false;

        PlayRandomTrack();

        musicRoutine = StartCoroutine(MusicLoop());
    }

    private void PlayRandomTrack()
    {
        if (isPlayingTrack)
            return;

        AudioClip nextTrack = GetRandomTrack();

        if (nextTrack == null)
            return;

        isPlayingTrack = true;

        audioSource.Stop();
        audioSource.clip = nextTrack;
        audioSource.Play();

        Debug.Log("Now Playing: " + nextTrack.name);
    }

    private AudioClip GetRandomTrack()
    {
        if (musicTracks.Count == 0)
            return null;

        AudioClip randomTrack;

        do
        {
            randomTrack = musicTracks[Random.Range(0, musicTracks.Count)];
        }
        while (randomTrack == lastTrack && musicTracks.Count > 1);

        lastTrack = randomTrack;
        return randomTrack;
    }
}