using System;
using Inventory;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;
using Utility;

public class SellJunkButtonController : MonoBehaviour
{
    public ShopManager shopManager;
    public InventoryData playerInventory;
    [Get] 
    public Button sellJunkButton;


    private void Awake()
    {
        sellJunkButton.interactable = false;
        playerInventory.OnItemAdded += OnItemAdded;
        playerInventory.OnItemRemoved += OnItemRemoved;
    }

    private void OnItemRemoved(ItemType arg1, float arg2)
    {
        sellJunkButton.interactable = shopManager.CanSell(playerInventory);
    }

    private void OnItemAdded(ItemType arg1, float arg2)
    {
        sellJunkButton.interactable = shopManager.CanSell(playerInventory);
    }

    void Start()
    {
        sellJunkButton.interactable = shopManager.CanSell(playerInventory);
    }

    private void OnDestroy()
    {
        playerInventory.OnItemAdded -= OnItemAdded;
        playerInventory.OnItemRemoved -= OnItemRemoved;
    }
}
