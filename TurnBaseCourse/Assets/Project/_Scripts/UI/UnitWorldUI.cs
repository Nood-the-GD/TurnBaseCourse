using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UnitWorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _actionPointText;
        [SerializeField] private Unit _unit;
        [SerializeField] private Image _healthBarImage;
        [SerializeField] private HealthSystem _healthSystem;

        #region Unit functions
        private void Start()
        {
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChangedHandler;
            _healthSystem.OnDamage += HealthSystem_OnDamageHandler;
            UpdateActionPointText();
            UpdateHealthBar();
        }
        #endregion

        #region Event functions
        private void Unit_OnAnyActionPointsChangedHandler(object sender, EventArgs eventArgs)
        {
            UpdateActionPointText();
        }
        private void HealthSystem_OnDamageHandler(object sender, EventArgs eventArgs)
        {
            UpdateHealthBar();
        }
        #endregion

        #region Update visual
        private void UpdateActionPointText()
        {
            _actionPointText.text = _unit.GetActionPoint().ToString();
        }
        private void UpdateHealthBar()
        {
            _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();
        }
        #endregion
    }
}
