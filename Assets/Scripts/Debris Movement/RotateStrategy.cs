using UnityEngine;

namespace Debris_Movement
{
    public abstract class RotateStrategy : ScriptableObject
    {
        public abstract void Invoke(Transform transform);
    }
}