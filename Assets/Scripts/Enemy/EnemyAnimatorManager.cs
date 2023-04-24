using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{

 public class EnemyAnimatorManager : AnimatorManager
 {
        EnemyBossManager enemyBossManager;
        EnemyFXManager enemyFXManager;
        EnemyManager enemyManager;
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyManager = GetComponent<EnemyManager>();
            enemyFXManager = GetComponent<EnemyFXManager>();
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity /* * enemyLocomotionManager.moveSpeed*/;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= animator.deltaRotation;
            }
        }


        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
            
            if (playerStats != null)
            {
                playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);
                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
           
        }

        public void PlayWeaponTrailFX()
        {
            enemyFXManager.PlayWeaponFX(false);
        }
        public void InstantiataeBossParticleFX()
        {
            BossFxTransform bossFxTransform = GetComponentInChildren<BossFxTransform>();

            GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFxTransform.transform);
        }
    }  
}