using System;
using UnityEngine;
using static Game.ShootAction;

namespace Game
{
    public class UnitAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _unitAnimator;

        private void Awake()
        {
            if(TryGetComponent<MoveAction>(out MoveAction moveAction))
            {
                moveAction.OnStartMoving += MoveAction_OnStartMoving;
                moveAction.OnStopMoving += MoveAction_OnStopMoving;
            }
            if(TryGetComponent<ShootAction>(out ShootAction shootAction))
            {
                shootAction.OnShoot += ShootAction_OnShoot;
            }
        }

        #region Event functions
        private void ShootAction_OnShoot(object sender, OnShootEventArgs eventArgs)
        {
            _unitAnimator.SetTrigger("Shoot");
        }
        private void MoveAction_OnStartMoving(object sender, EventArgs eventArgs)
        {
            _unitAnimator.SetBool("Walk", true);
        }
        private void MoveAction_OnStopMoving(object sender, EventArgs eventArgs)
        {
            _unitAnimator.SetBool("Walk", false);
        }
        #endregion
    }
}
