using System;
using Debris;
using player;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using VContainer;


public enum EnemyState
{
    Patrol,
    Sight,
    Engage
}

// This controller will control the enemy
// will be patrolling
// when the enemy runs into? a player, they will stop patrolling, and start attacking him by facing him and then shooting a bullet
[RequireComponent(typeof(CirclePatrol))]
public class EnemyShipController : MonoBehaviour
{
    // we have the patrol scripts. so that is good
    [Get] public CirclePatrol circlePatrol;
    [Inject] private PlayerMovementController _playerMovement;
    public float sightRange;
    public float sightSpeed = 5f;
    public float attackRange;
    public float attackCoolDownTimeInSeconds = 5f;
    public float stateTransitionTime = 0.5f;
    public GameObject bulletPrefab;

    [SerializeField]
    private EnemyState currentState;
    private CountdownTimer _attackTimer;
    private CountdownTimer _stateTransitionTimer;

    private void Awake()
    {
        currentState = EnemyState.Patrol;
        _attackTimer = new CountdownTimer(attackCoolDownTimeInSeconds);
        _stateTransitionTimer = new CountdownTimer(stateTransitionTime);
        _stateTransitionTimer.Start();
    }

    private void Update()
    {
        _attackTimer.Tick(Time.deltaTime);
        _stateTransitionTimer.Tick(Time.deltaTime);

        if (_stateTransitionTimer.IsRunning)
            return;
        
        var state = CheckState();

        if (state != currentState && _stateTransitionTimer.IsFinished)
        {
            _stateTransitionTimer.Reset(stateTransitionTime);
            UpdateCurrentState(state);
            Perform();
        }
        else if(state != currentState)
        {
            _stateTransitionTimer.Reset();
            _stateTransitionTimer.Start();
        } 
        else
        {
            UpdateCurrentState(state);
            Perform();
        }
    }

    private void UpdateCurrentState(EnemyState state)
    {
        if(_attackTimer.IsRunning && state != EnemyState.Engage)
            _attackTimer.Pause();

        
        currentState = state;
    }

    private void Perform()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PerformPatrol();
                break;
            case EnemyState.Sight:
                PerformSight();
                break;
            case EnemyState.Engage:
                PerformEngage();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void PerformEngage()
    {
        if (_attackTimer.IsFinished)
        {
            LookAt(_playerMovement.transform.position);
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            var bulletController = bullet.GetComponent<BulletController>();
            bulletController.Init(_playerMovement.transform.position);
            _attackTimer.Reset(attackCoolDownTimeInSeconds);
            _attackTimer.Start();
        }
        else if (!_attackTimer.IsRunning)
        {
            _attackTimer.Reset(attackCoolDownTimeInSeconds);
            _attackTimer.Start();
        }
        
    }

    private void PerformSight()
    {
        circlePatrol.enableScript = false;

        var distance = Vector2.Distance(transform.position, _playerMovement.transform.position);
        if(distance < sightRange)
            LookAt(_playerMovement.transform.position);
    }

    private void LookAt(Vector3 targetPosition)
    {
        // look at the player 
        Vector3 moveDelta = targetPosition - transform.position; 

        // Ensure we actually moved to avoid NaN errors or flickering
        if (moveDelta.sqrMagnitude > 0.000001f)
        {
            float angle = Mathf.Atan2(moveDelta.y, moveDelta.x) * Mathf.Rad2Deg;

            // Adjust the -90 offset depending on your sprite's "Forward"
            // If it's still slightly off, try -85 or -95 to account for the Lerp lag, 
            // but -90 is the mathematical target for a "tip-up" triangle.
            var targetLocation = Quaternion.Euler(0, 0, angle - 90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetLocation, Time.deltaTime * sightSpeed);
        }
    }
    
    private void PerformPatrol()
    {
        circlePatrol.enableScript = true;
    }

    private EnemyState CheckState()
    {
        float distance;
        switch (currentState)
        {
            case EnemyState.Patrol:
                distance = Vector3.Distance(_playerMovement.transform.position, transform.position);
                if (distance <= sightRange) return EnemyState.Sight;
                return EnemyState.Patrol;
            case EnemyState.Sight:
                distance = Vector3.Distance(_playerMovement.transform.position, transform.position);
                if (distance <= attackRange) return EnemyState.Engage;
                if (distance <= sightRange) return EnemyState.Sight;
                return EnemyState.Patrol;
            case EnemyState.Engage:
                distance = Vector3.Distance(_playerMovement.transform.position, transform.position);
                if (distance <= attackRange) return EnemyState.Engage;
                return EnemyState.Patrol;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}