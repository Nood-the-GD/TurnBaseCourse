#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class InputManager : MonoBehaviorInstance<InputManager>
    {
        private GameInput _gameInput;

        protected override void ChildAwake()
        {
            _gameInput = new GameInput();
            _gameInput.Player.Enable();
        }


        public Vector2 GetMouseScreenPosition()
        {
#if USE_NEW_INPUT_SYSTEM
            return Mouse.current.position.ReadValue();
#else            
            return Input.mousePosition;
#endif
        }

        public bool IsLeftMouseButtonDown()
        {
#if USE_NEW_INPUT_SYSTEM
            return _gameInput.Player.PressMouse0.WasPressedThisFrame();
#else
            return Input.GetMouseButtonDown(0);
#endif
        }

        public Vector2 GetCameraMoveVector()
        {
#if USE_NEW_INPUT_SYSTEM
            return _gameInput.Player.CameraMovement.ReadValue<Vector2>();
#else
            Vector2 inputMoveDir = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDir.y = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputMoveDir.y = -1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDir.x = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputMoveDir.x = 1;
            }

            return inputMoveDir;
#endif
        }

        public float GetCameraRotationAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return _gameInput.Player.CameraRotate.ReadValue<float>();
#else
            float rotateAmount = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                rotateAmount = 1;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotateAmount = -1;
            }
            return rotateAmount;
#endif
        }
        public float GetCameraZoomAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return _gameInput.Player.CameraZoom.ReadValue<float>();
#else
            float zoomAmount = 0f;
            Vector2 mouseScroll = Input.mouseScrollDelta;
            if (mouseScroll.y > 0)
            {
                zoomAmount = -1f;
            }
            if (mouseScroll.y < 0)
            {
                zoomAmount = 1;
            }
            return zoomAmount;
#endif
        }
    }
}
