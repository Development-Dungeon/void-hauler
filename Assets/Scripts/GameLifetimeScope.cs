using Attributes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Camera cam;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // We tell VContainer: "Look for the CharacterController on my Player and use it for injection."
        builder.RegisterComponentInHierarchy<CharacterController>();
        builder.RegisterComponentInHierarchy<PlayerMovement>();
        builder.RegisterComponent(playerHealth).Keyed(LifeTimeKeys.PlayerHealth);
        builder.RegisterComponent(cam);
    }

    public enum LifeTimeKeys 
    {
       PlayerHealth 
    }
}
