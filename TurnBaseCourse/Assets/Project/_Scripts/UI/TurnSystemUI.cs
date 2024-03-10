using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] private Button _endTurnBtn;
        [SerializeField] private TextMeshProUGUI _turnText;
        [SerializeField] private GameObject _enemyTurnGo;

        void Start()
        {
            _endTurnBtn.onClick.AddListener(() =>
            {
                TurnSystem.Instance.NextTurn();
            });
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
            UpdateTurnText();
            UpdateEnemyTurnVisual();
            UpdateEndTurnVisibility();
        }

        private void TurnSystem_OnTurnChange(object sender, EventArgs eventArgs)
        {
            UpdateTurnText();
            UpdateEnemyTurnVisual();
            UpdateEndTurnVisibility();
        }
        public void UpdateTurnText()
        {
            _turnText.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
        }
        private void UpdateEnemyTurnVisual()
        {
            _enemyTurnGo.SetActive(!TurnSystem.Instance.IsPlayerTurn());
        }

        private void UpdateEndTurnVisibility()
        {
            _endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
        }
    }
}
