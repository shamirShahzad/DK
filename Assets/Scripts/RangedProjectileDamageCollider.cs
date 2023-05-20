using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class RangedProjectileDamageCollider : DamageCollider
    {

        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPenetrated;
        protected GameObject penetratedProjectile;
        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                CharacterFXManager enemyFX = collision.GetComponent<CharacterFXManager>();
               

                if (enemyManager != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber)
                        return;
                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager);

                }
                if (enemyStats != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber || enemyManager.isDead)
                        return;
                    if (hasBeenParried)
                        return;
                    if (shieldHasBeenHit)
                        return;
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefense -= poiseBreak;
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                    enemyFX.PlayBloodSplatterEffect(contactPoint);
                    if (enemyStats.totalPoiseDefense > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(physicalDamage,fireDamage,magicDamage,lightningDamage,darkDamage);
                    }
                    else
                    {

                        enemyStats.TakeDamage(physicalDamage,fireDamage,magicDamage,lightningDamage,darkDamage, currentDamageAnimation,characterManager);
                    }
                }
            }
            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallhasBeenHit = true;
            }
            if (!hasAlreadyPenetrated && penetratedProjectile == null)
            {
                hasAlreadyPenetrated = true;
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                GameObject penetratedrrow = Instantiate(ammoItem.penetrateModel, contactPoint, Quaternion.Euler(0, 0, 0));

                penetratedProjectile = penetratedrrow;
                penetratedrrow.transform.parent = collision.transform;
                penetratedrrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);
            }
            Destroy(transform.root.gameObject);
        }
    }
}
