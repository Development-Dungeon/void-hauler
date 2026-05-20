using System.Collections.Generic;
using Debris;
using EventChannel.Audio_events;
using EventChannel.concrete;
using player;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Weapons;

namespace Enemy
{
    
    [RequireComponent(typeof(LineRenderer))]
    public class EnemyT2AIController : MonoBehaviour
    {
        [Header("Player")]
        public PlayerMovementController playerMovementController;
        
        [Header("Enemy Movement And Data")]
        [Get] public CirclePatrol circlePatrol;
        public BulletController bulletPrefab;
        public Weapon weapon;
        
        [Tooltip("degrees per second")]
        public float rotationSpeed = 90;
        
        [Header("Patrol")]
        [Tooltip("Pause before starting a patrol. \nUsed when transitioning from another state.\nIn seconds")]
        public float toPatrolCooldownTimer = 1f; // Seconds
        private CountdownTimer _toPatrolTimer;
        
        [Header("LockOn")]
        [Get] public LineRenderer lockOnLine;
        [Tooltip("The amount of time that the lockOn will occur.\nSeconds")]
        public float lockOnTimer = 5f;
        private CountdownTimer _lockOnTimer;
        
        [Header("Reload")]
        public float reloadTimer = 5f;
        private CountdownTimer _reloadTimer;
        
        [Header("Attack")]
        [Tooltip("Seconds")]
        public float attackRange = 8f;
        
        private List<CountdownTimer> _timers = new();
        
        [Header("Debug - Modify may be ignored")]
        [SerializeField] 
        private StateMachine stateMachine;
        private Vector3 _startingPosition;
        public bool drawGizmos = true;

        public EnemyStateEventChannel onLockOnChannel;
        public EnemyStateEventChannel onFireChannel;
 
        private void Awake()
        {
            _startingPosition = transform.position;
            
            // timers
            _toPatrolTimer = new CountdownTimer(toPatrolCooldownTimer);
            _lockOnTimer = new CountdownTimer(lockOnTimer);
            _reloadTimer = new CountdownTimer(reloadTimer);
            
            _timers.Add(_toPatrolTimer);
            _timers.Add(_lockOnTimer);
            _timers.Add(_reloadTimer);
            
            stateMachine = new StateMachine();
            
            var patrolStateNode = new StateNode(EnemyState.Patrol);
            var lockOnStateNode = new StateNode(EnemyState.LockOn);
            var reloadStateNode = new StateNode(EnemyState.Reload);
            var engageStateNode = new StateNode(EnemyState.Engage);
            var toPatrolStateNode = new StateNode(EnemyState.ToPatrol);

            patrolStateNode
                .OnEnter(() => circlePatrol.enableScript = true)
                .OnExit(() => circlePatrol.enableScript = false)
                .AddPerform(null)
                .AddTransition(lockOnStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                        return distance < attackRange;
                    })
                ;
            
            lockOnStateNode
                .OnEnter(() =>
                {
                    lockOnLine.enabled = true;
                    _lockOnTimer.Start();
                    onLockOnChannel.Invoke(new EnemyStateContext(transform.position, weapon));
                })
                .OnExit(() =>
                {
                    lockOnLine.enabled = false;
                    _lockOnTimer.Pause();
                })
                .AddPerform(PerformLockOn)
                .AddTransition(toPatrolStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, _startingPosition);
                    return distance > attackRange;
                })
                .AddTransition(lockOnStateNode, () => _lockOnTimer.IsRunning)
                .AddTransition(engageStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                    return distance < attackRange;
                    
                })
                ;

            engageStateNode
                .OnEnter(() =>
                {
                    onFireChannel.Invoke(new EnemyStateContext(transform.position, weapon));
                })
                .OnExit(null)
                .AddPerform(PerformEngage)
                .AddTransition(reloadStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                        return distance < attackRange;
                    })
                .AddTransition(toPatrolStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                        return distance > attackRange;
                    })
                ;

            reloadStateNode
                .OnEnter(() => _reloadTimer.Start())
                .OnExit(() => _reloadTimer.Pause())
                .AddPerform(null)
                .AddTransition(reloadStateNode, () => _reloadTimer.IsRunning)
                .AddTransition(lockOnStateNode, () =>
                {
                    var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                    return distance < attackRange;
                })
                .AddTransition(toPatrolStateNode, () =>
                    {
                        var distance = Vector3.Distance(playerMovementController.transform.position, transform.position);
                        return distance > attackRange;
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

        private void PerformLockOn()
        {
            LookAt(playerMovementController.transform.position);
            lockOnLine.SetPosition(0, transform.position);
            lockOnLine.SetPosition(1, playerMovementController.transform.position);
        }

        private void PerformEngage()
        {
            LookAt(playerMovementController.transform.position);
            Instantiate(bulletPrefab, transform.position, Quaternion.identity)
                .Init(playerMovementController.transform.position);
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
        private void OnValidate()
         {
             _startingPosition = transform.position;
         }
         
         private void OnDrawGizmos()
         {
             if (!drawGizmos) return;
             // DrawCircleAround(Color.white, _startingPosition, chaseRange);
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