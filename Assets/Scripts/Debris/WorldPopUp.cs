using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace Debris
{
    public class WorldPopUp : MonoBehaviour
    {
        private Vector3 worldPosition; // Where the item WAS
        private Camera mainCam;
        public Vector3 offset = new Vector3(0, 1.5f, 0); // Float slightly above the spot

        public void Setup(string text, Color color, Vector3 spawnPoint, Camera mainCamera)
        {
            mainCam = mainCamera;
            worldPosition = spawnPoint + offset;

            TMP_Text tmp = GetComponent<TMP_Text>();
            tmp.text = text;
            tmp.color = color;
        }

        void LateUpdate()
        {
            // Every frame, translate that 3D point to where it should be on your monitor
            worldPosition += Vector3.up * Time.deltaTime;
            Vector3 screenPos = mainCam.WorldToScreenPoint(worldPosition);

            // Optimization: Hide text if it's behind the camera
            if (screenPos.z < 0)
            {
                GetComponent<CanvasGroup>().alpha = 0;
                return;
            }

            transform.position = screenPos;
        }
    }
}