using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UnitActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform _actionBtnPref;
        [SerializeField] private Transform _actionBtnContainerTransform;

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectUnitChange += UnitActionSystem_OnSelectUnitChangeHandler;
            CreateUnitActionButtons();
        }

        private void CreateUnitActionButtons()
        {
            foreach(Transform buttonTrans in _actionBtnContainerTransform)
            {
                Destroy(buttonTrans.gameObject);
            }
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

            foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                Transform actionButton = Instantiate(_actionBtnPref, _actionBtnContainerTransform);
                ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);
            }
        }
        private void UnitActionSystem_OnSelectUnitChangeHandler(object sender, EventArgs eventArgs)
        {
            CreateUnitActionButtons();
        }
    }
}
