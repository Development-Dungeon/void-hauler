using EventChannel;
using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    // what do i need in order to i will need the channel

    public EventListener<float> playerHealthEventChannel;
    
    // this is going to be a fake ui controller which is going to be used to test out the channels
    // in this example i'm going to write to the console every time i receive an event 

    public void OnPlayerHealthChanged(float health)
    {
        Debug.Log("health percentage is " + health);
    }

}
