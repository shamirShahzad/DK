using UnityEngine;
using UnityEngine.UI;
namespace DK
{
    public class AdRewardPopup : MonoBehaviour
    {
        bool isGold;
        bool isSouls;
        int amount;
        [SerializeField] Image popupImage;
        [SerializeField] Sprite goldSprite;
        [SerializeField] Sprite soulsSprite;
        private void OnEnable()
        {
            SetRewards();
        }

        private void SetRewards()
        {
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                isGold = true;
                isSouls = false;
                popupImage.sprite = goldSprite;
            }
            else
            {
                isSouls = true;
                isGold = false;
                popupImage.sprite = soulsSprite;
            }

            amount = Random.Range(20,101);
        }

        public void OnCollectClick()
        {
            if (isGold)
            {
                FirebaseManager.instance.UpdateGold(amount);
            }
            else if(isSouls)
            {
                FirebaseManager.instance.UpdateSouls(amount);
            }

            FirebaseManager.instance.GetDataFromDatabase();
            this.gameObject.SetActive(false);
        }
    }
}
