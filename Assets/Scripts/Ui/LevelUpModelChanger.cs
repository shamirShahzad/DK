using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK {
    public class LevelUpModelChanger : MonoBehaviour
    {
        public PlayerManager player;
        private void OnEnable()
        {
            player.playerEquipmentManager.EquipAllEquipmentItemsOnStart();
        }
    }
}
