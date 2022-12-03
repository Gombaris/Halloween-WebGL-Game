using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Halloween
{ 
public class rag : MonoBehaviour
{
   [SerializeField] private BoxCollider _MainCollider;
    [SerializeField] private GameObject _Rig;
    [SerializeField] private Animator _Animator;
    [SerializeField] private EnemyBehaviour _Behaviour;
    Collider[] RagdollCollider;
    Rigidbody[] RagdollRigidbody;
        private Projectile _Projectile;
        private float _DestroyTime, _Health, _MaxHealth = 3f;
        private bool _Count;
       

    void Start()
    {
            _Projectile = GameObject.FindObjectOfType(typeof(Projectile)) as Projectile;
         
            _Health = _MaxHealth;
        GetRagdollBones();
        RagdollOff();
    }
      
    void Update()
    {

            if (_Health <= 0)
            {
                _Count = true;
                RagdollOn();
            }


            if (_Count == true)
            {
                _DestroyTime += Time.deltaTime;
                    Destroy(gameObject, 5);
                    _DestroyTime = 0f;
                    _Count = false;
                
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

        public void TakeDamage(float damageAmount)
        {
            _Health -= damageAmount;

        }
 
    }

}

