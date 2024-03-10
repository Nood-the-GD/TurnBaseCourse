using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class BaseAction : MonoBehaviour
    {
        #region Events
        public static event EventHandler OnAnyActionStarted;
        #endregion

        #region Variables
        protected Action OnActionComplete;
        protected Unit _unit;
        protected bool _isActive;
        #endregion

        #region Protected functions
        protected virtual void Awake()
        {
            _unit = GetComponent<Unit>();
        }
        protected void ActionStart(Action onActionComplete)
        {
            _isActive = true;
            this.OnActionComplete = onActionComplete;
            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }
        protected void  ActionComplete()
        {
            _isActive = false;
            OnActionComplete?.Invoke(); 
        }
        #endregion

        #region Abstract functions
        public abstract string GetActionName();
        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
        public abstract List<GridPosition> GetValidGridPositionList();
        #endregion

        #region Public functions
        public bool IsValidActionGridPosition(GridPosition gridPositionStruct)
        {
            List<GridPosition> validGridPositionList = GetValidGridPositionList();
            return validGridPositionList.Contains(gridPositionStruct);
        }
        public virtual int GetActionPointCost()
        {
            return 1;
        }
        #endregion
    }
}
