using System;
using DefaultNamespace;
using EventChannel;
using UnityEngine;

public class Health : MonoBehaviour
{
    // attributes
    public float maxHealth = 100f;
    private float _currentHealth;
    
    public EventChannel<float> eventChannel;

    // events
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnAlive;

    private CountdownTimer damageTimer;

    public float timer = 4;

    private void Awake()
    {
        _currentHealth = maxHealth;
        damageTimer = new CountdownTimer(timer);
        damageTimer.OnTimerStop += TakeTimerDamage;
    }

    private void TakeTimerDamage()
    {
        TakeDamage(1.1f);
        
        damageTimer.Reset(timer);
        damageTimer.Start();
    }

    private void Start()
    {
        damageTimer.Start();
    }

    public bool IsDead => _currentHealth <= 0;
    
    // take damage
    public void TakeDamage(float damage)
    {
        if (IsDead)
            return;
        
        _currentHealth = Math.Max(_currentHealth - damage, 0f); 
        
        eventChannel?.Invoke(_currentHealth / maxHealth);
    }

    private void Update()
    {
        if(damageTimer.IsRunning)
            damageTimer.Tick(Time.deltaTime);
        
    }
}
