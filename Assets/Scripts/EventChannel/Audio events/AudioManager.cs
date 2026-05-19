using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Weapon Sounds")]
    public AudioClip laserShootClip;
    [Range(0f, 1f)] public float laserShootVolume = 0.4f;

    public AudioClip heavyLaserShootClip;
    [Range(0f, 1f)] public float heavyLaserShootVolume = 0.8f;

    [Header("Item Sounds")]
    public AudioClip itemPickupClip;
    [Range(0f, 1f)] public float itemPickupVolume = 0.2f;

    private void OnEnable()
    {
        AudioEvents.OnSoundRequested += HandleSoundRequest;
    }

    private void OnDisable()
    {
        AudioEvents.OnSoundRequested -= HandleSoundRequest;
    }

    private void HandleSoundRequest(AudioEvent audioEvent, Vector3 position)
    {
        switch (audioEvent)
        {
            case AudioEvent.LaserShoot:
                PlayClip(laserShootClip, laserShootVolume, position);
                break;

            case AudioEvent.HeavyLaserShoot:
                PlayClip(heavyLaserShootClip, heavyLaserShootVolume, position);
                break;

            case AudioEvent.ItemPickup:
                PlayClip(itemPickupClip, itemPickupVolume, position);
                break;
        }
    }

    private void PlayClip(AudioClip clip, float volume, Vector3 position)
    {
        if (clip == null) return;

        GameObject tempAudio = new GameObject("TempAudio");
        tempAudio.transform.position = position;

        AudioSource source = tempAudio.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;

        // Slight randomization
        source.pitch = Random.Range(0.96f, 1.04f);

        // 2D sound
        source.spatialBlend = 0f;

        source.Play();

        Destroy(tempAudio, clip.length);
    }
}