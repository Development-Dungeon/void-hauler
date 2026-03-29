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
        builder.RegisterComponentInHierarchy<PlayerMovement>();
        builder.RegisterComponent(playerHealth).Keyed(LifeTimeKeys.PLAYER_HEALTH);

        builder.RegisterEntryPoint<DamagePlayer>();
    }

    public enum LifeTimeKeys 
    {
       PLAYER_HEALTH 
    }
}
