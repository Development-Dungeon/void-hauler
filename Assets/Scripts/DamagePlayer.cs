using EventChannel;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class DamagePlayer : ITickable
{
    
    private Health _playerHealth;
    private CountdownTimer _damageCounter;

    [Inject]
    public DamagePlayer(Health playerHealth)
    {
        _playerHealth = playerHealth;
        _damageCounter = new CountdownTimer(3);
        _damageCounter.OnTimerStop += DoDamage;
        _damageCounter.Start();
    }

    private void DoDamage()
    {
        _playerHealth.TakeDamage(1);
    }

    public void Tick()
    {
        if(_damageCounter.IsRunning)
            _damageCounter.Tick(Time.deltaTime);
    }
}