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

/*
 * TODO
 * 1. display the players coins
 * 2. display the players junk that they have in their pockets
 * 3. sell the junk for a price
 *      a. grey out when there is nothing left to sell
 * 4. display the upgrade for increasing speed
 * 5. buy the upgrade
 *      a. grey out when there is nothing left to sell
 */


/*
 * I need a method for displaying the upgrades in the list?
 * what if i created a prefab for the upgrade entry and then just init that?
 *  i think this can be v 2
 *  lets just get this working
 *
 */

namespace Upgrades
{
    [Serializable]
    public class ShopCatalogEntry
    {
        public ItemType itemType;
        public float salePrice;
    }

    public class ShopManager : MonoBehaviour
    {
        public List<ShopCatalogEntry> catalog = new();
        public Upgrades upgradeSo;

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
                var playerCanAffordUpgrade = playerInventory.CanRemove(ItemType.Money, firstUpgrade.price);
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

            var junk = playerInventory.items.Find(i => i.item.itemType.Equals(ItemType.Junk));

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

            var coins = playerInventory.items.Find(i => i.item.itemType.Equals(ItemType.Money));

            if (coins == null)
                return;

            coinText.text = "Coins : " + coins.count;
        }

        public void SellButtonJunk()
        {
            if (!playerInventory.CanRemove(ItemType.Junk, 1)) return;

            var catalogEntry = catalog.Find(e => e.itemType.Equals(ItemType.Junk));

            if (catalogEntry is not { salePrice: var price } || !playerInventory.CanAddItem(ItemType.Money, price))
                return;

            if (playerInventory.Remove(ItemType.Junk))
                playerInventory.Add(ItemType.Money, price);

            RefreshUI();
        }

        public void PurchaseUpgradeButton()
        {
            if (upgradeSo == null || upgradeSo.upgrades == null || upgradeSo.upgrades.Count == 0) return;

            if (playerInventory == null) return;

            var firstUpgrade = upgradeSo.upgrades.First();
            var price = firstUpgrade.price;
            
            if (!playerInventory.CanRemove(ItemType.Money, price)) return;
            
            planarForceMotor.boostUpgradeEnabled = true;
            firstUpgrade.purchased = true;
            playerInventory.Remove(ItemType.Money, price);
            
            RefreshUI();

        }
    }
}