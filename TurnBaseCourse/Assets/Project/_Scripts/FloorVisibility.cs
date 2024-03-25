using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class FloorVisibility : MonoBehaviour
    {
        private Renderer[] _renderArray;
        private int _floor;

        private void Awake()
        {
            _renderArray = GetComponentsInChildren<Renderer>(true);
        }

        private void Start()
        {
        }

        private void Update()
        {
            _floor = LevelGrid.Instance.GetFloor(transform.position);
            float cameraHeight = CameraController.Instance.GetCameraHeight();
            float floorOffset = 2f;
            bool showObject = cameraHeight > LevelGrid.FLOOR_HEIGHT * _floor + floorOffset;

            if(showObject || _floor == 0)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            foreach(Renderer render in _renderArray)
            {
                render.enabled = true;
            }
        }
        private void Hide()
        {
            foreach(Renderer render in _renderArray)
            {
                render.enabled = false;
            }
        }
    }
}
