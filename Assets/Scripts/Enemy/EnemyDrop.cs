using System;
using System.Collections.Generic;
using Debris;
using Inventory;
using UnityEngine;

namespace Enemy
{

    [Serializable]
    public class DropMetadata
    {
        [SerializeField]
        public OnTouchCollect ItemPrefab; 
        [SerializeField]
        public int Amount;

        public DropMetadata(OnTouchCollect itemPrefab, int amount)
        {
            ItemPrefab = itemPrefab;
            Amount = amount;
        }
    }
    
    public class EnemyDrop : MonoBehaviour
    {
        [SerializeField]
        public List<DropMetadata> Drops = new();
        
        public void Drop()
        {
            foreach (var dropMetadata in Drops)
            {
                var item = Instantiate(dropMetadata.ItemPrefab, transform.position, Quaternion.identity);
                item.SetQuantity(dropMetadata.Amount);
            }

        }
    }
}