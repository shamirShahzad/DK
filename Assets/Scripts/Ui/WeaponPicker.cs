using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace DK
{
    public class WeaponPicker : MonoBehaviour
    {
        public WeaponItem []weaponItemsRight;
        public WeaponItem[] weaponItemsLeft;

        public Image weaponSpriteLeft;
        public Image weaponSpriteRight;

        private void Awake()
        {
            weaponSpriteLeft.sprite = weaponItemsLeft[PlayerPrefs.GetInt("SelectedWeaponIndexLeft", 0)].itemIcon;
            weaponSpriteRight.sprite = weaponItemsRight[PlayerPrefs.GetInt("SelectedWeaponIndexRight", 0)].itemIcon;
        }

        public void LeftClicked()
        {
            PlayerPrefs.SetInt("LeftHandItem", 1);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            

        }

        public void RightClicked()
        {
            PlayerPrefs.SetInt("LeftHandItem", 0);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            

        }
        public void StartGame()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}
