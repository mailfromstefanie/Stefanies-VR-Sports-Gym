using UdonSharp;
using UnityEngine;

namespace StefanieInVR
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SportsMatchButton : UdonSharpBehaviour
    {
        public enum MatchAction
        {
            JoinRed,
            JoinBlue,
            LeaveGame
        }

        [Header("Manager")]
        [Tooltip("De centrale SportsMatchManager.")]
        public SportsMatchManager sportsMatchManager;

        [Header("Action")]
        [Tooltip("Kies welke actie deze knop naar de manager stuurt.")]
        public MatchAction action = MatchAction.JoinRed;

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

            if (action == MatchAction.JoinRed)
            {
                sportsMatchManager.RequestJoinRed();
                return;
            }

            if (action == MatchAction.JoinBlue)
            {
                sportsMatchManager.RequestJoinBlue();
                return;
            }

            if (action == MatchAction.LeaveGame)
            {
                sportsMatchManager.RequestLeaveGame();
            }
        }
    }
}
