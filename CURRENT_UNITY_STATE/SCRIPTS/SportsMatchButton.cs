using UdonSharp;
using UnityEngine;

namespace StefanieInVR
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SportsMatchButton : UdonSharpBehaviour
    {
        public const int ACTION_JOIN_RED = 0;
        public const int ACTION_JOIN_BLUE = 1;
        public const int ACTION_LEAVE_GAME = 2;

        [Header("Manager")]
        [Tooltip("De centrale SportsMatchManager.")]
        public SportsMatchManager sportsMatchManager;

        [Header("Action")]
        [Tooltip("0 = Join Red, 1 = Join Blue, 2 = Leave Game")]
        [Range(0, 2)]
        public int action = ACTION_JOIN_RED;

        public override void Interact()
        {
            OnButtonPressed();
        }

        public void OnButtonPressed()
        {
            if (sportsMatchManager == null)
            {
                return;
            }

            if (action == ACTION_JOIN_RED)
            {
                sportsMatchManager.RequestJoinRed();
                return;
            }

            if (action == ACTION_JOIN_BLUE)
            {
                sportsMatchManager.RequestJoinBlue();
                return;
            }

            if (action == ACTION_LEAVE_GAME)
            {
                sportsMatchManager.RequestLeaveGame();
            }
        }
    }
}
