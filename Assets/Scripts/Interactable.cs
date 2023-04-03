using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        float radius = 0.6f;
        public string interactableText;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("You Interacted with object");
        }
    }
}
