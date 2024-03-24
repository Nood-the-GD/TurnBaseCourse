using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpinAction : BaseAction
    {
        private float _totalSpinAmount;
        
        #region Unity functions
        private void Update()
        {
            if(_isActive)
            {
                float spinAmount = 360f * Time.deltaTime;
                _totalSpinAmount += spinAmount;
                if(_totalSpinAmount >= 360)
                {
                    ActionComplete();
                }
                transform.eulerAngles += new Vector3(0, spinAmount, 0);
            }
        }
        #endregion

        #region Override functions
        public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
        {
            _totalSpinAmount = 0;
            ActionStart(onSpinComplete);
        }
        public override List<GridPosition> GetValidGridPositionList()
        {
            GridPosition unitGridPosition = _unit.GetCurrentGridPosition();

            return new List<GridPosition>
            {
                unitGridPosition
            };
        }
        public override string GetActionName()
        {
            return "Spin";
        }
        public override int GetActionPointCost()
        {
            return 2;
        }
        public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                gridPosition = _unit.GetCurrentGridPosition(),
                actionValue = 0
            };
        }
        protected override bool IsGridPositionValid(GridPosition testGridPosition)
        {
            return true;
        }
        #endregion
    }
}
