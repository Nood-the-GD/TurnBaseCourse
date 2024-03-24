using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InteractionAction : BaseAction
    {
        #region Variables
        [SerializeField] private int _maxInteractDistance = 1;
        #endregion

        #region Unity functions
        void Update()
        {
            if (!_isActive) return;
        }
        #endregion

        #region Override functions
        public override string GetActionName()
        {
            return "Interact";
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
            List<GridPosition> result = new List<GridPosition>();
            for(int x = -_maxInteractDistance; x <= _maxInteractDistance; x++)
            {
                for (int z = -_maxInteractDistance; z <= _maxInteractDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z, 0);
                    GridPosition testGridPosition = _unit.GetCurrentGridPosition() + offsetGridPosition;

                    Debug.Log(x + " " + z);
                    if(IsGridPositionValid(testGridPosition))
                        result.Add(testGridPosition);
                }
            }
            return result;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            IInteractable interactable = LevelGrid.Instance.GetInteractableAtThisGridPosition(gridPosition);

            interactable.Interact(ActionComplete);

            ActionStart(onActionComplete);
        }

        protected override bool IsGridPositionValid(GridPosition testGridPosition)
        {
            if(!LevelGrid.Instance.IsValidGrid(testGridPosition))
            {
                // Grid is not valid in this current level
                return false;
            }

            IInteractable interactable = LevelGrid.Instance.GetInteractableAtThisGridPosition(testGridPosition);
            if(interactable == null)
            {
                return false;
            }


            return true;
        }
        #endregion
    }
}
