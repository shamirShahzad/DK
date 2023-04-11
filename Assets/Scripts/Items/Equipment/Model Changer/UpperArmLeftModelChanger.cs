using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class UpperArmLeftModelChanger : MonoBehaviour
    {

        public List<GameObject> upperLeftArmModels;

        private void Awake()
        {
            GetAllUpperLeftArmModels();
        }

        private void GetAllUpperLeftArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                upperLeftArmModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllUpperLeftArmModels()
        {
            foreach (GameObject item in upperLeftArmModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipUpperLeftArmModelByName(string upperLeftArmName)
        {
            for (int i = 0; i < upperLeftArmModels.Count; i++)
            {
                if (upperLeftArmModels[i].name == upperLeftArmName)
                {
                    upperLeftArmModels[i].SetActive(true);
                }
            }
        }
    }
}
