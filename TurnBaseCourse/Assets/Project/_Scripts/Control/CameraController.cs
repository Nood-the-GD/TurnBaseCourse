using UnityEngine;
using Cinemachine;
using NOOD.Extension;
using NOOD;

namespace Game
{
    public class CameraController : MonoBehaviorInstance<CameraController>
    {
        #region Variables
        private const float MIN_FOLLOW_Y_OFFSET = 2f;
        private const float MAX_FOLLOW_X_OFFSET = 15f;

        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _zoomSpeed = 10f;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private CinemachineTransposer _cinemachineTransposer;
        private Vector3 _followOffset;
        #endregion

        #region Unity functions
        void Start()
        {
            _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _followOffset = _cinemachineTransposer.m_FollowOffset;
        }

        private void Update()
        {
            MoveHandler();
            RotateHandler();
            ZoomHandler();
        }
        #endregion

        #region Main functions
        private void MoveHandler()
        {
            Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

            Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
            transform.position += _moveSpeed * Time.deltaTime * moveVector;
        }
        private void RotateHandler()
        {
            Vector3 rotationVector = Vector3.zero;
            rotationVector.y = InputManager.Instance.GetCameraRotationAmount();

            transform.eulerAngles += rotationVector * _rotationSpeed * Time.deltaTime;
        }
        private void ZoomHandler()
        {
            _followOffset.y += InputManager.Instance.GetCameraZoomAmount();

            _followOffset.y = Mathf.Clamp(_followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_X_OFFSET);
            
            _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _followOffset, Time.deltaTime * _zoomSpeed);
        }
        #endregion

        #region Support
        public float GetCameraHeight()
        {
            return _followOffset.y;
        }
        #endregion
    }
}