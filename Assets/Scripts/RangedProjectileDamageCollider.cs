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
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyManager != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber)
                        return;
                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager, shield, enemyStats);

                }
                if (enemyStats != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber || enemyStats.isDead)
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
                        enemyStats.TakeDamageNoAnimation(weaponDamage);
                    }
                    else
                    {
                        if (gameObject.tag == "Skeleton Sword")
                        {
                            enemyStats.TakeDamage(weaponDamage, "Skeleton Hit");
                        }
                        enemyStats.TakeDamage(weaponDamage, currentDamageAnimation);
                    }
                }
            }
            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallhasBeenHit = true;
            }
        }
    }
}
