using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class RightHandModelChanger : MonoBehaviour
    {
        public List<GameObject> rightHandModels;

        private void Awake()
        {
            GetAllRightHandModels();
        }

        private void GetAllRightHandModels()
        {
            int childrenGameObjects = transform.childCount;

            for(int i = 0; i < childrenGameObjects; i++)
            {
                rightHandModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void  UnequipAllRightHandModels()
        {
            foreach(GameObject item in rightHandModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipRightHandModelByName(string rightHandName)
        {
            for(int i  = 0; i < rightHandModels.Count; i++)
            {
                    if(rightHandModels[i].name == rightHandName)
                {
                    rightHandModels[i].SetActive(true);
                }
            }
        }
    }
}
