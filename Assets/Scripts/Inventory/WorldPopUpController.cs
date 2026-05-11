using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Inventory
{
    public class WorldPopUpController : MonoBehaviour
    {
        public static WorldPopUpController Instance;
        public GameObject popUpPrefab;
        public GameObject canvas;
        public float popUpDuration = 1f;
        private IObjectResolver _resolver;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            _resolver = objectResolver;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        public void AddEvent(Item entryItem, Vector3 transformPosition)
        {
            var popup = _resolver.Instantiate(popUpPrefab, canvas.transform);
            popup.GetComponent<WorldPopUp>().Setup(entryItem.itemType.name, Color.white, transformPosition);
            Destroy(popup, popUpDuration);

        }
        
    }
}