using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class LowerArmLeftModelChanger : MonoBehaviour
    {
        public List<GameObject> lowerLeftArmModels;

        private void Awake()
        {
            GetAllLowerLeftArmModels();
        }

        private void GetAllLowerLeftArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                lowerLeftArmModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllLowerLeftArmModels()
        {
            foreach (GameObject item in lowerLeftArmModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipLowerLeftArmModelByName(string lowerLeftArmName)
        {
            for (int i = 0; i < lowerLeftArmModels.Count; i++)
            {
                if (lowerLeftArmModels[i].name == lowerLeftArmName)
                {
                    lowerLeftArmModels[i].SetActive(true);
                }
            }
        }
    }
}
