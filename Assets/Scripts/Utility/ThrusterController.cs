using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ThrusterController : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer thrusterSprite;

    [Header("Audio")]
    public AudioSource loopAudio;

    [Header("Tuning")]
    public float fadeInTime = 0.15f;
    public float fadeOutTime = 0.2f;

    private bool isThrusting = false;
    private bool wasThrusting = false;

    private Coroutine fadeRoutine;

    void Start()
    {
        if (thrusterSprite != null)
            thrusterSprite.enabled = false;

        if (loopAudio != null)
        {
            loopAudio.loop = true;
            loopAudio.playOnAwake = false;
            loopAudio.volume = 0f;
        }
    }

    void Update()
    {
        isThrusting = GetInput();

        // Visual
        if (thrusterSprite != null)
            thrusterSprite.enabled = isThrusting;

        // State change handling
        if (isThrusting && !wasThrusting)
        {
            StartThruster();
        }
        else if (!isThrusting && wasThrusting)
        {
            StopThruster();
        }

        wasThrusting = isThrusting;
    }

    bool GetInput()
    {
        bool key = Keyboard.current != null && Keyboard.current.wKey.isPressed;
        bool touch = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed;
        return key || touch;
    }

    void StartThruster()
    {
        if (loopAudio == null) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        loopAudio.volume = 0f;

        if (!loopAudio.isPlaying)
            loopAudio.Play();

        fadeRoutine = StartCoroutine(Fade(loopAudio, 1f, fadeInTime));
    }

    void StopThruster()
    {
        if (loopAudio == null) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(loopAudio, 0f, fadeOutTime, true));
    }

    IEnumerator Fade(AudioSource audio, float targetVolume, float duration, bool stopAfter = false)
    {
        float startVolume = audio.volume;
        float time = 0f;

        while (time < duration)
        {
            // If player starts thrusting again during fade-out, cancel it
            if (targetVolume == 0f && GetInput())
                yield break;

            time += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }

        audio.volume = targetVolume;

        if (stopAfter && Mathf.Approximately(targetVolume, 0f))
        {
            audio.Stop();
        }
    }
}