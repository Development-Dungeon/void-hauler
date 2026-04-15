using System;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class Item
    {
        public ItemType itemType;

        public Item(ItemType itemType)
        {
            this.itemType = itemType;
        }

        protected bool Equals(Item other)
        {
            return itemType == other.itemType;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Item)obj);
        }

        public override int GetHashCode()
        {
            return (itemType != null ? itemType.GetHashCode() : 0);
        }
    }
}