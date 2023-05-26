using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DK
{
    public class ItemPopulationInShop : MonoBehaviour
    {
        [SerializeField] Transform contentTransform;
        [SerializeField] GameObject shopItems;
        private GameObject uiItem;
        [SerializeField]
        List<HelmetEquipment> helmetEquipmentList = new List<HelmetEquipment>();
        [SerializeField]
        List<HandEquipment> armsEquipmentList = new List<HandEquipment>();
        [SerializeField]
        List<TorsoEquipment> torsoEquipmentList = new List<TorsoEquipment>();
        [SerializeField]
        List<LegEquipment> legEquipmentList = new List<LegEquipment>();

        [SerializeField] GameObject[] lineFocuses = new GameObject[4];

        [SerializeField] GameObject sucessPopup;
        [SerializeField] GameObject warningPopup;

        

        private void OnEnable()
        {
            onHelmetClick();
            lineFocuses[0].SetActive(true);
        }
        public void onHelmetClick()
        {
            DisableAllFocusLines();
            DestroyAnyOtherChildPresentInContentObject();
            List<HelmetEquipment> notPurchasedHelmets = new List<HelmetEquipment>();
            for (int i = 0; i < helmetEquipmentList.Count; i++)
            {
                if (!helmetEquipmentList[i].isPurchased)
                {
                    notPurchasedHelmets.Add(helmetEquipmentList[i]);
                }
            }
            if (notPurchasedHelmets.Count > 0)
            {
                for (int i = 0; i < notPurchasedHelmets.Count; i++)
                {
                    uiItem = Instantiate(shopItems);

                    //The prefab has a script that takes a item as argument and displays its values when butto is pressed
                    uiItem.GetComponent<PrefabButtonAccessScript>().equipment = notPurchasedHelmets[i] as EquipmentItem;
                    uiItem.GetComponent<PrefabButtonAccessScript>().sucessPopup = sucessPopup;
                    uiItem.GetComponent<PrefabButtonAccessScript>().warningPopup = warningPopup;
                    //put player gold amount here same as above for all on click events

                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedHelmets[i].itemIcon;
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void onArmsClick()
        {
            
            DisableAllFocusLines();
            DestroyAnyOtherChildPresentInContentObject();
            List<HandEquipment> notPurchasedArms = new List<HandEquipment>();
            for (int i = 0; i < armsEquipmentList.Count; i++)
            {
                if (!armsEquipmentList[i].isPurchased)
                {
                    notPurchasedArms.Add(armsEquipmentList[i]);
                }
            }
            if (notPurchasedArms.Count > 0)
            {
                for (int i = 0; i < notPurchasedArms.Count; i++)
                {
                    uiItem = Instantiate(shopItems);

                    //The prefab has a script that takes a item as argument and displays its values when butto is pressed
                    uiItem.GetComponent<PrefabButtonAccessScript>().equipment = notPurchasedArms[i] as EquipmentItem;
                    uiItem.GetComponent<PrefabButtonAccessScript>().sucessPopup = sucessPopup;
                    uiItem.GetComponent<PrefabButtonAccessScript>().warningPopup = warningPopup;
                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedArms[i].itemIcon;
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void onTorsoClick()
        {
            DisableAllFocusLines();
            DestroyAnyOtherChildPresentInContentObject();
            List<TorsoEquipment> notPurchasedTorso = new List<TorsoEquipment>();
            for (int i = 0; i < torsoEquipmentList.Count; i++)
            {
                if (!torsoEquipmentList[i].isPurchased)
                {
                    notPurchasedTorso.Add(torsoEquipmentList[i]);
                }
            }
            if (notPurchasedTorso.Count > 0)
            {
                for (int i = 0; i < notPurchasedTorso.Count; i++)
                {
                    uiItem = Instantiate(shopItems);

                    //The prefab has a script that takes a item as argument and displays its values when butto is pressed
                    uiItem.GetComponent<PrefabButtonAccessScript>().equipment = notPurchasedTorso[i] as EquipmentItem;
                    uiItem.GetComponent<PrefabButtonAccessScript>().sucessPopup = sucessPopup;
                    uiItem.GetComponent<PrefabButtonAccessScript>().warningPopup = warningPopup;
                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedTorso[i].itemIcon;
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void onLegsClick()
        {
            DisableAllFocusLines();
            DestroyAnyOtherChildPresentInContentObject();
            List<LegEquipment> notPurchasedLegs = new List<LegEquipment>();
            for (int i = 0; i < legEquipmentList.Count; i++)
            {
                if (!legEquipmentList[i].isPurchased)
                {
                    notPurchasedLegs.Add(legEquipmentList[i]);
                }
            }
            if (notPurchasedLegs.Count > 0)
            {
                for (int i = 0; i < notPurchasedLegs.Count; i++)
                {
                    uiItem = Instantiate(shopItems);

                    //The prefab has a script that takes a item as argument and displays its values when butto is pressed
                    uiItem.GetComponent<PrefabButtonAccessScript>().equipment = notPurchasedLegs[i] as EquipmentItem;
                    uiItem.GetComponent<PrefabButtonAccessScript>().sucessPopup = sucessPopup;
                    uiItem.GetComponent<PrefabButtonAccessScript>().warningPopup = warningPopup;
                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedLegs[i].itemIcon;
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }
        private void DestroyAnyOtherChildPresentInContentObject()
        {
            if (contentTransform.childCount != 0)
            {
                for (int i = contentTransform.childCount-1; i>=0; i--)
                {
                    Destroy(contentTransform.GetChild(i).gameObject);
                }
                
            }
        }

        private void DisableAllFocusLines()
        {
            foreach(GameObject lineFocus in lineFocuses)
            {
                lineFocus.SetActive(false);
            }
        }

    }
}
