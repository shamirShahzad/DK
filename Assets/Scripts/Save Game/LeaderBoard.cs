using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace DK
{
    [System.Serializable]
    public class LeaderBoard
    {
        public string uid;
        public int rank;
        public string name;
        public int deaths;
        public DateTime timestamp;

        public LeaderBoard(string name,int deaths)
        {
            this.name = name;
            this.deaths = deaths;
        }
        public LeaderBoard()
        {
          
        }
    }
}