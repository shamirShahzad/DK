using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{

    public class LeftHandModelChanger : MonoBehaviour
    {
        public List<GameObject> leftHandModels;

        private void Awake()
        {
            GetAllLeftHandModels();
        }

        private void GetAllLeftHandModels()
        {
            int childrenGameObjects = transform.childCount;

            for (int i = 0; i < childrenGameObjects; i++)
            {
                leftHandModels.Add(transform.GetChild(i).gameObject);
            }
        }

        public void UnequipAllLeftHandModels()
        {
            foreach (GameObject item in leftHandModels)
            {
                item.SetActive(false);
            }
        }

        public void EquipLeftHandModelByName(string leftHandName)
        {
            for (int i = 0; i < leftHandModels.Count; i++)
            {
                if (leftHandModels[i].name == leftHandName)
                {
                    leftHandModels[i].SetActive(true);
                }
            }
        }
    }
}
