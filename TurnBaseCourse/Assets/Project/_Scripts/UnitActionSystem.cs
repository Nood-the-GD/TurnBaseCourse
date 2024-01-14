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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (TrySelectUnit() == false)
                {

                    GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                    if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
                    {
                        _selectedUnit.GetMoveAction().SetTargetPosition(mouseGridPosition);
                    }
                }
            }
        }

        private bool TrySelectUnit()
        {
            if (MouseWorld.TryGetSelectedObjectWithLayer(_unitLayer, out GameObject go))
            {
                SetSelectedUnit(go.GetComponent<Unit>());
                return true;
            }
            else
                return false;
        }

        private void SetSelectedUnit(Unit unit)
        {
            _selectedUnit = unit;
            OnSelectUnitChange?.Invoke(this, EventArgs.Empty);
        }

        public Unit GetSelectedUnit()
        {
            return _selectedUnit;
        }
    }
}