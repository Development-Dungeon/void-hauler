using UnityEngine;

namespace Debris
{
    public abstract class RotateStrategy : ScriptableObject
    {
        public abstract void Invoke(Transform transform);
    }
}