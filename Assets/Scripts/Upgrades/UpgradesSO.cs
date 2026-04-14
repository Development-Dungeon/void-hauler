using System;
using System.Collections.Generic;
using EventChannel.templates;
using UnityEngine;

namespace Upgrades
{

    [CreateAssetMenu(fileName = "Upgrades", menuName = "Upgrades/Upgrades")]
    public class Upgrades : ScriptableObject
    {
        [SerializeField]
        public List<UpgradeEntry> upgrades;
    }
    
    [Serializable]
    public class UpgradeEntry
    {
        public string name;
        public string description;
        public float price;
        public bool purchased;
    }
}