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
    
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<PlayerMovement>();
        builder.RegisterComponent(cam);
        builder.RegisterComponent(playerHealthData);
        builder.RegisterComponent(playerMovementData);
        builder.RegisterComponent(playerInventoryData);
    }
}
