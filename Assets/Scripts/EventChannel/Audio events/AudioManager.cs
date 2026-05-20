using System;
using System.Diagnostics.Tracing;
using Enemy;
using EventChannel.concrete;
using EventChannel.templates;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace EventChannel.Audio_events
{
    public class AudioManager : MonoBehaviour
    {
        public void OnItemPickupChannel(InventoryEventContext inventoryEventContext)
        {
            PlayClip(inventoryEventContext.itemType.pickUpSound, inventoryEventContext.itemType.pickUpSoundVolume, inventoryEventContext.position);
        }

        public void OnFireChannel(EnemyStateContext enemyStateContext)
        {
            PlayClip(enemyStateContext.weapon.fireSound, enemyStateContext.weapon.fireVolume, enemyStateContext.position);
        }
        
        public void OnLockOnChannel(EnemyStateContext enemyStateContext)
        {
            PlayClip(enemyStateContext.weapon.lockOnSound, enemyStateContext.weapon.lockOnVolume, enemyStateContext.position);
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