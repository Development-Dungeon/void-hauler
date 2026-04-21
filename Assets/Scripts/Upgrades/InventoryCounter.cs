using Inventory;
using UnityEngine;

namespace Upgrades
{
    public abstract class InventoryCounter : ScriptableObject
    {
        public abstract float Calculate(InventoryData inventoryData);
    }
}