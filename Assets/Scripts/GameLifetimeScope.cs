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
        builder.RegisterComponentInHierarchy<PlayerMovement>();
        builder.RegisterComponent(playerHealth);
        builder.RegisterComponent(cam);
    }
}
