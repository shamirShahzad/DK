using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class LowerArmRightModelChanger : MonoBehaviour
    {

        public List<GameObject> lowerRightArmModels;

        private void Awake()
        {
            GetAllLowerRightArmModels();
        }

        private void GetAllLowerRightArmModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                lowerRightArmModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllLowerRightArmModels()
        {
            foreach (GameObject item in lowerRightArmModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipLowerRightArmModelByName(string lowerRightArmName)
        {
            for (int i = 0; i < lowerRightArmModels.Count; i++)
            {
                if (lowerRightArmModels[i].name == lowerRightArmName)
                {
                    lowerRightArmModels[i].SetActive(true);
                }
            }
        }
    }
}
