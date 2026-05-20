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
        public Inventory playerInventory;
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

        private void OnItemAdded(InventoryEventContext obj)
        {
            if (obj == null || obj.item == null || obj.position == null) return;
            
            var spawnPosition = obj.position ?? Vector3.zero;
            
            var itemColor = TierToColors.FirstOrDefault(ttc =>
            {
                var pickedUpItem = obj.item.GetType();
                var tierToFilter = ttc.ItemType.GetType();
                return pickedUpItem == tierToFilter || tierToFilter.IsAssignableFrom(pickedUpItem);
            })?.Color ?? Color.red;
                
            var popup = _resolver.Instantiate(popUpPrefab, canvas.transform);
            popup.Setup(obj.item.name, itemColor, spawnPosition);
            
            Destroy(popup.gameObject, popUpDuration);
            
        }

        private void Start()
        {
            playerInventory.OnItemAdded += OnItemAdded;
        }
        private void OnDestroy()
        {
            playerInventory.OnItemAdded -= OnItemAdded;
        }
    }
}