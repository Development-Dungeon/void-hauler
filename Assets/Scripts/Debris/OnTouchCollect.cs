using Inventory;
using player;
using UnityEngine;

namespace Debris
{
    public class OnTouchCollect : MonoBehaviour
    {
        public Item Item;
        private void OnTriggerEnter(Collider other)
        {
            if (Item == null)
                return;
            
            var inventory = other.gameObject.GetComponent<Inventory.Inventory>();
            
            if (inventory == null)
                return;
            
            var added = inventory.AddItem(Item);

            if (added)
                Destroy(gameObject);

        }
    }
}