using UnityEngine;

namespace player
{
    [CreateAssetMenu(fileName = "PlayerLoadPosition", menuName = "Player/Load Position")]
    public class PositionSo : ScriptableObject
    {
        public Vector3? Position = null; 
    }
}