using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [Serializable]
    public class StateNode
    {
        [SerializeField]
        public EnemyState stateEnum;
        
        private Func<bool> _transitionFunc;
        private Dictionary<StateNode, Func<bool>> _stateTransitions = new ();
        private Action _eventInvoker;
        
        private Action _initFunc;
        private Action _exitInvoker;

        public StateNode(EnemyState patrol)
        {
            stateEnum = patrol;
        }
        
        public StateNode AddTransition(StateNode sightStateNode, Func<bool> func)
        {
            _stateTransitions.Add(sightStateNode, func);
            return this;
        }

        public StateNode VerifyState()
        {
            foreach (var (state, boolCheck) in _stateTransitions)
                if (boolCheck())
                    return state;

            return this;
        }

        public void Invoke()
        {
            _eventInvoker?.Invoke();
        }

        public void ExitState()
        {
            _exitInvoker?.Invoke();
        }

        public StateNode AddInit(Action func)
        {
            _initFunc = func;
            return this;
        }

        public void EnterState()
        {
            _initFunc?.Invoke();
        }

        public StateNode AddExit(Action func)
        {
            _exitInvoker = func;
            return this;
        }

        public StateNode AddPerform(Action performPatrol)
        {
            _eventInvoker = performPatrol;
            return this;
        }
    }
}