using UnityEngine;

namespace Game
{
    public class ActionBusyUI : MonoBehaviour
    {

        private void Start()
        {
            UnitActionSystem.Instance.OnActionBusyChange += UnitActionSystem_OnActionBusyChangeHandler;
            this.gameObject.SetActive(false);
        }

        private void UnitActionSystem_OnActionBusyChangeHandler(object sender, bool value)
        {
            this.gameObject.SetActive(value);
        }
    }
}
