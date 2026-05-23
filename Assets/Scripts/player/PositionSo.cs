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
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void ClearOnLoad() {
            
            var positions = Resources.FindObjectsOfTypeAll<PositionSo>();
            if (positions == null) return;
            foreach (var positionSo in positions)
            {
                positionSo.ClearStackManual();
            }
            
        }
        
#if UNITY_EDITOR
        [ContextMenu("Clear Stack")]
        public void ClearStackManual()
        {
            Debug.Log("clearing player position stack");
            _positions.Clear();
        }
#endif
    }
}