using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform _actionBtnPref;
        [SerializeField] private Transform _actionBtnContainerTransform;
        [SerializeField] private TextMeshProUGUI _actionPointText;

        private List<ActionButtonUI> _actionButtonUIs = new List<ActionButtonUI>();

        #region Unity functions
        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectUnitChange += UnitActionSystem_OnSelectUnitChangeHandler;
            UnitActionSystem.Instance.OnSelectActionChange += UnitActionSystem_OnSelectActionChangeHandler;
            UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStartedHandler;
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        }
        private void Start()
        {
            
            CreateUnitActionButtons();
            UpdateSelectionVisual();
            UpdateActionPoint();
        }
        #endregion

        #region Action button
        private void CreateUnitActionButtons()
        {
            foreach(Transform buttonTrans in _actionBtnContainerTransform)
            {
                Destroy(buttonTrans.gameObject);
            }
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            _actionButtonUIs.Clear();
            foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                Transform actionButton = Instantiate(_actionBtnPref, _actionBtnContainerTransform);
                ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);

                _actionButtonUIs.Add(actionButtonUI);
            }
        }
        private void UpdateSelectionVisual()
        {
            foreach(ActionButtonUI actionButtonUI in _actionButtonUIs)
            {
                actionButtonUI.UpdateSelectedVisual();
            }
        }
        #endregion

        #region Event functions
        private void UnitActionSystem_OnSelectUnitChangeHandler(object sender, EventArgs eventArgs)
        {
            CreateUnitActionButtons();
            UpdateSelectionVisual();
            UpdateActionPoint();
        }
        private void UnitActionSystem_OnSelectActionChangeHandler(object sender, EventArgs eventArgs)
        {
            UpdateSelectionVisual();
        }
        private void UnitActionSystem_OnActionStartedHandler(object sender, EventArgs eventArgs)
        {
            UpdateActionPoint();
        }
        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs eventArgs)
        {
            UpdateActionPoint();
        }
        #endregion

        #region Action Point
        private void UpdateActionPoint()
        {
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            _actionPointText.text = "Action point: " + selectedUnit.GetActionPoint();
        }
        #endregion
    }
}
