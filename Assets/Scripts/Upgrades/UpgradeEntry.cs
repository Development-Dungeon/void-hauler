using Inventory;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeSO", menuName = "Upgrades/UpgradeEntry")]
    public class UpgradeEntry : ScriptableObject
    {
        public string upgradeName;
        public string description;
        public ItemType itemCost;
        public float price;
        public bool purchased;
        public string stateDescription;
        public Sprite icon;
    }
}