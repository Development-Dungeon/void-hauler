using System;
using UnityEngine;

namespace Enemy
{
    [Serializable]
    public class StateMachine
    {
        
        [SerializeField]
        private StateNode currentState;
        
        public void StartNode(StateNode patrolStateNode)
        {
            currentState = patrolStateNode;
        }

        public void UpdateState(StateNode nextState)
        {
            currentState = nextState;
        }

        public void PerformState()
        {
            currentState.Invoke();
        }

        public StateNode GetNextState()
        {
            return currentState.VerifyState();
        }

        public bool IsStateChange(StateNode nextState)
        {
            return currentState != nextState;
        }

        public void ExitCurrentState()
        {
            currentState.ExitState();
        }

        public void InitState()
        {
            currentState.EnterState();
        }
    }
}