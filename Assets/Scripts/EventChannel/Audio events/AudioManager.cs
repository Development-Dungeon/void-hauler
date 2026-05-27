using System;
using Enemy;
using Inventory;
using player;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace EventChannel.Audio_events
{
    [Serializable]
    public class AudiMapper
    {
        public AudioClip audioClip;
        [Range(0,1)]
        public float volume;

        public AudiMapper(AudioClip audioClip, float volume)
        {
            this.audioClip = audioClip;
            this.volume = volume;
        }
    }
    public class AudioManager : MonoBehaviour
    {
        [Header("Weapon Sounds")] 
        public AudiMapper laserShootClip;
        public AudiMapper heavyLaserShootClip;

        [Header("Item Sounds")] 
        public AudiMapper itemPickupClip;

        public AudiMapper playerGun;
        [Header("Sources")]
        [Inject]
        [SerializeField] 
        private Inventory.Inventory playerInventory;
        [Inject]
        [SerializeField] 
        private PlayerGunController gunController;

        private void Start()
        {
            playerInventory.OnItemAdded += OnItemAdded;
            EnemyT1AIController.OnEngageState += OnLaserShoot;
            EnemyT2AIController.OnEngageState += OnHeavyLaserShoot;
            gunController.OnFire += OnPlayerFire;
        }

        private void OnPlayerFire(Vector3 obj)
        {
            PlayClip(playerGun.audioClip, playerGun.volume, obj);
        }

        private void OnDestroy()
        {
            playerInventory.OnItemAdded -= OnItemAdded;
            EnemyT1AIController.OnEngageState -= OnLaserShoot;
            EnemyT2AIController.OnEngageState -= OnHeavyLaserShoot;
        }

        private void OnHeavyLaserShoot(EnemyStateChangeContext obj)
        {
            PlayClip(heavyLaserShootClip.audioClip, heavyLaserShootClip.volume, obj.Position);
        }
        
        private void OnLaserShoot(EnemyStateChangeContext obj)
        {
            PlayClip(laserShootClip.audioClip, laserShootClip.volume, obj.Position);
        }

        private void OnItemAdded(InventoryEventContext obj)
        {
            if (obj.position == null) return;
            
            PlayClip(itemPickupClip.audioClip, itemPickupClip.volume, obj.position.Value);
        }

        private void PlayClip(AudioClip clip, float volume, Vector3 position)
        {
            if (!clip) return;

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
}