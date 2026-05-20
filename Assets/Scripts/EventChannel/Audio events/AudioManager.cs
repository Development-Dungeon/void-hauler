using System;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AudiotEventMapper {
    public AudioClip audioClip;
    [Range(0f, 1f)] public float volume;
}
public class AudioManager : MonoBehaviour
{
    public AudiotEventMapper laserShooterMapper;
    public AudiotEventMapper heavyLaserShooterMapper;
    public AudiotEventMapper itemPickupMapper;
    public Inventory.Inventory playerInventory;

    private void Awake()
    {
        EnemyT1AIController.onFireEvent += OnFireLaserShoot;
        EnemyT2AIController.OnFireEvent += OnFireHeavyLaserShoot;
    }

    private void Start()
    {
        if (!playerInventory) return;
        playerInventory.OnItemAdded += OnItemAdded;
    }

    private void OnItemAdded(GameObject obj)
    {
        PlayClip(itemPickupMapper.audioClip, itemPickupMapper.volume, obj.transform.position);
    }



    private void OnFireHeavyLaserShoot(GameObject obj)
    {
        PlayClip(heavyLaserShooterMapper.audioClip, heavyLaserShooterMapper.volume, obj.transform.position);
    }
    private void OnFireLaserShoot(GameObject obj)
    {
        PlayClip(laserShooterMapper.audioClip, laserShooterMapper.volume, obj.transform.position);
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
    
    private void OnDestroy()
    {
        EnemyT1AIController.onFireEvent -= OnFireLaserShoot;
        EnemyT2AIController.OnFireEvent -= OnFireHeavyLaserShoot;
        playerInventory.OnItemAdded -= OnItemAdded;
    }
}