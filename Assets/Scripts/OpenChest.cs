using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class OpenChest : Interactable
    {
        Animator animator;
        [SerializeField]
        Transform playerStandingPosition;
        public GameObject itemSpawner;
        public ConsumableItem itemInChest;
        public ObjectiveManager objectiveManager;
        OpenChest openChest;

        private void Awake()
        {
            openChest = GetComponent<OpenChest>();
            animator = GetComponent <Animator>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            //Rotaate player towards chest
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;
            //Lock Trnsform of player
            playerManager.OpenChestInteraction(playerStandingPosition);

            //open chest lid and animate player
            animator.Play("Chest Open");
            //spawn item in chest
            StartCoroutine(SpawnItemInChest());
            WeaponPickup weaponPickup = itemSpawner.GetComponent<WeaponPickup>();

            if (weaponPickup != null)
            {
                weaponPickup.consumableItem = itemInChest;
            }


        }
        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
        public void AddChestToObjectiveManager()
        {
            objectiveManager.chestsFound++;
        }
    }
}
