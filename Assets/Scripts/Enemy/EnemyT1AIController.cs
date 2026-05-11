using System.Collections.Generic;
using Debris;
using player;
using UnityEngine;
using Utility;

namespace Enemy
{
    public class EnemyT1AIController : MonoBehaviour
    {
        [Header("Player")]
        public PlayerMovementController playerMovementController;
        
        [Header("Enemy Movement And Data")]
        [Get] public CirclePatrol circlePatrol;
        public BulletController bulletPrefab; 
        
        [Header("Patrol")]
        public float toPatrolCooldownTimer = 1f; // Seconds
        private CountdownTimer _toPatrolTimer;
        
        [Header("Sight")]
        public float sightRange = 10f;
        
        [Header("Attack")]
        public float attackCooldownTimer = 1f; // Seconds
        public float attackRange = 8f;
        private CountdownTimer _attackTimer;
        
        private List<CountdownTimer> _timers = new();
        
        [Header("Debug - Do Not Modify")]
        [SerializeField] 
        private StateMachine stateMachine;
        

        private void Awake()
        {
            // timers
            _attackTimer = new CountdownTimer(attackCooldownTimer);
            _toPatrolTimer = new CountdownTimer(toPatrolCooldownTimer);
            
            _timers.Add(_attackTimer);
            _timers.Add(_toPatrolTimer);
            
            stateMachine = new StateMachine();
            
            var patrolStateNode = new StateNode(EnemyState.Patrol);
            var sightStateNode = new StateNode(EnemyState.Sight);
            var engageStateNode = new StateNode(EnemyState.Engage);
            var toPatrolStateNode = new StateNode(EnemyState.ToPatrol);

            patrolStateNode
                .AddInit(() => circlePatrol.enableScript = true)
                .AddExit(() => circlePatrol.enableScript = false)
                .AddPerform(null)
                .AddTransition(sightStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                        return distance <= sightRange;
                    });

            sightStateNode
                .AddPerform(PerformSight)
                .AddTransition(toPatrolStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                    return distance > sightRange;
                })
                .AddTransition(engageStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                    return distance <= attackRange;
                });

            engageStateNode
                .AddPerform(PerformEngage)
                .AddInit(() => _attackTimer.Start())
                .AddExit(() => _attackTimer.Pause())
                .AddTransition(sightStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                    return distance <= sightRange && distance > attackRange;
                })
                .AddTransition(toPatrolStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                        return distance > attackRange;
                    });

            toPatrolStateNode
                .AddInit(() => _toPatrolTimer.Start())
                .AddExit(() => _toPatrolTimer.Pause())
                .AddPerform(null)
                .AddTransition(patrolStateNode, () => _toPatrolTimer.IsFinished);
            
            stateMachine.StartNode(patrolStateNode);
            stateMachine.InitState();
        }
        
        private void PerformSight()
        {
            circlePatrol.enableScript = false;

            var distance = Vector2.Distance(transform.position, playerMovementController.transform.position);
            if(distance < sightRange)
                LookAt(playerMovementController.transform.position);
        }

        private void PerformEngage()
        {
            if (_attackTimer.IsFinished)
            {
                LookAt(playerMovementController.transform.position);
                Instantiate(bulletPrefab, transform.position, Quaternion.identity)
                    .Init(playerMovementController.transform.position);
                _attackTimer.Reset(attackCooldownTimer);
                _attackTimer.Start();
            }
            else if (!_attackTimer.IsRunning)
            {
                _attackTimer.Reset(attackCooldownTimer);
                _attackTimer.Start();
            }
        
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetLocation, Time.deltaTime);
            }
        }

        private void Update()
        {
            _timers.ForEach(t => t.Tick(Time.deltaTime));

            var nextState = stateMachine.GetNextState();
            
            if (stateMachine.IsStateChange(nextState))
            {
                stateMachine.ExitCurrentState();
                stateMachine.UpdateState(nextState);
                stateMachine.InitState();
            }
            
            stateMachine.PerformState();
        }
    }
}