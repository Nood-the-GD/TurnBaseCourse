using System;
using UnityEngine;

namespace Game
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit _unit;

        private MeshRenderer _meshRenderer;

        #region Unity Functions
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        private void Start()
        {
            UpdateVisual();
        }
        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectUnitChange += UnitActionSystem_OnSelectUnitChange;
        }
        private void OnDisable()
        {
            UnitActionSystem.Instance.OnSelectUnitChange -= UnitActionSystem_OnSelectUnitChange;
        }
        #endregion

        private void UnitActionSystem_OnSelectUnitChange(object sender, EventArgs eventArgs)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (UnitActionSystem.Instance.GetSelectedUnit() == _unit)
                _meshRenderer.enabled = true;
            else
                _meshRenderer.enabled = false;
        }
    }
}