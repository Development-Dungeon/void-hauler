using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "MerchantShopData", menuName = "Shop/Merchant Shop Data")]
    public class MerchantShopData : ScriptableObject
    {
        public List<ShopCatalogEntry> catalogByTier = new();
        public Upgrades upgradeSo;
        public LaunchCost launchCost;
    }
}