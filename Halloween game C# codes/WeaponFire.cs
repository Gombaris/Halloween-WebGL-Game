using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Halloween
{
    public class WeaponFire : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Animator _PlayerAnimator;
        [SerializeField] private AudioClip FireSound;
        [SerializeField] private AudioClip ReloadSound;
        private AudioSource WeaponSound;
        private GameObject _Camera;
        private GameObject currentBullet;
        private int _MaxAmmo = 20, _CurrentAmmo = 5, _MaxClipAmmo = 5, _CurrentClipAmmo = 5;
        private float shootForce = 20f;
        private bool CanReloadAudio;

        private void Start()
        {
            _Camera = GameObject.FindGameObjectWithTag("MainCamera");
            _PlayerAnimator.SetInteger("PlayerBehaviour", 0);
            _CurrentClipAmmo = _MaxClipAmmo;
            GameManager.Instance.ClipAmmunition = _CurrentClipAmmo;
            GameManager.Instance.Ammunition = _CurrentAmmo;
            WeaponSound = GetComponent<AudioSource>();
            CanReloadAudio = true;
        }

        private void Update()
        {
            GameManager.Instance.ClipAmmunition = _CurrentClipAmmo;
            GameManager.Instance.Ammunition = _CurrentAmmo;
            if (Time.timeScale != 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (_CurrentClipAmmo > 0)
                    {
                        _PlayerAnimator.SetTrigger("Shoot");
                        Shoot();

                    }
                    else
                    {
                        Reload();
                    }
                }
                Destroy(currentBullet, 3);
                if (Input.GetKeyDown(KeyCode.R) && _CurrentClipAmmo < _MaxClipAmmo)
                {
                    Reload();
                }
            }
        }


        private void Shoot()
        {
            AnimatorStateInfo animStateInfo;
            animStateInfo = _PlayerAnimator.GetCurrentAnimatorStateInfo(0);

            if (!animStateInfo.IsName("Reload") && (!animStateInfo.IsName("Run")))
            {
                WeaponSound.clip = FireSound;
                WeaponSound.Play();
                _CurrentClipAmmo -= 1;
                GameManager.Instance.ClipAmmunition = _CurrentClipAmmo;
                currentBullet = Instantiate(_bullet, _attackPoint.position, Quaternion.identity);
                currentBullet.GetComponent<Rigidbody>().AddForce(shootForce * _Camera.transform.forward, ForceMode.Impulse);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Ammo" && _CurrentAmmo < _MaxAmmo)
            {
                Destroy(other.transform.parent.gameObject);
                _CurrentAmmo += 5;
                if (_CurrentAmmo > _MaxAmmo)
                {
                    _CurrentAmmo = _MaxAmmo;
                }
                GameManager.Instance.Ammunition = _CurrentAmmo;
            }
        }
        private void Reload()
        {
            AnimatorStateInfo animStateInfo;
            animStateInfo = _PlayerAnimator.GetCurrentAnimatorStateInfo(0);
            if (_CurrentAmmo > 0 && (!animStateInfo.IsName("Run")))
            {
                if (CanReloadAudio == true)
                {
                    StartCoroutine(CallReloadAudio());
                }
                _PlayerAnimator.SetTrigger("Reload");

                if (_CurrentAmmo < _MaxClipAmmo)
                {
                    if ((_MaxClipAmmo - _CurrentClipAmmo) >= _CurrentAmmo)
                    {
                        _CurrentClipAmmo += _CurrentAmmo;
                        _CurrentAmmo -= _CurrentAmmo;

                    }
                    else
                    {
                        int reloadsum = _MaxClipAmmo - _CurrentClipAmmo;
                        _CurrentClipAmmo += reloadsum;
                        _CurrentAmmo -= reloadsum;
                    }
                }
                else
                {
                    int reloadsum = _MaxClipAmmo - _CurrentClipAmmo;
                    _CurrentClipAmmo += reloadsum;
                    _CurrentAmmo -= reloadsum;
                }
            }

        }
        IEnumerator CallReloadAudio()
        {
            CanReloadAudio = false;
            WeaponSound.clip = ReloadSound;
            WeaponSound.Play();
            yield return new WaitForSeconds(2f);
            CanReloadAudio = true;
        }


    }
}
