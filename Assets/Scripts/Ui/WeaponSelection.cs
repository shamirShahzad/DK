using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DK
{
    public class WeaponSelection : MonoBehaviour
    {
        [SerializeField]
        Transform weaponHolderUi;
        [SerializeField]
        int weaponItemIndexLeft = 0;
        int weaponItemIndexRight = 0;
        GameObject currentWeapon;
        public WeaponItem[] weaponItemsRight;
        public WeaponItem[] weaponItemsLeft;
        GameObject displayWeapon;
        public GameObject hand;

        int isLeft;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            WeaponScroller(0);
            isLeft = PlayerPrefs.GetInt("LeftHandItem");
        }

        private void RenderWeapon(int index,WeaponItem[] items)
        {
            if (index == 0)
            {
                currentWeapon = hand;
                currentWeapon.transform.localScale = new Vector3(10, 10, 10);
                displayWeapon = Instantiate(currentWeapon, weaponHolderUi.transform);
            }
            else
            {
                currentWeapon = items[index].modelPrefab;
                currentWeapon.transform.localScale = new Vector3(5, 5, 5);
                displayWeapon = Instantiate(currentWeapon, weaponHolderUi.transform);
            }
        }

        public void WeaponScroller(int change)
        {
            if(isLeft== 1)
            {
                weaponItemIndexLeft += change;

                if (weaponItemIndexLeft < 0) weaponItemIndexLeft = weaponItemsLeft.Length - 1;
                else if (weaponItemIndexLeft > weaponItemsLeft.Length - 1) weaponItemIndexLeft = 0;

                Destroy(displayWeapon);
                RenderWeapon(weaponItemIndexLeft,weaponItemsLeft);
            }
            else
            {
                weaponItemIndexRight += change;

                if (weaponItemIndexRight < 0) weaponItemIndexRight = weaponItemsRight.Length - 1;
                else if (weaponItemIndexRight > weaponItemsRight.Length - 1) weaponItemIndexRight = 0;

                Destroy(displayWeapon);
                RenderWeapon(weaponItemIndexRight,weaponItemsRight);
            }
            

        }
        public void Selection()
        {
            if (isLeft == 1)
            {
                PlayerPrefs.SetInt("SelectedWeaponIndexLeft", weaponItemIndexLeft);
            }

            else
            {
                PlayerPrefs.SetInt("SelectedWeaponIndexRight", weaponItemIndexRight);
            }
           
            SceneManager.LoadScene(0);
        }

    }
}
