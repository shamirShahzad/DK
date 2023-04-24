using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class WeaponVFX : MonoBehaviour
    {
        [Header("Weapon VFX")]
        public ParticleSystem normalTrail;


        public void PlayWeaponFX()
        {
            normalTrail.Stop();

            if (normalTrail.isStopped)
            {
                normalTrail.Play();
            }
        }
    }
}
