using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject _actionCameraObject;

        private void Start()
        {
            HideActionCamera();
        }

        private void ShowActionCamera()
        {
            _actionCameraObject.SetActive(true);
        }
        private void HideActionCamera()
        {
            _actionCameraObject.SetActive(false);
        }
    }
}
