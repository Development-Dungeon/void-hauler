using UnityEngine;

namespace Attributes
{
    [CreateAssetMenu(fileName = "Movement", menuName = "Attributes/Movement")]
    public class MovementData : ScriptableObject
    {
        // public float speed;
        public float turnSmoothing;
    }
}