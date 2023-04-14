using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class FoggWall : MonoBehaviour
    {
        private void Awake()
        {
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
