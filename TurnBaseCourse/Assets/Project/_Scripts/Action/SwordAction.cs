using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SwordAction : BaseAction
    {
        #region Events
        public event EventHandler onSwordActionStarted;
        public event EventHandler onSwordActionCompleted;
        #endregion

        #region Enum
        private enum State
        {
            SwingSwordBeforeHit,
            SwingSwordAfterHit
        }
        #endregion

        #region Variables
        private int _maxSwordDistance = 1;
        private float _stateTimer;
        private State _state;
        private Unit _targetUnit;
        #endregion

        #region Unity functions
        private void Update()
        {
            if(!_isActive) return;

            _stateTimer -= Time.deltaTime;

            switch (_state)
            {
                case State.SwingSwordBeforeHit:
                    Vector3 aimDir = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;

                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);
                    break;
                case State.SwingSwordAfterHit:
                    break;
            }
            if(_stateTimer <= 0)
            {
                NextState();
            }
        }
        #endregion

        #region Support
        private void NextState()
        {
            switch (_state)
            {
                case State.SwingSwordBeforeHit:
                    _state = State.SwingSwordAfterHit;
                    float beforeHitStateTime = 0.5f;
                    _stateTimer = beforeHitStateTime;
                    Vector3 hitPos = _targetUnit.GetWorldPosition() + Vector3.up * 1.7f;
                    _targetUnit.Damage(100, hitPos);
                    break;
                case State.SwingSwordAfterHit:
                    onSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                    break;
            }
        }
        public int GetRange()
        {
            return _maxSwordDistance;
        }
        #endregion

        #region Override functions
        public override string GetActionName()
        {
            return "Sword";
        }
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                gridPosition = gridPosition,
                actionValue = 0
            };
        }
        public override List<GridPosition> GetValidGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
            {
                for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
                {
                    GridPosition offSetGridPosition = new GridPosition(x, z, 0);
                    GridPosition testingGridPosition = _unit.GetCurrentGridPosition() + offSetGridPosition;

                    // Debug.Log(x + " " + z);
                    if(IsGridPositionValid(testingGridPosition))
                        validGridPositionList.Add(testingGridPosition);
                }
            }

            return validGridPositionList;
        }
        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            _state = State.SwingSwordBeforeHit;
            float beforeHitStateTime = 0.7f;
            _stateTimer = beforeHitStateTime;

            onSwordActionStarted?.Invoke(this, EventArgs.Empty);
            ActionStart(onActionComplete);
        }
        protected override bool IsGridPositionValid(GridPosition testGridPosition)
        {
            if(!LevelGrid.Instance.IsValidGrid(testGridPosition))
            {
                // Grid is not valid in this current level
                return false;
            }

            // int testDistance = Mathf.Abs(testGridPosition.X) + Mathf.Abs(testGridPosition.Z);
            // if(testDistance > _maxSwordDistance)
            // {
            //     return false;
            // }

            if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
            {
                // Grid is empty, no unit
                return false;
            }

            Unit unit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

            if(unit.IsEnemy() == _unit.IsEnemy())
            {
                // Both Unit on same "team"
                return false;
            }

            return true;
        }
        #endregion
    }
}
