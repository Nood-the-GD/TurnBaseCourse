using System;
using NOOD;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class UnitActionSystem : MonoBehaviorInstance<UnitActionSystem>
    {
        public event EventHandler OnSelectUnitChange;
        public event EventHandler OnSelectActionChange;
        public event EventHandler<bool> OnActionBusyChange;
        public event EventHandler OnActionStarted;

        [SerializeField] private Unit _selectedUnit;
        [SerializeField] private LayerMask _unitLayer;

        private BaseAction _selectedAction;
        private bool _isBusy;

        #region UnityFunctions
        private void Start()
        {
            SetSelectedUnit(_selectedUnit);
        }

        private void Update()
        {
            if(_isBusy)
            {
                return;
            }

            if(!TurnSystem.Instance.IsPlayerTurn())
            {
                return;
            }

            if (TrySelectUnit()) return;
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            HandleSelectedAction();
        }
        #endregion

        #region Mouse Actions
        private void HandleSelectedAction()
        {
            if(Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
                if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) return;

                SetBusy();
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                OnActionStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool TrySelectUnit()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (!MouseWorld.IsTryGetSelectedObject(out GameObject gameObject)) return false;
                
                if (MouseWorld.TryGetSelectedObjectWithLayer(_unitLayer, out GameObject go))
                {
                    Unit unit = go.GetComponent<Unit>();
                   if(unit.IsEnemy())
                    {
                        return false;
                    }
                    if(unit == this._selectedUnit)
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
                else
                    return false;
            }
            return false;
        }
        #endregion

        #region Busy zone
        private void SetBusy()
        {
            _isBusy = true;
            OnActionBusyChange?.Invoke(this, true);
        }
        private void ClearBusy()
        {
            _isBusy = false;
            OnActionBusyChange?.Invoke(this, false);
        }
        #endregion

        #region Unit zone
        private void SetSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;
            _selectedAction = unit.GetAction<MoveAction>();
            OnSelectUnitChange?.Invoke(this, EventArgs.Empty);
        }
        public Unit GetSelectedUnit()
        {
            return _selectedUnit;
        }
        #endregion

        #region Action zone
        public void SetSelectedAction(BaseAction baseAction)
        {
            _selectedAction = baseAction;
            OnSelectActionChange?.Invoke(this, EventArgs.Empty);
        }
        public BaseAction GetSelectedAction()
        {
            return _selectedAction;
        }
        #endregion
    }
}