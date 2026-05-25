using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class OnDamageController : MonoBehaviour
    {

        public List<SpriteRenderer> imagesToFlicker = new();
    
        public float flickerDuration = 2f; // seconds
        private CountdownTimer _flickerTotalDurationTimer;

        public float flickOnDuration = .1f;
        private CountdownTimer _flickOnTimer;

        public float flickOffDuration = .5f;
        private CountdownTimer _flickOffTimer;
    
        private readonly List<CountdownTimer> _timers = new();

        private float? _localHealth = null;

        private void Awake()
        {
            _flickerTotalDurationTimer = new CountdownTimer(flickerDuration);
            _flickOnTimer = new CountdownTimer(flickOnDuration);
            _flickOffTimer = new CountdownTimer(flickOffDuration);
        
            _timers.Add(_flickerTotalDurationTimer);
            _timers.Add(_flickOnTimer);
            _timers.Add(_flickOffTimer);

            _flickerTotalDurationTimer.OnTimerStart += StartFlickOffTimer;
            _flickerTotalDurationTimer.OnTimerStop += StopTimers;

            _flickOffTimer.OnTimerStart += () => Flick(false);
            _flickOffTimer.OnTimerStop += () => { _flickOnTimer.Start();};
        
            _flickOnTimer.OnTimerStart += () => Flick(true);
            _flickOnTimer.OnTimerStop += () => { _flickOffTimer.Start();};

        }

        private void StopTimers()
        {
            Flick(true);
            _timers.ForEach(t => t.Pause());
        }

        private void StartFlickOffTimer()
        {
            if (_flickOffTimer.IsRunning)
                return;
        
            _flickOffTimer.Reset(flickOffDuration);
            _flickOffTimer.Start();
        }

        private void Flick(bool isOn)
        {
            imagesToFlicker.ForEach(image => image.enabled = isOn);
        }

        private void Update()
        {
            _timers.ForEach(timer => timer.Tick(Time.deltaTime));
        }


        public void OnHealthChanged(float currentHealthPercentage)
        {
            if (_localHealth == null)
            {
                _localHealth = currentHealthPercentage;
            }
            else if (_localHealth != null)
            {
                _localHealth = currentHealthPercentage;
                _flickerTotalDurationTimer.Reset(flickerDuration);
                _flickerTotalDurationTimer.Start();
            }
        
        }

    }
}
