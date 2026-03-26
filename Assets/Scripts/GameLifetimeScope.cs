using UnityEngine;
using VContainer;
using VContainer.Unity;

public class NewMonoBehaviourScript : LifetimeScope 
{
    protected override void Configure(IContainerBuilder builder)
    {
        // We tell VContainer: "Look for the CharacterController on my Player and use it for injection."
        builder.RegisterComponentInHierarchy<CharacterController>();
        builder.RegisterComponentInHierarchy<PlayerMovement>();
    } 
}
