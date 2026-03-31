using System;

namespace Inventory
{
    [Serializable]
    public class Item
    {

        public string itemName;

        public Item(string _itemName)
        {
            itemName = _itemName;
        }
        
        protected bool Equals(Item other)
        {
            return itemName == other.itemName;
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
            return (itemName != null ? itemName.GetHashCode() : 0);
        }
        
    }
}