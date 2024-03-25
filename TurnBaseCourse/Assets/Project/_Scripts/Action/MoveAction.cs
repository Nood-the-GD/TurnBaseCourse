using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NOOD;
using UnityEngine;

namespace Game
{
    public class MoveAction : BaseAction
    {
        #region Variables
        private const float DISTANCE_TO_STOP = 0.1f;
        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;
        public event EventHandler<OnChangeFloorEventArgs> OnChangeFloorStarted;
        public class OnChangeFloorEventArgs : EventArgs
        {
            public GridPosition unitGridPosition;
            public GridPosition targetGridPosition;
        }

        private float _moveSpeed;
        private float _rotateSpeed;
        private int _maxMoveDistance = 4;
        private List<Vector3> _targetPositionList;
        private int _currentPositionIndex;
        private bool _isChangingFloor;
        private float _differentFloorTimer;
        private float _differentFloorTimerMax = .87f;
        #endregion

        #region Unity functions
        private void OnEnable()
        {
            if(TryGetComponent<UnitAnimator>(out UnitAnimator unitAnimator))
            {
                unitAnimator.OnJumpComplete += UnitAnimator_OnJumpComplete;
            }
        }
        private void Update()
        {
            if (!_isActive) return;

            Vector3 targetPosition = _targetPositionList[_currentPositionIndex];
            if(_isChangingFloor)
            {
                // Stop and teleport
                Vector3 targetSameFloorPosition = targetPosition;
                targetSameFloorPosition.y = this.transform.position.y;

                Vector3 rotateDir = (targetSameFloorPosition - this.transform.position).normalized;

                RotateHandler(rotateDir);
                // Move unit to target position in UnitAnimator_OnJumpComplete
            }
            else
            {
                // Regular movement
                Vector3 moveDirection = (targetPosition - this.transform.position).normalized;
                RotateHandler(moveDirection);
                MoveHandler(moveDirection);
            }

            if (Vector3.Distance(this.transform.position, targetPosition) < DISTANCE_TO_STOP)
            {
                // Stop moving and check if target is different floor
                _currentPositionIndex++;
                if(_currentPositionIndex >= _targetPositionList.Count)
                {
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                }
                else
                {
                    Vector3 nextTargetPosition = _targetPositionList[_currentPositionIndex];
                    GridPosition nextTargetGridPosition = LevelGrid.Instance.GetGridPosition(nextTargetPosition);
                    GridPosition unitGridPosition = _unit.GetCurrentGridPosition();

                    if (nextTargetGridPosition.floor != unitGridPosition.floor && _isChangingFloor == false) 
                    {
                        // Different floors
                        _isChangingFloor = true;
                        _differentFloorTimer = _differentFloorTimerMax;
                        OnChangeFloorStarted?.Invoke(this, new OnChangeFloorEventArgs
                        {
                            unitGridPosition = unitGridPosition,
                            targetGridPosition = nextTargetGridPosition
                        });
                    }
                }
            }
        }
        #endregion

        #region Events functions
        private void UnitAnimator_OnJumpComplete(object sender, UnitAnimator.OnJumpCompleteEventArgs e)
        {
            this.transform.position = e.targetGridPosition.GetWorldPosition();
            _isChangingFloor = false;
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
            this.transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
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
                    for(int floor = -_maxMoveDistance; floor <= _maxMoveDistance; floor++)
                    {
                        GridPosition offSetGridPosition = new GridPosition(x, z, floor);
                        GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                        if (IsGridPositionValid(testGridPosition))
                        {
                            validGridPositionList.Add(testGridPosition);
                        }
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
        protected override bool IsGridPositionValid(GridPosition testGridPosition)
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

        #region Support
        #endregion
    }
}