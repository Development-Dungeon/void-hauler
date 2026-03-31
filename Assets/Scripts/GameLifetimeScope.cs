using Attributes;
using player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Health playerHealth;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // We tell VContainer: "Look for the CharacterController on my Player and use it for injection."
        builder.RegisterComponentInHierarchy<CharacterController>();
        builder.RegisterComponentInHierarchy<PlayerMovementController>();
        builder.RegisterComponent(playerHealth).Keyed(LifeTimeKeys.PlayerHealth);

    }

    public enum LifeTimeKeys 
    {
       PlayerHealth 
    }
}
