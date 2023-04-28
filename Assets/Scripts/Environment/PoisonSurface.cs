using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class PoisonSurface : MonoBehaviour
    {
        public float poisonBuildupAmount = 7;

        public List<CharacterFXManager> charactersInPoisonSurface;

        private void OnTriggerEnter(Collider other)
        {
            CharacterFXManager character = other.GetComponentInParent<CharacterFXManager>();

            if (character != null)
            {
                charactersInPoisonSurface.Add(character);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            CharacterFXManager character = other.GetComponentInParent<CharacterFXManager>();
            if (character != null)
            {
                charactersInPoisonSurface.Remove(character);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            foreach(CharacterFXManager character in charactersInPoisonSurface)
            {
                if (character.isPoisned)
                    continue;
                character.poisonBuildup = character.poisonBuildup + poisonBuildupAmount * Time.deltaTime;
            }
        }
    }
}
