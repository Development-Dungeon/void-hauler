using System.Linq;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "ItemCounterStrategy", menuName = "Items/Counters/ItemCounterStrategy")]
    public class ItemCounterStrategy : InventoryCounter
    {
        public ItemType itemToFilterBy;

        public override float Calculate(InventoryData inventoryData)
        {
            if (!(inventoryData?.items?.Count > 0)) return 0;

            return inventoryData
                .FindAll(itemToFilterBy)
                .Sum(entry => entry.count);
        }
    }
}