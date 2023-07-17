using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyBossDeath : MonoBehaviour
    {
        [SerializeField]GameObject chestSpawn;
        public void SpawnChest()
        {
            chestSpawn.SetActive(true);
        }
    }
}
