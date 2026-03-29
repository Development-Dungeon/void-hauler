using Attributes;
using player;
using test;
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
        builder.RegisterComponentInHierarchy<Movement>();
        builder.RegisterComponent(playerHealth).Keyed(LifeTimeKeys.PlayerHealth);

        builder.RegisterEntryPoint<DamagePlayer>();
    }

    public enum LifeTimeKeys 
    {
       PlayerHealth 
    }
}
