using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD")]
        public GameObject aimCrosshair;

        public SoulCountBar soulCountBar;
        private void Awake()
        {
            soulCountBar = FindObjectOfType<SoulCountBar>();
            soulCountBar.SetSoulCountText(FirebaseManager.instance.userData.soulPlayersPosseses);
        }
    }
}
