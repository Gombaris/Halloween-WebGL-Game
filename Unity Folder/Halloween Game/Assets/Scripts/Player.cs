using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Halloween
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private MovementController _MovementController = null;
        [SerializeField] private WeaponFire _WeaponFire = null;
        [SerializeField] private AudioClip HitSound;
        private AudioSource HitSoundSource;
        private int _PlayerMaxHealth = 10, _PlayerHealth;

        public static Player Instance { get; private set; }

        public int PlayerHealth
        {
            get { return _PlayerHealth; }
            set { _PlayerHealth = value; Damaged(); }

        }

        void Start()
        {
            Instance = this;
            _PlayerHealth = _PlayerMaxHealth;
            GameManager.Instance.Lives = _PlayerHealth;
            HitSoundSource = GetComponent<AudioSource>();
        }


        void Update()
        {
            GameManager.Instance.Lives = _PlayerHealth;
            if (_PlayerHealth <= 0)
            {
                YouDied();
            }
        }

        public void Damaged()
        {
            HitSoundSource.clip = HitSound;
            HitSoundSource.Play();

        }
        private void YouDied()
        {

        }
    }
}
