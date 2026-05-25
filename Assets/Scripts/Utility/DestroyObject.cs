using UnityEngine;

namespace Utility
{
    public class DestroyObject : MonoBehaviour
    {
        public void ManualDestroy()
        {
            Destroy(gameObject);
        }
    }
}
