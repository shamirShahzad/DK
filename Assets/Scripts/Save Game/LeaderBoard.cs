using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace DK
{
    [Serializable]
    public class LeaderBoard
    {
        public string uid;
        public int rank;
        public string name;
        public int deaths;
        public object timestamp;

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