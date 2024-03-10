using System;
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
            BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStartedHandler;
            BaseAction.OnAnyActionComplete += BaseAction_OnAnyActionCompleteHandler;
            HideActionCamera();
        }
        void OnDisable()
        {
            BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStartedHandler;
            BaseAction.OnAnyActionComplete -= BaseAction_OnAnyActionCompleteHandler;
        }

        private void BaseAction_OnAnyActionStartedHandler(object sender, EventArgs eventArgs)
        {
            switch (sender)
            {
                case ShootAction shootAction:
                    Unit shooterUnit = shootAction.GetUnit();
                    Unit targetUnit = shootAction.GetTargetUnit();

                    Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                    Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                    float shoulderOffsetAmount = 0.5f;
                    Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                    Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                    _actionCameraObject.transform.position = actionCameraPosition;
                    _actionCameraObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                    ShowActionCamera();
                    break;
                default:
                    break;
            }
        }
        private void BaseAction_OnAnyActionCompleteHandler(object sender, EventArgs eventArgs)
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
