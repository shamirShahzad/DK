using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    [CreateAssetMenu(menuName = "RewardType")]
    public class RewardsObject :Item
    {
        public RewardType rewardType;
        public int amount;
    }
}
