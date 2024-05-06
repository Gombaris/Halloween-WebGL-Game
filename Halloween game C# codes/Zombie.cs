using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Halloween
{
    public class Zombie : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider _MainCollider;
        [SerializeField] private GameObject _Rig;
        [SerializeField] private Animator _Animator;
        [SerializeField] private EnemyBehaviour _Behaviour;
        [SerializeField] private AudioClip[] _ZombieSoundsArray;
        private AudioSource _RandomZombieSounds;
        private Collider[] _RagdollCollider;
        private Rigidbody[] _RagdollRigidbody;
        private int _Health, _MaxHealth = 3;
        private bool _BoolDie;

        void Start()
        {
            _Health = _MaxHealth;
            _BoolDie = true;
            _Animator = GetComponent<Animator>();
            _RandomZombieSounds = GetComponent<AudioSource>();
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
            _RagdollCollider = _Rig.GetComponentsInChildren<Collider>();
            _RagdollRigidbody = _Rig.GetComponentsInChildren<Rigidbody>();
        }
        public void RagdollOn()
        {
            _Animator.enabled = false;
            foreach (Collider col in _RagdollCollider)
            {
                col.enabled = true;
            }
            foreach (Rigidbody rig in _RagdollRigidbody)
            {
                rig.isKinematic = false;
            }
            _Animator.enabled = false;
            _MainCollider.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        private void RagdollOff()
        {
            foreach (Collider col in _RagdollCollider)
            {
                col.enabled = false;
            }
            foreach (Rigidbody rig in _RagdollRigidbody)
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
            Invoke("RandomSounds", 5);
        }

        private void RandomSounds()
        {
            _RandomZombieSounds.clip = _ZombieSoundsArray[Random.Range(0, _ZombieSoundsArray.Length)];
            _RandomZombieSounds.Play();
            CallAudio();
        }

    }

}