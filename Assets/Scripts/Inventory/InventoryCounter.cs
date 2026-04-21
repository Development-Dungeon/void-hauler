using UnityEngine;

namespace Inventory
{
    public abstract class InventoryCounter : ScriptableObject
    {
        public abstract float Calculate(InventoryData inventoryData);
    }
}