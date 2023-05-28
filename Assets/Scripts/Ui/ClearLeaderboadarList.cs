
using UnityEngine;
namespace DK {
    public class ClearLeaderboadarList : MonoBehaviour
    {
        private void OnDisable()
        {
            FirebaseManager.instance.leaderBoardList.Clear();
        }
    }
}
