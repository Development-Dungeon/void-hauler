using System;
using UnityEngine;

namespace Inventory
{
    
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class ItemType : ScriptableObject
    {
        public AudioClip pickUpSound;
        [Range(0,1)]
        public float pickUpSoundVolume;
    }
}