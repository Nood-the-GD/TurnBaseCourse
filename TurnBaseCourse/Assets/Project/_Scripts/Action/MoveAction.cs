using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MoveAction : BaseAction
    {
        #region Variables
        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;

        private float DISTANCE_TO_STOP = 0.1f;
        private Vector3 _targetPosition;
        private float _moveSpeed;
        private float _rotateSpeed;
        private int _maxMoveDistance = 4;
        #endregion

        #region Unity functions
        protected override void Awake()
        {
            base.Awake();
            _targetPosition = this.transform.position;
        }
        private void Update()
        {
            if (!_isActive) return;

            float distance = Vector3.Distance(this.transform.position, _targetPosition);
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;

            if (distance > DISTANCE_TO_STOP)
            {
                MoveHandler(moveDirection);
            }
            else
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }

            
            RotateHandler(moveDirection);
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

        #region Override functions
        public override void TakeAction(GridPosition targetPos, Action onComplete)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPos);
            OnStartMoving?.Invoke(this, EventArgs.Empty);
            ActionStart(onComplete);
        }
        public override List<GridPosition> GetValidGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            GridPosition unitGridPosition = _unit.GetCurrentGridPosition();

            for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
            {
                for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
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
        public override string GetActionName()
        {
            return "Move";
        }
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            int targetCountAtGridPosition = _unit.GetAction<ShootAction>().GetTargetCountWhenStandAtGridPosition(gridPosition);

            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = targetCountAtGridPosition * 10
            };
        }
        #endregion

        #region Support
        private bool IsGridPositionValid(GridPosition testGridPosition)
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
        #endregion
    }
}