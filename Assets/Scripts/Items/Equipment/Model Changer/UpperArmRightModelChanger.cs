using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class UpperArmRightModelChanger : MonoBehaviour
    {
        public List<GameObject> upperRightArmModels;

        private void Awake()
        {
            GetAllUpperRightArmModels();
        }

        private void GetAllUpperRightArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                upperRightArmModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllUpperRightArmModels()
        {
            foreach (GameObject item in upperRightArmModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipUpperRightArmModelByName(string upperRightArmName)
        {
            for (int i = 0; i < upperRightArmModels.Count; i++)
            {
                if (upperRightArmModels[i].name == upperRightArmName)
                {
                    upperRightArmModels[i].SetActive(true);
                }
            }
        }
    }
}
