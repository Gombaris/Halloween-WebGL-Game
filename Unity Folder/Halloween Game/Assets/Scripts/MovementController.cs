using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Halloween
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private CharacterController _CharacterController = null;
        [SerializeField] private Transform _CameraPoint = null;
        [SerializeField] private Transform _GroundCheck = null;
        [SerializeField] private LayerMask _GroundMask;
        [SerializeField] private AudioClip SprintSound;
        [SerializeField] Animator _Animator;
        private GameObject _Camera;
        private GameObject _Aim;
        private AudioSource MovingSound;
        private AnimatorStateInfo AnimStateInfo;
        private Vector3 Velocity;
        private Vector3 _Move;
        private bool _OnGround, invokerun, _Reloading, _Shooting;
        private float _InputHorizontal, _InputVertical, _MouseY, _MouseX, _Speed;
        private float _RotationX = 0f, _RotationY = 0f, _RotationLimit = 60f, _gravitation = 9.81f;

        private void Start()
        {
            _Camera = GameObject.FindGameObjectWithTag("MainCamera");
            MovingSound = GetComponent<AudioSource>();
            _Aim = GameObject.FindGameObjectWithTag("Aim");
            _Speed = 2.5f;
            invokerun = true;
            _OnGround = true;

        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift) && (!AnimStateInfo.IsName("Reload")) && Time.timeScale == 1)
            {
                _Speed = 5f;
                if (invokerun == true)
                {
                    _Aim.SetActive(false);
                    StartCoroutine(CallSprintAudio());
                    _Animator.SetInteger("PlayerBehaviour", 1);
                }
            }
            else 
            {
                _Animator.SetInteger("PlayerBehaviour", 0);
                _Speed = 2.5f;
                _Aim.SetActive(true);
                invokerun = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Time.timeScale == 0)
            {
                MovingSound.Stop();
            }

            _InputHorizontal = Input.GetAxis("Horizontal");
            _InputVertical = Input.GetAxis("Vertical");
            Velocity.x = _InputHorizontal;
            Velocity.z = _InputVertical;
            _Move = Velocity;
            _Move.y = 0f;
            _Move = _CameraPoint.TransformDirection(_Move);
            Velocity.x = _Move.x;
            Velocity.z = _Move.z;
            _MouseX = Input.GetAxis("Mouse X");
            _MouseY = Input.GetAxis("Mouse Y");
            _RotationX -= _MouseY * 2f;
            _RotationY += _MouseX * 2f;
            if (_RotationX > _RotationLimit)
                _RotationX = 60f;
            if (_RotationX < -_RotationLimit)
            {
                _RotationX = -60f;
            }


        }
        void FixedUpdate()
        {

            if (_OnGround == true)
            {

                Velocity.y = -1f;

            }
            else
            {
                Velocity.y -= _gravitation * Time.deltaTime;
            }
            _CharacterController.Move(new Vector3(Velocity.x * _Speed, Velocity.y, Velocity.z * _Speed) * Time.deltaTime);
            _CameraPoint.rotation = Quaternion.Euler(_RotationX, _RotationY, 0f);
            _Camera.transform.position = _CameraPoint.position;
            _Camera.transform.rotation = _CameraPoint.rotation;

            if (Physics.OverlapSphere(_GroundCheck.position, 0.1f, _GroundMask).Length > 0 && Input.GetButtonDown("Jump"))
            {
                Jump();
            }


        }
        private void Jump()
        {

            Velocity.y = 3f;
            _OnGround = false;

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Ground" )
            {
                _OnGround = true;
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Ground" )
            {
                _OnGround = false;
            }
        }

        IEnumerator CallSprintAudio()
        {
            invokerun = false;
            MovingSound.clip = SprintSound;
            MovingSound.Play();
            yield return new WaitForSeconds(14f);
            invokerun = true;
        }
    }
}
