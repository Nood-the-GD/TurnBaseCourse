using System;
using UnityEngine;
using static Game.ShootAction;

namespace Game
{
    public class UnitAnimator : MonoBehaviour
    {
        #region Variables
        private const string SWORD_ATTACK_ANIMATION_NAME = "SwordAttack";
        private const string SHOOT_ANIMATION_NAME = "Shoot";
        private const string WALK_ANIMATION_NAME = "Walk";
        #endregion

        [SerializeField] private Animator _unitAnimator;
        [SerializeField] private Transform _gun, _sword;

        #region Unity functions
        private void OnEnable()
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
            if(TryGetComponent<SwordAction>(out SwordAction swordAction))
            {
                swordAction.onSwordActionStarted += SwordAction_OnSwordActionStarted;
                swordAction.onSwordActionCompleted += SwordAction_OnSwordActionCompleted;
            }
        }
        private void Start()
        {
            EquipGun();
        }
        #endregion

        #region Event functions
        private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
        {
            EquipSword();
            _unitAnimator.SetTrigger(SWORD_ATTACK_ANIMATION_NAME);
        }
        private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
        {
            EquipGun();
        }
        private void ShootAction_OnShoot(object sender, OnShootEventArgs eventArgs)
        {
            _unitAnimator.SetTrigger(SHOOT_ANIMATION_NAME);
        }
        private void MoveAction_OnStartMoving(object sender, EventArgs eventArgs)
        {
            _unitAnimator.SetBool(WALK_ANIMATION_NAME, true);
        }
        private void MoveAction_OnStopMoving(object sender, EventArgs eventArgs)
        {
            _unitAnimator.SetBool(WALK_ANIMATION_NAME, false);
        }
        #endregion

        #region Support
        private void EquipSword()
        {
            _sword.gameObject.SetActive(true);
            _gun.gameObject.SetActive(false);
        }
        private void EquipGun()
        {
            _sword.gameObject.SetActive(false);
            _gun.gameObject.SetActive(true);
        }
        #endregion
    }
}















