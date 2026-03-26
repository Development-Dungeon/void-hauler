using UnityEngine;
using VContainer;
using VContainer.Unity;

public class NewMonoBehaviourScript : LifetimeScope 
{
    protected override void Configure(IContainerBuilder builder)
    {
        // No RegisterComponentInHierarchy here — PlayerMovement resolves CharacterController
        // and PlayerFuel via GetComponent. Re-add registrations when you use [Inject] again.
    }
}
