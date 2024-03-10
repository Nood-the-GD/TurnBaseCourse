using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Button _button;
        [SerializeField] private Image _border;
        private BaseAction _baseAction;

        public void SetBaseAction(BaseAction baseAction)
        {
            _baseAction = baseAction;
            _buttonText.text = baseAction.GetActionName();

            _button.onClick.AddListener(() =>
            {
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
        }

        #region Update visual
        public void UpdateSelectedVisual()
        {
            BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
            _border.gameObject.SetActive(selectedBaseAction == _baseAction);
        }
        #endregion
    }
}
