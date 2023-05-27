using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DK
{
    public class ItemPopulationInShop : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI goldAmountText;
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
        [SerializeField] GameObject loadingPopup;

        [Header("Flags For Checking which item is picked")]
        public bool isHelmet;
        public bool isArms;
        public bool isTorso;
        public bool isLegs;
        public bool isLeft;
        public bool isRight;



        private void OnEnable()
        {
            goldAmountText.text = FirebaseManager.instance.userData.goldAmount.ToString();
            onHelmetClick();
            lineFocuses[0].SetActive(true);
        }
        public void onHelmetClick()
        {
            SetFlagsForEquipment(true, false, false, false, false, false);
            DisableAllFocusLines();
            lineFocuses[0].SetActive(true);
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
                    uiItem.GetComponent<PrefabButtonAccessScript>().shop = this;
                    //put player gold amount here same as above for all on click events

                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedHelmets[i].itemIcon;
                    uiItem.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = notPurchasedHelmets[i].goldRequiredToPurchase.ToString();
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void onArmsClick()
        {
            SetFlagsForEquipment(false, true, false, false, false, false);
            DisableAllFocusLines();
            lineFocuses[2].SetActive(true);
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
                    uiItem.GetComponent<PrefabButtonAccessScript>().shop = this;
                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedArms[i].itemIcon;
                    uiItem.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = notPurchasedArms[i].goldRequiredToPurchase.ToString();
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void onTorsoClick()
        {
            SetFlagsForEquipment(false, false, true, false, false, false);
            DisableAllFocusLines();
            lineFocuses[1].SetActive(true);
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
                    uiItem.GetComponent<PrefabButtonAccessScript>().shop = this;
                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedTorso[i].itemIcon;
                    uiItem.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = notPurchasedTorso[i].goldRequiredToPurchase.ToString();
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void onLegsClick()
        {
            SetFlagsForEquipment(false, false, false, true, false, false);
            DisableAllFocusLines();
            lineFocuses[3].SetActive(true);
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
                    uiItem.GetComponent<PrefabButtonAccessScript>().shop = this;
                    uiItem.transform.GetChild(3).GetChild(1).GetComponent<Image>().sprite = notPurchasedLegs[i].itemIcon;
                    uiItem.transform.GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = notPurchasedLegs[i].goldRequiredToPurchase.ToString();
                    //Do other things with the prefab here

                    uiItem.transform.SetParent(contentTransform);
                    uiItem.transform.localScale = Vector3.one;
                    uiItem.transform.localPosition = Vector3.zero;
                }
            }
        }

        public void SetFlagsForEquipment(bool helmetItem, bool armItem, bool torsoItem, bool legsItem, bool leftItem, bool rightItem)
        {
            isHelmet = helmetItem;
            isArms = armItem;
            isTorso = torsoItem;
            isLegs = legsItem;
            isLeft = leftItem;
            isRight = rightItem;
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

        public void SetGoldAmountOnPurchase()
        {
            goldAmountText.text = FirebaseManager.instance.userData.goldAmount.ToString();
        }

    }
}
