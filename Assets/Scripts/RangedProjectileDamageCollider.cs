using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class RangedProjectileDamageCollider : DamageCollider
    {

        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPenetrated;

        Rigidbody arrowRigidBody;
        CapsuleCollider arrowCollider;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.enabled = true;
            arrowCollider = GetComponent<CapsuleCollider>();
            arrowRigidBody = GetComponent<Rigidbody>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;
            CharacterManager enemy = collision.gameObject.GetComponentInParent<CharacterManager>();


            if (enemy != null)
            {
                if (enemy.characterStatsManager.teamIdNumber == teamIdNumber || enemy.isDead)
                    return;
                CheckForParry(enemy);
                CheckForBlock(enemy);
                if (hasBeenParried)
                    return;
                if (shieldHasBeenHit)
                    return;
                enemy.characterStatsManager.poiseResetTimer = enemy.characterStatsManager.totalPoiseResetTime;
                enemy.characterStatsManager.totalPoiseDefense -= poiseBreak;
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemy.transform.forward, Vector3.up));
                ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                enemy.characterFXManager.PlayBloodSplatterEffect(contactPoint);
                if (enemy.characterStatsManager.totalPoiseDefense > poiseBreak)
                {
                    enemy.characterStatsManager.TakeDamageNoAnimation(physicalDamage, fireDamage, magicDamage, lightningDamage, darkDamage);
                }
                else
                {

                    enemy.characterStatsManager.TakeDamage(physicalDamage, fireDamage, magicDamage, lightningDamage, darkDamage, currentDamageAnimation, characterManager);
                }
            }
            
            if (collision.gameObject.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.gameObject.GetComponent<IllusionaryWall>();

                illusionaryWall.wallhasBeenHit = true;
            }
            if (!hasAlreadyPenetrated )
            {
                hasAlreadyPenetrated = true;
                arrowRigidBody.isKinematic = true;
                arrowCollider.enabled = false;
                gameObject.transform.position = collision.GetContact(0).point;
                gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
                gameObject.transform.parent = collision.collider.transform;

            }
            else
            {
                Destroy(this.gameObject, 20);
            }
            
        }

        private void FixedUpdate()
        {
            if (arrowRigidBody.velocity != Vector3.zero)
            {
                arrowRigidBody.rotation = Quaternion.LookRotation(arrowRigidBody.velocity);
            }
        }
    }
}
