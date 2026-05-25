using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Upgrades
{
    [Serializable]
    public class UpgradeActionPair
    {
        [SerializeField]
        public UpgradeEntry upgradeEntry;
        public Action Action;
    }
    public class PlayerUpgradeController : MonoBehaviour
    {
        public UpgradesSo playerUpgrades;
    
        public UpgradeActionPair fuelUpgrade;
        public UpgradeActionPair thrusterUpgrade;
        public UpgradeActionPair boostUpgrade;

        private readonly List<UpgradeActionPair> _upgradeActionPairs = new();

        private void Start()
        {
            _upgradeActionPairs.Add(fuelUpgrade);
            _upgradeActionPairs.Add(thrusterUpgrade);
            _upgradeActionPairs.Add(boostUpgrade);
        
            _upgradeActionPairs.ForEach(u => SendEventFor(u, playerUpgrades));
        
        }

        private void SendEventFor(UpgradeActionPair upgradeEntry, UpgradesSo upgradesSo)
        {
            if(HasUpgrade(upgradeEntry.upgradeEntry, upgradesSo))
                upgradeEntry.Action?.Invoke();
        }

        private bool HasUpgrade(UpgradeEntry upgradeEntryToSearchFor, UpgradesSo upgradesToSearch)
        {
            if (upgradesToSearch == null || upgradesToSearch.upgrades == null || upgradeEntryToSearchFor == null)
            {
                return false;
            }

            return upgradesToSearch.upgrades.Any(u => u == upgradeEntryToSearchFor);
        }
    }
}