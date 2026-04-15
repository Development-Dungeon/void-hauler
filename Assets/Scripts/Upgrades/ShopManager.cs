using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Attributes;
using Inventory;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Upgrades
{
    [Serializable]
    public class ShopCatalogEntry
    {
        public ItemType itemForSale;
        public float salePrice;
        public ItemType saleItemType;
    }

    public class ShopManager : MonoBehaviour
    {
        public List<ShopCatalogEntry> catalog = new();
        public Upgrades upgradeSo;
        public ItemType coinItemType;
        public ItemType sellJunkItemType;

        // Player Data
        public HealthData playerHealth;
        public PlanarForceMotorData planarForceMotor;
        public InventoryData playerInventory;

        // UI data
        public TMP_Text coinText;
        public TMP_Text junkText;
        public Button sellJunkButton;
        public TMP_Text upgradeText;
        public TMP_Text upgradeCostText;
        public Button upgradeBuyButton;

        private void Start()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            sellJunkButton.interactable = false;
            upgradeBuyButton.interactable = false;

            SetCoinText();
            SetJunkText();
            SetUpgrade();
        }

        private void SetUpgrade()
        {
            if (upgradeSo == null || upgradeSo.upgrades == null || upgradeSo.upgrades.Count <= 0)
                return;

            var firstUpgrade = upgradeSo.upgrades.First();

            if (upgradeText != null)
                upgradeText.text = firstUpgrade.name;
            if (upgradeCostText != null)
                upgradeCostText.text = firstUpgrade.price.ToString();
            if (upgradeBuyButton != null)
            {
                var playerCanAffordUpgrade = playerInventory.CanRemove(firstUpgrade.itemCost, firstUpgrade.price);
                upgradeBuyButton.interactable = playerCanAffordUpgrade && !firstUpgrade.purchased;
            }
        }

        private void SetJunkText()
        {
            sellJunkButton.interactable = false;

            if (junkText == null)
                return;

            junkText.text = "Junk : 0";

            if (playerInventory == null)
                return;

            var junk = playerInventory.items.Find(i => i.item.itemType.Equals(sellJunkItemType));

            if (junk == null)
                return;

            junkText.text = "Junk : " + junk.count;

            sellJunkButton.interactable = junk.count >= 1;
        }

        private void SetCoinText()
        {
            if (coinText == null)
                return;

            coinText.text = "Coins : 0";

            if (playerInventory == null)
                return;

            var coins = playerInventory.items.Find(i => i.item.itemType.Equals(coinItemType));

            if (coins == null)
                return;

            coinText.text = "Coins : " + coins.count;
        }

        public void SellButtonJunk()
        {
            if (!playerInventory.CanRemove(sellJunkItemType, 1)) return;

            var catalogEntry = catalog.Find(e => e.itemForSale.Equals(sellJunkItemType));

            if (catalogEntry is not { salePrice: var price } || !playerInventory.CanAddItem(catalogEntry.saleItemType, price))
                return;

            if (playerInventory.Remove(sellJunkItemType))
                playerInventory.Add(catalogEntry.saleItemType, price);

            RefreshUI();
        }

        public void PurchaseUpgradeButton()
        {
            if (upgradeSo == null || upgradeSo.upgrades == null || upgradeSo.upgrades.Count == 0) return;

            if (playerInventory == null) return;

            var firstUpgrade = upgradeSo.upgrades.First();
            var price = firstUpgrade.price;
            
            if (!playerInventory.CanRemove(firstUpgrade.itemCost, price)) return;
            
            planarForceMotor.boostUpgradeEnabled = true;
            firstUpgrade.purchased = true;
            playerInventory.Remove(firstUpgrade.itemCost, price);
            
            RefreshUI();

        }
    }
}