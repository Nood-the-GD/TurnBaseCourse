using UnityEngine;
using Cinemachine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        private const float MIN_FOLLOW_Y_OFFSET = 2f;
        private const float MAX_FOLLOW_X_OFFSET = 12f;

        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _zoomSpeed = 10f;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private CinemachineTransposer _cinemachineTransposer;
        private Vector3 _followOffset;

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

        private void MoveHandler()
        {
            Vector3 inputMoveDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                inputMoveDir.z = 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputMoveDir.z = -1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputMoveDir.x = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputMoveDir.x = 1;
            }

            Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
            transform.position += _moveSpeed * Time.deltaTime * moveVector;
        }
        private void RotateHandler()
        {
            Vector3 rotationVector = Vector3.zero;
            if (Input.GetKey(KeyCode.Q))
            {
                rotationVector.y = 1;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotationVector.y = -1;
            }

            transform.eulerAngles += rotationVector * _rotationSpeed * Time.deltaTime;
        }

        private void ZoomHandler()
        {
            Vector2 mouseScroll = Input.mouseScrollDelta;
            float zoomAmount = 1;
            if (mouseScroll.y > 0)
            {
                _followOffset.y -= zoomAmount;
            }
            if (mouseScroll.y < 0)
            {
                _followOffset.y += zoomAmount;
            }
            _followOffset.y = Mathf.Clamp(_followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_X_OFFSET);
            _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _followOffset, Time.deltaTime * _zoomSpeed);
        }
    }
}