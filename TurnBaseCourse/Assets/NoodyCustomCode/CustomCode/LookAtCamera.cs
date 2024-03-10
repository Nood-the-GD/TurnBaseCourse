using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool _invert;
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if(_invert)
            {
                Vector3 dirToCam = (_cameraTransform.position - transform.position).normalized;
                transform.LookAt(transform.position + dirToCam * -1);
            }
            else
            {
                this.transform.LookAt(_cameraTransform);
            }
        }
    }
}
