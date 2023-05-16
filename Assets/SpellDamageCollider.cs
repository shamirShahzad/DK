using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollded = false;

        CharacterStatsManager spellTarget;
       // Rigidbody rigidbody;
        Vector3 impactNormal;
        private void Start()
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;
            if (muzzleParticles)
            {
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollded)
            {
                spellTarget = collision.transform.GetComponent<CharacterStatsManager>();
                if (spellTarget != null && spellTarget.teamIdNumber !=teamIdNumber)
                {
                    spellTarget.TakeDamage(0,fireDamage,magicDamage,0,0,currentDamageAnimation);
                }
                hasCollded = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 1f);
                Destroy(gameObject, 0.2f);
            }
        }
    }
}
