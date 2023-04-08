using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class DestroyAfterCastingSpell : MonoBehaviour
    {
        public CharacterManager characterCastingSpell;

        private void Awake()
        {
            characterCastingSpell = GetComponentInParent<CharacterManager>();
        }

        void Update()
        {
            if(characterCastingSpell.isFiringSpell)
            {
                Destroy(gameObject);
            }
        }
    }
}