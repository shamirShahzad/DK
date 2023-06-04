using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Level/Level Object")]
    public class LevelObject : ScriptableObject
    {
        public bool isLocked;
        public bool isCompleted;
        public int numStars;
        public int levelNumber;
        public Object levelScene;
    }
}
