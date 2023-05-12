using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{

 public class EnemyAnimatorManager : CharacterAnimatorManager
 {

        EnemyManager enemy;
        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyManager>();
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemy.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = enemy.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemy.enemyRigidbody.velocity = velocity /* * enemyLocomotionManager.moveSpeed*/;

            if (enemy.isRotatingWithRootMotion)
            {
                enemy.transform.rotation *= enemy.animator.deltaRotation;
            }
        }


        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
            
            if (playerStats != null)
            {
                playerStats.AddSouls(enemy.characterStatsManager.soulsAwardedOnDeath);
                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
           
        }

        public void PlayWeaponTrailFX()
        {
            enemy.enemyFXManager.PlayWeaponFX(false);
        }
        public void InstantiataeBossParticleFX()
        {
            BossFxTransform bossFxTransform = GetComponentInChildren<BossFxTransform>();

            GameObject phaseFX = Instantiate(enemy.enemyBossManager.particleFX, bossFxTransform.transform);
        }
    }  
}