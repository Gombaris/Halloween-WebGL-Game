using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Halloween
{

    public class Projectile : MonoBehaviour 
    {
        [SerializeField] private LayerMask _EnemyMask;

        void Start()
        {

        }

        void Update()
        {
        }
         private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.TryGetComponent<Skeleton>(out Skeleton enemyComponent))
            {
                enemyComponent.TakeDamage(1);
            }
            else if (other.gameObject.TryGetComponent<SkeletonWarrior>(out SkeletonWarrior swComponent))
            {
                swComponent.TakeDamage(1);
            }
            else if (other.gameObject.TryGetComponent<Zombie>(out Zombie zombieComponent))
            {
                zombieComponent.TakeDamage(1);
            }
            if (other.gameObject.TryGetComponent<Player>(out Player playerComponent) || other.gameObject.TryGetComponent<Ammo>(out Ammo ammoComponent))
            {

            }
            else 
            {
                Destroy(gameObject);
            }
        }
    }
}