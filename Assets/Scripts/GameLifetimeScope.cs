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
        // Keyed Health breaks at runtime: RegisterComponent<T> adds a build callback that calls
        // Resolve<Health>() with no key, so keyed-only registration fails. Uncomment only if you
        // switch to a registration path that resolves by key (or use [Key] on inject sites).
        // builder.RegisterComponent(playerHealth).Keyed(LifeTimeKeys.PlayerHealth);
        builder.RegisterComponent(playerHealth);
        builder.RegisterComponent(cam);
    }

    // Used with keyed Health registration above (currently commented out).
    // public enum LifeTimeKeys
    // {
    //     PlayerHealth
    // }
}
