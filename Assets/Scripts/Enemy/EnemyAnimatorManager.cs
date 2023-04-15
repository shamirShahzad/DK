using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{

 public class EnemyAnimatorManager : AnimatorManager
 {
        EnemyManager enemyManager;
        EnemyBossManager enemyBossManager;
        EnemyStats enemyStats;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
            enemyBossManager = GetComponentInParent<EnemyBossManager>();
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity /* * enemyLocomotionManager.moveSpeed*/;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= anim.deltaRotation;
            }
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
            enemyManager.pendingCriticalDamage = 0;
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
            
            if (playerStats != null)
            {
                playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);
                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
           
        }

        public void EnableIsParrying()
        {
            enemyManager.isParrying = true;
        }
        public void DisableIsParrying()
        {
            enemyManager.isParrying = false;
        }

        public void EnableCanBeRiposted()
        {
            enemyManager.canBeRiposted = true;
        }
        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }
        public void CanRotate()
        {
            anim.SetBool("canRotate", true);

        }

        public void StopRotate()
        {
            anim.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }
        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }

        public void InstantiataeBossParticleFX()
        {
            BossFxTransform bossFxTransform = GetComponentInChildren<BossFxTransform>();

            GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFxTransform.transform);
        }
    }  
}