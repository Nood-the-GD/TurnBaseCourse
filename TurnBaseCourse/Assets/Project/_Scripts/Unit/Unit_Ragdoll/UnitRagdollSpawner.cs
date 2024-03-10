using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.HealthSystem;

namespace Game
{
    public class UnitRagdollSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _ragdollPref;
        [SerializeField] private Transform _originalRootBone;
        private HealthSystem _healthSystem;


        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();

            _healthSystem.OnDead += HealthSystem_OnDead;
        }

        private void HealthSystem_OnDead(object sender, OnDeadEventArgs  eventArgs)
        {
            Transform ragdollTrans = Instantiate(_ragdollPref, transform.position, transform.rotation);
            UnitRagdoll unitRagdoll = ragdollTrans.GetComponent<UnitRagdoll>();
            unitRagdoll.Setup(_originalRootBone, eventArgs.shotPosition);
        }
    }
}
