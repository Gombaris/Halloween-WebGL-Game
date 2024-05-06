using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Halloween
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _NavMeshAgent;
        [SerializeField] private Animator _AnimatorTrigger;
        private Player _Player;
        private Transform _PlayerPos;
        private float _Time, _CurrentTimeDest;
        private bool _Move = true, _Alive = true, _Startcount = false, _Attack = false, _DamageCondition = true;

        void Start()
        {
            StartCoroutine( Waiting());
            _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _PlayerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _NavMeshAgent.SetDestination(_PlayerPos.transform.position);
            _DamageCondition = true;
        }

        // Update is called once per frame
        void Update()
        {
            _CurrentTimeDest += Time.deltaTime;
           
            if (_Alive == true  && ( _Startcount == true || _Time != 0f ))
            {
                _Time += Time.deltaTime;
                _Attack = true;
                _NavMeshAgent.SetDestination(_NavMeshAgent.transform.position);
                _AnimatorTrigger.SetInteger("Behaviour", 1);

                if (_Time > 1f && _Time < 3f)
                {
                   if ( _DamageCondition == true)
                    {
                        if (gameObject.GetComponent<SkeletonWarrior>())
                        {
                            Player.Instance.PlayerHealth -= 2;
                            _DamageCondition = false;
                        }
                        else
                        {
                            Player.Instance.PlayerHealth -= 1;
                            _DamageCondition = false;
                        }
                    }
                    _AnimatorTrigger.SetInteger("Behaviour", 2);
                }
                else if (_Time >= 3f)
                {
                    _AnimatorTrigger.SetInteger("Behaviour", 0);
                    _Attack = false;
                    _DamageCondition = true;
                    _Time = 0f;
                }
            }
        }

        void FixedUpdate()
        {
            if (_Move == true && _Alive == true && _Startcount == false && _Attack == false)
            {
                if (_CurrentTimeDest > 0.25f)
                {
                    var targetRotation = Quaternion.LookRotation(_PlayerPos.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
                }
                    if (_CurrentTimeDest > 0.5f)
                {
                    _NavMeshAgent.SetDestination(_PlayerPos.transform.position);
                    _CurrentTimeDest = 0f;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_Alive == true && other.gameObject.tag == "Player")
            {
                if (_Time == 0f)
                {
                    _Move = false;
                    _Startcount = true;
                    _Attack = true;
                    _DamageCondition = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_Alive == true && other.gameObject.tag == "Player")
            {
                _Move = true;
                _Startcount = false;
            }
        }
        public void Died()
        {
            _Alive = false;
            _NavMeshAgent.SetDestination(_NavMeshAgent.transform.position);
        }

        IEnumerator Waiting()
        {
            int wait_time = Random.Range(0, 3);
            yield return new WaitForSeconds(wait_time);
        }

    }
}