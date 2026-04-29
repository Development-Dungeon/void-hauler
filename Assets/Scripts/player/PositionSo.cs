using System;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "PlayerLoadPosition", menuName = "Player/Load position")]
    public class PositionSo : ScriptableObject
    {
        public Vector3 defaultPosition;
        private readonly Stack<Vector3> _positions= new ();

        private void OnEnable()
        {
            ClearStackManual();
        }

        public Vector3 PopPositionOrDefault()
        {
            if (_positions.Count == 0)
                return defaultPosition;
            
            return _positions.Pop();

        }

        public void PushPosition(Vector3 position)
        {
            _positions.Push(position);
            Debug.Log("the stack count is" + _positions.Count);
        }
        
#if UNITY_EDITOR
        [ContextMenu("Clear Stack")]
        private void ClearStackManual() => _positions.Clear();
#endif
    }
}