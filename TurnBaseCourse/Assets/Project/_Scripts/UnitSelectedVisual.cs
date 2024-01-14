using System;
using UnityEngine;

namespace Game
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit _unit;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        private void Start()
        {
            UpdateVisual();
        }
        void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectUnitChange += UnitActionSystem_OnSelectUnitChange;
        }
        void OnDisable()
        {
            UnitActionSystem.Instance.OnSelectUnitChange -= UnitActionSystem_OnSelectUnitChange;
        }

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