using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class FoggWall : MonoBehaviour
    {
        PassThroughWall passThroughWall;
        private void Awake()
        {
            passThroughWall = GetComponentInChildren<PassThroughWall>();
            passThroughWall.foggBlockColider = GetComponent<BoxCollider>();
            gameObject.SetActive(false);
        }

        public  void ActivateFoggWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFoggWall()
        {
            gameObject.SetActive(false);
        }
    }
}
