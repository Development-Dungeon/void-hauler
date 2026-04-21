using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "TierStrategy", menuName = "Items/Counters/TierStrategy")]
    public class InventoryCounterByTierStrategy : InventoryCounter
    {
        public List<ItemType> tiersToFilter;

        public override float Calculate(InventoryData inventoryData)
        {
            if (inventoryData == null || tiersToFilter == null || tiersToFilter.Count == 0)
                return 0;

            var count = inventoryData.items.Where(entry =>
                {
                    var itemFromInventoryType = entry.item.itemType.GetType();
                    return tiersToFilter.Exists(tier =>
                    {
                        var tierToFilter = tier.GetType();
                        return tierToFilter == itemFromInventoryType ||
                               tierToFilter.IsAssignableFrom(itemFromInventoryType);
                    });
                })
                .Sum(entry => entry.count);

            return count;
        }
    }
}