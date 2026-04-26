using Attributes;
using Inventory;
using UnityEngine;
using Upgrades;

public class SaveManger : MonoBehaviour
{
    public InventoryData inventoryData;
    public Item money;
    public Item tier1;

    public Upgrades.Upgrades upgrade1;
    public Upgrades.Upgrades upgrade2;
    public Upgrades.Upgrades upgrade3;
    
    public PlanarForceMotorData planarForceMotorData;
    

    public void Load()
    {
        inventoryData.items = new ();
        inventoryData.items.Add(new InventoryEntry(money, 50));
        inventoryData.items.Add(new InventoryEntry(tier1, 50));
        
        upgrade1.upgrades.ForEach(u => u.purchased = false);
        upgrade2.upgrades.ForEach(u => u.purchased = false);
        upgrade3.upgrades.ForEach(u => u.purchased = false);

        planarForceMotorData.boostUpgradeEnabled = false;

    }

}
