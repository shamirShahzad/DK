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
        protected override void OnAnimatorMove()
        {
            
            Vector3 velocity = enemy.animator.deltaPosition;
            enemy.characterController.Move(velocity);
            
            if (enemy.isRotatingWithRootMotion)
            {
                enemy.transform.rotation *= enemy.animator.deltaRotation;
            }
            
        }


        public void AwardSoulsOnDeath()
        {
            PlayerManager player = FindObjectOfType<PlayerManager>();
            if (player != null)
            {

                player.playerStatsManager.AddSouls(enemy.characterStatsManager.soulsAwardedOnDeath);
                if (player.uIManager.soulCountBar != null)
                {
                    player.uIManager.soulCountBar.SetSoulCountText(player.playerStatsManager.soulCount);
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