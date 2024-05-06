using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Halloween
{
    public class Skeleton : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider _MainCollider;
        [SerializeField] private GameObject _Rig;
        [SerializeField] private Animator _Animator;
        [SerializeField] private EnemyBehaviour _Behaviour;
        [SerializeField] private AudioClip[] SkeletonSoundsArray;
        private Collider[] RagdollCollider;
        private Rigidbody[] RagdollRigidbody;
        private AudioSource RandomSkeletonSounds;
        private int _Health, _MaxHealth = 1;
        private bool _BoolDie;

        void Start()
        {
            _Health = _MaxHealth;
            _BoolDie = true;
            _Animator = GetComponent<Animator>();
            RandomSkeletonSounds = GetComponent<AudioSource>();
            GetRagdollBones();
            RagdollOff();
            CallAudio();
        }

        void Update()
        {

            if (_Health <= 0)
            {
                if (_BoolDie == true )
                {
                    _Behaviour.Died();
                    RagdollOn();
                    GameManager.Instance.Enemies -= 1;
                    _BoolDie = false;
                }
                Destroy(gameObject, 5);
            }
            


        }

        private void GetRagdollBones()
        {
            RagdollCollider = _Rig.GetComponentsInChildren<Collider>();
            RagdollRigidbody = _Rig.GetComponentsInChildren<Rigidbody>();
        }
        public void RagdollOn()
        {
            _Animator.enabled = false;
            foreach (Collider col in RagdollCollider)
            {
                col.enabled = true;
            }
            foreach (Rigidbody rig in RagdollRigidbody)
            {
                rig.isKinematic = false;
            }
            _Animator.enabled = false;
            _MainCollider.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        private void RagdollOff()
        {
            foreach (Collider col in RagdollCollider)
            {
                col.enabled = false;
            }
            foreach (Rigidbody rig in RagdollRigidbody)
            {
                rig.isKinematic = true;
            }
            _Animator.enabled = true;
            _MainCollider.enabled = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        public void TakeDamage(int damageAmount)
        {
            _Health -= damageAmount;
        }
        private void CallAudio()
        {
            Invoke("RandomSounds", 4);
        }

        private void RandomSounds()
        {
            RandomSkeletonSounds.clip = SkeletonSoundsArray[Random.Range(0, SkeletonSoundsArray.Length)];
            RandomSkeletonSounds.Play();
            CallAudio();
        }

    }

}