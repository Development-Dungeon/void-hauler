using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Upgrades
{

    [CreateAssetMenu(fileName = "UpgradeSO", menuName = "Upgrades/Upgrades")]
    public class UpgradesSo : ScriptableObject
    {
        [SerializeField]
        public List<UpgradeEntry> upgrades;
    }
    
    [Serializable]
    public class UpgradeEntry
    {
        public string name;
        public string description;
        public ItemType itemCost;
        public float price;
        public bool purchased;
        public string stateDescription;
        public Sprite icon;
    }
}