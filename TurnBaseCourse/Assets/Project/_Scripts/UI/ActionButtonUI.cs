using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ActionButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Button _button;

        public void SetBaseAction(BaseAction baseAction)
        {
            _buttonText.text = baseAction.GetActionName();

            _button.onClick.AddListener(() =>
            {
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
        }

    }
}
