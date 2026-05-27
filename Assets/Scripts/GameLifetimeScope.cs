using Attributes;
using Inventory;
using player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Camera cam;
    [SerializeField] private HealthData playerHealthData;
    [SerializeField] private MovementData playerMovementData;
    [SerializeField] private InventoryData playerInventoryData;
    [SerializeField] private FuelData playerFuelData;
    [SerializeField] private Inventory.Inventory playerInventory;
    [SerializeField] private PlayerGunController playerGunController;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<PlayerMovement>();
        builder.RegisterComponentInHierarchy<PlayerMovementController>();
        builder.RegisterComponent(cam);
        builder.RegisterComponent(playerHealthData);
        builder.RegisterComponent(playerMovementData);
        builder.RegisterComponent(playerInventoryData);
        builder.RegisterComponent(playerFuelData);
        builder.RegisterComponent(playerInventory);
        builder.RegisterComponent(playerGunController);
    }
}
