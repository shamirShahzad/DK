using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK {
    public class LevelUpModelChanger : MonoBehaviour
    {
        public PlayerManager player;
        public GameObject levelWindow;
        private void OnEnable()
        {
            player.playerEquipmentManager.EquipAllEquipmentItemsOnStart();
            levelWindow.SetActive(true);
        }
        private void OnDisable()
        {
            levelWindow.SetActive(false);
        }
    }
}
