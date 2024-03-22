using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class MoveAction : BaseAction
    {
        #region Variables
        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;

        private float DISTANCE_TO_STOP = 0.1f;
        private float _moveSpeed;
        private float _rotateSpeed;
        private int _maxMoveDistance = 4;
        private List<Vector3> _targetPositionList;
        private int _currentPositionIndex;
        #endregion

        #region Unity functions
        private void Update()
        {
            if (!_isActive) return;

            Vector3 targetPosition = _targetPositionList[_currentPositionIndex];
            float distance = Vector3.Distance(this.transform.position, targetPosition);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            RotateHandler(moveDirection);
            if (distance > DISTANCE_TO_STOP)
            {
                MoveHandler(moveDirection);
            }
            else
            {
                _currentPositionIndex++;
                if(_currentPositionIndex >= _targetPositionList.Count)
                {
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                }
            }
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
            List<GridPosition> path = PathFinding.Instance.FindPath(_unit.GetCurrentGridPosition(), targetPos, out int pathLength);

            _currentPositionIndex = 0;

            _targetPositionList = path.Select(x => x.GetWorldPosition()).ToList();
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
            if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition)) return false;
            if (!PathFinding.Instance.HasPath(_unit.GetCurrentGridPosition(), testGridPosition)) return false;
            int pathFindingDistanceMultipler = 10;
            if(PathFinding.Instance.GetPathLength(_unit.GetCurrentGridPosition(), testGridPosition) > _maxMoveDistance * pathFindingDistanceMultipler) 
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}