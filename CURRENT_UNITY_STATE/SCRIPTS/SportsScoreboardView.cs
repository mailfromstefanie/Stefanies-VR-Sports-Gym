using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using TMPro;

namespace StefanieInVR
{
    public class SportsScoreboardView : UdonSharpBehaviour
    {
        [Header("References")]
        [SerializeField] private SportsMatchManager matchManager;
        [SerializeField] private TMP_Text redTeamText;
        [SerializeField] private TMP_Text blueTeamText;

        private void Start()
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (matchManager == null)
            {
                return;
            }

            if (redTeamText != null)
            {
                redTeamText.text = BuildPlayerList(matchManager.GetRedPlayerIds());
            }

            if (blueTeamText != null)
            {
                blueTeamText.text = BuildPlayerList(matchManager.GetBluePlayerIds());
            }
        }

        private string BuildPlayerList(int[] playerIds)
        {
            if (playerIds == null)
            {
                return "";
            }

            string playerList = "";

            for (int i = 0; i < playerIds.Length; i++)
            {
                int playerId = playerIds[i];

                if (playerId == SportsMatchManager.EMPTY_PLAYER_SLOT)
                {
                    continue;
                }

                VRCPlayerApi player = VRCPlayerApi.GetPlayerById(playerId);
                if (player == null || !player.IsValid())
                {
                    continue;
                }

                if (playerList != "")
                {
                    playerList += "\n";
                }

                playerList += player.displayName;
            }

            return playerList;
        }
    }
}
