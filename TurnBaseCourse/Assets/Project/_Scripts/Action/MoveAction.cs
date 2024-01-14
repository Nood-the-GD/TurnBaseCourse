using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private Animator _unitAnimator;
        private float DISTANCE_TO_STOP = 0.1f;
        private Vector3 _targetPosition;
        private float _moveSpeed;
        private float _rotateSpeed;
        private Unit _unit;

        #region Unity functions
        void Awake()
        {
            _targetPosition = this.transform.position;
            _unit = GetComponent<Unit>();
        }
        private void Update()
        {
            float distance = Vector3.Distance(this.transform.position, _targetPosition);

            if (distance > 0.1f)
            {
                _unitAnimator.SetBool("Walk", true);
                Vector3 moveDirection = (_targetPosition - transform.position).normalized;
                MoveHandler(moveDirection);
                RotateHandler(moveDirection);
            }
            else
                _unitAnimator.SetBool("Walk", false);
        }
        #endregion

        #region Movement Handler
        public void SetMoveProperty(float moveSpeed, float rotateSpeed)
        {
            _moveSpeed = moveSpeed;
            _rotateSpeed = rotateSpeed;
        }
        private void MoveHandler(Vector3 moveDirection)
        {
            this.transform.position += _moveSpeed * Time.deltaTime * moveDirection;
        }
        private void RotateHandler(Vector3 moveDirection)
        {
            this.transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
        }
        #endregion

        public void SetTargetPosition(GridPosition targetPos)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPos);
        }

        public bool IsValidActionGridPosition(GridPosition gridPositionStruct)
        {
            List<GridPosition> validGridPositionList = GetValidGridPositionList();
            return validGridPositionList.Contains(gridPositionStruct);
        }

        public List<GridPosition> GetValidGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = _unit.GetCurrentGridPosition();

            int maxMoveDistance = 1;
            for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
            {
                for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
                {
                    GridPosition offSetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                    if (IsGridPositionValid(testGridPosition))
                    {
                        validGridPositionList.Add(testGridPosition);
                    }
                }
            }
            return validGridPositionList;
        }

        public bool IsGridPositionValid(GridPosition testGridPosition)
        {
            GridPosition unitGridPosition = _unit.GetCurrentGridPosition();

            if (testGridPosition == unitGridPosition) return false;
            if (!LevelGrid.Instance.IsValidGrid(testGridPosition)) return false;
            if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
            {
                // Already has unit in this position
                return false;
            }
            return true;
        }
    }
}