using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.BulletScript;

namespace Game
{
    public class HealthSystem : MonoBehaviour
    {
        #region Events
        public event EventHandler<OnDeadEventArgs> OnDead;
        public class OnDeadEventArgs : EventArgs
        {
            public Vector3 shotPosition;
        }
        public event EventHandler OnDamage;
        #endregion

        [SerializeField] private int _health = 100;
        private int _healthMax = 100;
        private Vector3 _shotPosition;

        #region Unity functions
        private void Start()
        {
            BulletScript.OnBulletSetTarget += BulletScript_OnBulletSetTargetHandler;
        }
        #endregion

        #region Damage
        public void Damage(int damageAmount)
        {
            _health -= damageAmount;
            OnDamage?.Invoke(this, EventArgs.Empty);
            if(_health <= 0) 
            {
                _health = 0;
                Die();
            }
        }
        private void Die()
        {
            OnDead?.Invoke(this, new OnDeadEventArgs
            {
                shotPosition = _shotPosition
            });
        }
        #endregion

        #region Event functions
        private void BulletScript_OnBulletSetTargetHandler(object sender, OnBulletSetTargetEventArgs onBulletSetTarget)
        {
            _shotPosition = onBulletSetTarget.shootPos;
        }
        #endregion


        #region Support
        public float GetHealthNormalized()
        {
            return (float) _health / _healthMax;
        }
        #endregion
    }
}
