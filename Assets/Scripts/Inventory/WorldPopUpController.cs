using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Inventory
{
    [Serializable]
    public class TierToColor
    {
        public ItemType ItemType;
        public Color Color;
    }
    public class WorldPopUpController : MonoBehaviour
    {
        public static WorldPopUpController Instance;
        public WorldPopUp popUpPrefab;
        public GameObject canvas;
        public float popUpDuration = 1f;
        private IObjectResolver _resolver;

        [SerializeField] public List<TierToColor> TierToColors = new();

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
            var itemColor = TierToColors.FirstOrDefault(ttc =>
            {
                var pickedUpItem = entryItem.itemType.GetType();
                var tierToFilter = ttc.ItemType.GetType();
                return pickedUpItem == tierToFilter || tierToFilter.IsAssignableFrom(pickedUpItem);
            })?.Color ?? Color.red;
                
            var popup = _resolver.Instantiate(popUpPrefab, canvas.transform);
            popup.Setup(entryItem.itemType.name, itemColor, transformPosition);
            Destroy(popup.gameObject, popUpDuration);

        }
        
    }
}