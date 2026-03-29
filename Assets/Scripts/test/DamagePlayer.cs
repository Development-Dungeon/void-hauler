using Attributes;
using UnityEngine;
using Utility;
using VContainer;
using VContainer.Unity;

// this is a test class and won't be in the final game. testing out some structures and concepts
namespace test
{
    public class DamagePlayer : ITickable
    {
    
        // I want to take the channel and publish to it
        private readonly Health _playerHealth;
        private readonly CountdownTimer _damageCounter;

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
}