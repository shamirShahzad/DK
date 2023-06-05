using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class ObjectiveManager : MonoBehaviour
    {
        public int enemiesKilled;
        public int chestsFound;


        public void SetEnemiesKilled(int killed)
        {
            enemiesKilled = killed;
        }

    }
}
