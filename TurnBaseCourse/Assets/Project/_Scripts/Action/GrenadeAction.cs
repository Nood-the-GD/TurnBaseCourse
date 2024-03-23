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
                    GridPosition offSetGridPosition = new GridPosition(x, z);
                    GridPosition testingGridPosition = unitGridPosition + offSetGridPosition;

                    if (!LevelGrid.Instance.IsValidGrid(testingGridPosition))
                    {
                        // Grid is not valid in this current level
                        continue;
                    }
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > _maxThrowDistance)
                    {
                        continue;
                    }

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
        #endregion

        #region Support
        private void OnGrenadeExploded()
        {
            ActionComplete();
        }
        #endregion
    }
}
















