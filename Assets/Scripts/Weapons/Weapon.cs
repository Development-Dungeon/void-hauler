using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Weapon")]
    public class Weapon : ScriptableObject
    {
        public AudioClip lockOnSound;
        [Range(0f, 1f)]
        public float lockOnVolume;
        public AudioClip fireSound;
        [Range(0f, 1f)]
        public float fireVolume;
        public GameObject bulletPrefab;
    }
}