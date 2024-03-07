using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BaseAction : MonoBehaviour
    {
        protected Action OnActionComplete;
        protected Unit _unit;
        protected bool _isActive;

        protected virtual void Awake()
        {
            _unit = GetComponent<Unit>();
        }
        public abstract string GetActionName();
        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
        public bool IsValidActionGridPosition(GridPosition gridPositionStruct)
        {
            List<GridPosition> validGridPositionList = GetValidGridPositionList();
            return validGridPositionList.Contains(gridPositionStruct);
        }
        public abstract List<GridPosition> GetValidGridPositionList();
    }
}
