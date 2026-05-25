using System;
using System.Collections.Generic;
using Debris;
using EventChannel.Audio_events;
using player;
using UnityEngine;
using Utility;

namespace Enemy
{

    public class EnemyStateChangeContext
    {
        public Vector3 Position;

        public EnemyStateChangeContext(Vector3 position)
        {
            Position = position;
        }
    }
    public class EnemyT1AIController : MonoBehaviour
    {
        [Header("Player")]
        public PlayerMovementController playerMovementController;
        
        [Header("Enemy Movement And Data")]
        [Get] public CirclePatrol circlePatrol;
        public BulletController bulletPrefab; 
        [Tooltip("degrees per second")]
        public float rotationSpeed = 90;
        [Header("Patrol")]
        [Tooltip("Pause before starting a patrol. \nUsed when transitioning from another state.\nIn seconds")]
        public float toPatrolCooldownTimer = 1f; // Seconds
        private CountdownTimer _toPatrolTimer;

        [Header("Chase")] 
        public float chaseRange = 10f;
        public float chaseMoveSpeed = 5f;
        
        [Header("Attack")]
        [Tooltip("Seconds")]
        public float attackCooldownTimer = 1f; 
        public float attackRange = 8f;
        private CountdownTimer _attackTimer;
        
        private List<CountdownTimer> _timers = new();
        
        [Header("Debug - Modify may be ignored")]
        [SerializeField] 
        private StateMachine stateMachine;
        private Vector3 _startingPosition;
        public bool drawGizmos = true;
        [Get] public Collider2D selfCollider;

        // Events
        public static event Action<EnemyStateChangeContext> OnEngageState;

        private void OnValidate()
        {
            _startingPosition = transform.position;
        }
        

        private void Awake()
        {
            _startingPosition = transform.position;
            
            // timers
            _attackTimer = new CountdownTimer(attackCooldownTimer);
            _toPatrolTimer = new CountdownTimer(toPatrolCooldownTimer);
            
            _timers.Add(_attackTimer);
            _timers.Add(_toPatrolTimer);
            
            stateMachine = new StateMachine();
            
            var patrolStateNode = new StateNode(EnemyState.Patrol);
            var chaseStateNode = new StateNode(EnemyState.Chase);
            var lockOnStateNode = new StateNode(EnemyState.LockOn);
            var engageStateNode = new StateNode(EnemyState.Engage);
            var toPatrolStateNode = new StateNode(EnemyState.ToPatrol);

            patrolStateNode
                .OnEnter(() => circlePatrol.enableScript = true)
                .OnExit(() => circlePatrol.enableScript = false)
                .AddPerform(null)
                .AddTransition(chaseStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                        return distance < chaseRange;
                    })
                ;

            chaseStateNode
                .OnEnter(null)
                .OnExit(null)
                .AddPerform(PerformChase)
                .AddTransition(lockOnStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                    return distance < attackRange;
                    
                })
                .AddTransition(toPatrolStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                    return distance > chaseRange;
                    
                })
                ;

            lockOnStateNode
                .OnEnter(() => _attackTimer.Start())
                .OnExit(() => _attackTimer.Pause())
                .AddTransition(engageStateNode, () =>
                {
                    if (_attackTimer.IsRunning) return false;
                    
                    var distance = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                    return distance < attackRange;
                })
                .AddTransition(lockOnStateNode, () =>
                {
                    if (_attackTimer.IsFinished) return false;
                    
                    var distance = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                    return distance < attackRange;
                })
                .AddTransition(chaseStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                    return distance > attackRange && distance < chaseRange;

                });

            engageStateNode
                .OnEnter(() => OnEngageState?.Invoke(new EnemyStateChangeContext(transform.position)))
                .OnExit(null)
                .AddPerform(PerformEngage)
                .AddTransition(lockOnStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                        return distance < attackRange;
                    })
                .AddTransition(chaseStateNode, () =>
                    {
                        var distanceFromCenter = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                        return distanceFromCenter < chaseRange;
                    })
                .AddTransition(toPatrolStateNode, () =>
                    {
                        var distanceFromCenter = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                        return distanceFromCenter > chaseRange;
                    })
                ;

            toPatrolStateNode
                .OnEnter(() => _toPatrolTimer.Start())
                .OnExit(() => _toPatrolTimer.Pause())
                .AddPerform(null)
                .AddTransition(patrolStateNode, () => _toPatrolTimer.IsFinished);
            
            stateMachine.StartNode(patrolStateNode);
            stateMachine.InitState();
        }

        private void PerformChase()
        {
            LookAt(playerMovementController.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, playerMovementController.transform.position, Time.deltaTime * chaseMoveSpeed);
            
        }

        private void PerformEngage()
        {
            LookAt(playerMovementController.transform.position);
            
            Instantiate(bulletPrefab, transform.position, Quaternion.identity)
                .Init(playerMovementController.transform.position, selfCollider );
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetLocation, rotationSpeed * Time.deltaTime);
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
        
        private void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            DrawCircleAround(Color.white, _startingPosition, chaseRange);
            DrawCircleAround(Color.red, transform.position, attackRange);
        }

        private void DrawCircleAround(Color colorToDraw, Vector3 attachPoint, float range)
        {
            // 1. Set the color
            Gizmos.color = colorToDraw;

            // 2. Set the Gizmo matrix to match the object's position/rotation, 
            // but flatten the Z-axis to lock it to 2D
            Gizmos.matrix = Matrix4x4.TRS(attachPoint, Quaternion.identity, new Vector3(1, 1, 0));

            // 3. Draw a wire sphere at the center. It will render as a flat 2D circle.
            Gizmos.DrawWireSphere(Vector3.zero, range);

            // 4. Reset the matrix so it doesn't distort other gizmos
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}