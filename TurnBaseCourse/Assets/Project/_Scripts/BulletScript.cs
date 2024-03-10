using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BulletScript : MonoBehaviour
    {
        #region Events
        public static event EventHandler<OnBulletSetTargetEventArgs> OnBulletSetTarget;
        public class OnBulletSetTargetEventArgs : EventArgs
        {
            public Vector3 shootPos;
        }
        #endregion

        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private ParticleSystem _bulletHitEff;

        private Vector3 _targetPos;
        private float _speed = 100f;

        private void Update()
        {
            Vector3 moveDir = (_targetPos - transform.position).normalized;

            float distanceBeforeMoving = Vector3.Distance(this.transform.position, _targetPos);

            transform.position += moveDir * _speed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(this.transform.position, _targetPos);
             
            if(distanceBeforeMoving < distanceAfterMoving)
            {
                this.transform.position = _targetPos;
                _trailRenderer.transform.SetParent(null);
                Instantiate(_bulletHitEff, _targetPos, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }

        public void Setup(Vector3 targetPos)
        {
            _targetPos = targetPos;
            OnBulletSetTarget?.Invoke(this, new OnBulletSetTargetEventArgs 
            { 
                shootPos = _targetPos 
            });
        }
    }
}
