using TMPro;
using UnityEngine;
using VContainer;

namespace Inventory
{
    public class WorldPopUp : MonoBehaviour
    {
        private Vector3 _worldPosition; // Where the item WAS
        [Inject] private Camera _mainCam;
        public Vector3 offset = new Vector3(0, 1.5f, 0); // Float slightly above the spot

        public void Setup(string text, Color color, Vector3 spawnPoint)
        {
            _worldPosition = spawnPoint + offset;
            
            TMP_Text tmp = GetComponent<TMP_Text>();
            tmp.text = text;
            tmp.color = color;
        }

        void LateUpdate()
        {
            // Every frame, translate that 3D point to where it should be on your monitor
            _worldPosition += Vector3.up * Time.deltaTime;
            var screenPos = _mainCam.WorldToScreenPoint(_worldPosition);
            

            transform.position = screenPos;
        }
    }
}