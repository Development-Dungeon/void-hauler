using UnityEngine;

namespace Enemy
{
    public class EnemyStateContext
    {
        public Vector3 position;
        public Weapon weapon;

        public EnemyStateContext(Vector3 position, Weapon weapon)
        {
            this.position = position;
            this.weapon = weapon;
        }
    }
}