using System;
using System.Collections.Generic;
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
}