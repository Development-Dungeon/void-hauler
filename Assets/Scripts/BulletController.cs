using System;
using Debris;
using UnityEngine;
using Utility;

[RequireComponent(typeof(DamageOnTouch))]
public class BulletController : MonoBehaviour
{
    public Vector3 targetPos;
    public float speed;
    public float damage;
    public float maxTimeAlive = 30f;
    [Get] public DamageOnTouch damageOnTouch;
    private CountdownTimer _maxLifeTimer;


    public void Init(Vector3 transformPosition)
    {
        targetPos = transformPosition;
        _maxLifeTimer = new CountdownTimer(maxTimeAlive);
        _maxLifeTimer.OnTimerStop += () => Destroy(gameObject);
        _maxLifeTimer.Start();
        damageOnTouch.Init(damage, true);
    }

    private void Update()
    {
        var distance = Vector3.Distance(transform.position, targetPos);
        if(distance < .00001f) Destroy(gameObject);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // look at the target
            LookAt(targetPos);
        }
    }
    
    private void LookAt(Vector3 targetPosition)
    {
        // look at the player 
        Vector3 moveDelta = transform.position - targetPosition;

        // Ensure we actually moved to avoid NaN errors or flickering
        if (moveDelta.sqrMagnitude > 0.000001f)
        {
            float angle = Mathf.Atan2(moveDelta.y, moveDelta.x) * Mathf.Rad2Deg;

            // Adjust the -90 offset depending on your sprite's "Forward"
            // If it's still slightly off, try -85 or -95 to account for the Lerp lag, 
            // but -90 is the mathematical target for a "tip-up" triangle.
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}