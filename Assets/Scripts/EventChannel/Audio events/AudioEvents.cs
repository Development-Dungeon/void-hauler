using System;
using UnityEngine;

public static class AudioEvents
{
    public static Action<AudioEvent, Vector3> OnSoundRequested;

    public static void RequestSound(AudioEvent audioEvent, Vector3 position)
    {
        OnSoundRequested?.Invoke(audioEvent, position);
    }
}