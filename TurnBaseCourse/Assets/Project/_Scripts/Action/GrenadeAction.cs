using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GrenadeAction : BaseAction
    {
        [SerializeField] private int _maxThrowDistance = 7;
        [SerializeField] private GrenadeProjectile _grenadeProjectilePrefab;

        #region Unity functions
        private void Update()
        {
            if(!_isActive)
            {
                return;
            }
        }
        #endregion

        #region Override functions
        public override string GetActionName()
        {
            return "Grenade";
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

            GridPosition unitGridPosition = _unit.GetCurrentGridPosition();

            for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
            {
                for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
                {
                    GridPosition offSetGridPosition = new GridPosition(x, z, 0);
                    GridPosition testingGridPosition = unitGridPosition + offSetGridPosition;

                    if(IsGridPositionValid(testingGridPosition))
                        validGridPositionList.Add(testingGridPosition);
                }
            }

            return validGridPositionList;
        }
        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            GrenadeProjectile grenadeActionProjectile = Instantiate(_grenadeProjectilePrefab, _unit.GetWorldPosition(), Quaternion.identity);
            // Transform grenadeTransform = grenadeActionProjectile.transform;
            grenadeActionProjectile.Setup(gridPosition, OnGrenadeExploded);

            ActionStart(onActionComplete);
        }
        protected override bool IsGridPositionValid(GridPosition testGridPosition)
        {

            if (!LevelGrid.Instance.IsValidGrid(testGridPosition))
            {
                // Grid is not valid in this current level
                return false;
            }
            int testDistance = Mathf.Abs(testGridPosition.X) + Mathf.Abs(testGridPosition.Z);
            if (testDistance > _maxThrowDistance)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Support
        private void OnGrenadeExploded()
        {
            ActionComplete();
        }

        #endregion
    }
}
















