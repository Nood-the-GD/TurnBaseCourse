using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UnitRagdoll : MonoBehaviour
    {
        [SerializeField] private Transform _ragdollRootBone;

        public void Setup(Transform originalRootBone, Vector3 explosionPos)
        {
            MathAllChildTransform(originalRootBone, _ragdollRootBone);
            ApplyExplosionToRagdoll(_ragdollRootBone, 1000f, explosionPos, 10f);
            Debug.Log(explosionPos);
        }

        private void MathAllChildTransform(Transform root, Transform clone)
        {
            foreach(Transform child in root)
            {
                Transform cloneChild = clone.Find(child.name);
                if(cloneChild != null)
                {
                    cloneChild.SetPositionAndRotation(child.position, child.rotation);

                    MathAllChildTransform(child, cloneChild);
                }
            }
        }
        private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
        {
            foreach(Transform child in root)
            {
                if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                {
                    childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);             
                }
                ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}
