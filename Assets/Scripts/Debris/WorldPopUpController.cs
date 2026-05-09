using Inventory;
using UnityEngine;

namespace Debris
{
    public class WorldPopUpController : MonoBehaviour
    {
        public static WorldPopUpController INSTANCE;
        public GameObject popUpPrefab;
        public GameObject canvas;
        public float popUpDuration = 1f;
        public Camera mainCamera;

        private void Awake()
        {
            if (INSTANCE != null && INSTANCE != this)
            {
                Destroy(this.gameObject);
                return;
            }
            INSTANCE = this;
        }

        public void AddEvent(Item entryItem, Vector3 transformPosition)
        {
            var popup = Instantiate(popUpPrefab, canvas.transform);
            popup.GetComponent<WorldPopUp>().Setup(entryItem.itemType.name, Color.red, transformPosition, mainCamera);
            Destroy(popup, popUpDuration);

        }
        
    }
}