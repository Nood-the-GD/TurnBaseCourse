using System;
using UnityEngine;

namespace Game
{
    public class GrenadeProjectile : MonoBehaviour
    {
        #region Variables
        public static event EventHandler onAnyGrenadeExploded;

        private Action onGrenadeExploded;

        [SerializeField] private Transform _grenadeExplodeVFXPrefab;
        [SerializeField] private TrailRenderer _grenadeTrailVFXPrefab;
        [SerializeField] private float _damageRadius = 4f;
        [SerializeField] private int _damage = 60;
        [SerializeField] private AnimationCurve _throwGrenadeCurve;

        private Vector3 _targetPosition;
        private float _speed = 10f;
        private float _totalDistance;
        private Vector3 _positionXZ;
        #endregion

        #region Unity functions
        private void Update()
        {
            Vector3 moveDir = (_targetPosition - _positionXZ).normalized;

            _positionXZ += moveDir * _speed * Time.deltaTime;

            float distance = Vector3.Distance(_positionXZ, _targetPosition);
            float distanceNormalized = 1 - (distance/_totalDistance);

            float maxHeight = _totalDistance / 4f;
            float positionY = _throwGrenadeCurve.Evaluate(distanceNormalized) * maxHeight;

            this.transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);

            if (Vector3.Distance(_positionXZ, _targetPosition) < 0.1f)
            {
                // Reached target
                Collider[] colliders = Physics.OverlapSphere(_targetPosition, _damageRadius);
                foreach(Collider collider in colliders)
                {
                    if(collider.TryGetComponent(out Unit targetUnit))
                    {
                        targetUnit.Damage(_damage, _targetPosition);
                    }
                    if(collider.TryGetComponent(out DestructionCrate destructionCrate))
                    {
                        destructionCrate.Damage(this.transform.position);
                    }
                }

                onGrenadeExploded?.Invoke();
                onAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

                Instantiate(_grenadeExplodeVFXPrefab, _targetPosition + Vector3.up * 1f, Quaternion.identity);

                Destroy(gameObject);
                _grenadeTrailVFXPrefab.transform.parent = null;
            }            
        }
        #endregion

        #region Setup
        public void Setup(GridPosition targetGridPosition, Action onGrenadeExploded)
        {
            this.onGrenadeExploded = onGrenadeExploded;
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

            _positionXZ = this.transform.position;
            _positionXZ.y = 0f;

            _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);            
        }
        #endregion
    }
}













