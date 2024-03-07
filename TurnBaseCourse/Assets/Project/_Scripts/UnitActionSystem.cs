using System;
using NOOD;
using UnityEngine;

namespace Game
{
    public class UnitActionSystem : MonoBehaviorInstance<UnitActionSystem>
    {
        public event EventHandler OnSelectUnitChange;

        [SerializeField] private Unit _selectedUnit;
        [SerializeField] private LayerMask _unitLayer;

        private BaseAction _selectedAction;
        private bool _isBusy;


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

            if (TrySelectUnit()) return;

            HandleSelectedAction();
        }
         
        private void HandleSelectedAction()
        {
            if(Input.GetMouseButtonDown(0))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }

        private bool TrySelectUnit()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (MouseWorld.TryGetSelectedObjectWithLayer(_unitLayer, out GameObject go))
                {
                    SetSelectedUnit(go.GetComponent<Unit>());
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        private void SetBusy()
        {
            _isBusy = true;
        }
        private void ClearBusy()
        {
            _isBusy = false;    
        }

        private void SetSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;
            _selectedAction = unit.GetMoveAction();
            OnSelectUnitChange?.Invoke(this, EventArgs.Empty);
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            _selectedAction = baseAction;
        }

        public Unit GetSelectedUnit()
        {
            return _selectedUnit;
        }
    }
}