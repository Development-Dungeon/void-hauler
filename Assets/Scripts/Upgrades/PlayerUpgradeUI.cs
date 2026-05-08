using System;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class PlayerUpgradeUI : MonoBehaviour
    {
        public GameObject upgradeIconPrefab;
        public UpgradesSo playerUpgradesSo; 

        private void Start()
        {
            if (playerUpgradesSo == null || playerUpgradesSo.upgrades == null)
                return;
            
            foreach (var playerUpgrade in playerUpgradesSo.upgrades)
            {
                var upgradeObject = Instantiate(upgradeIconPrefab, transform);
                var icon = upgradeObject.GetComponent<Image>();
                icon.sprite = playerUpgrade.icon;
            }
        }
    }
}
