using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class WorldEventManager : MonoBehaviour
    {
        public List<FoggWall> foggWalls;
        UiBossHealthBar bossHealthBar;
        EnemyBossManager enemyBossManager;

        public bool bossFightIsActive;
        public bool bossHasBeenAwakened;
        public bool bossHasBeenDefeated;
        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UiBossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetUIHealthBarToActive();
            foreach(var foggWall in foggWalls)
            {
                foggWall.ActivateFoggWall();
            }
        }

        public void BossHasBeenDefeated()
        {
            bossFightIsActive = false;
            bossHasBeenDefeated = true;

            foreach (var foggWall in foggWalls)
            {
                foggWall.DeactivateFoggWall();
            }
        }
    }
}
