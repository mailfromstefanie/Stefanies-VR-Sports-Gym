using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace StefanieInVR
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SportsMatchManager : UdonSharpBehaviour
    {
        public const int PHASE_READY = 0;
        public const int PHASE_PLAYING = 1;
        public const int PHASE_GOAL_PAUSE = 2;
        public const int PHASE_SUDDEN_DEATH = 3;
        public const int PHASE_FINISHED = 4;

        public const int TEAM_NONE = 0;
        public const int TEAM_RED = 1;
        public const int TEAM_BLUE = 2;

        public const int ANNOUNCEMENT_NONE = 0;

        public const int EMPTY_PLAYER_SLOT = -1;
        public const int MAX_PLAYERS_PER_TEAM = 16;

        [Header("First Release Defaults")]
        [Tooltip("De standaard wedstrijdduur in seconden.")]
        [Min(60)]
        [SerializeField]
        private int defaultMatchDurationSeconds = 600;

        [Header("Shared Match Snapshot - Read Only In This Step")]
        [UdonSynced]
        [SerializeField]
        private int matchPhase = PHASE_READY;

        [UdonSynced]
        [SerializeField]
        private int redScore;

        [UdonSynced]
        [SerializeField]
        private int blueScore;

        [UdonSynced]
        [SerializeField]
        private int configuredDurationSeconds = 600;

        [UdonSynced]
        [SerializeField]
        private int networkEndTimeMilliseconds;

        [UdonSynced]
        [SerializeField]
        private bool teamSwitchingOpen = true;

        [UdonSynced]
        [SerializeField]
        private int winnerTeam = TEAM_NONE;

        [UdonSynced]
        private int[] redPlayerIds = new int[MAX_PLAYERS_PER_TEAM];

        [UdonSynced]
        private int[] bluePlayerIds = new int[MAX_PLAYERS_PER_TEAM];

        [UdonSynced]
        [SerializeField]
        private int persistentAnnouncement = ANNOUNCEMENT_NONE;

        [UdonSynced]
        [SerializeField]
        private int announcementSequence;

        [UdonSynced]
        [SerializeField]
        private int goalPauseEndTimeMilliseconds;

        private bool initialized;

        private void Start()
        {
            if (Networking.IsOwner(gameObject))
            {
                InitializeOwnerSnapshot();
                RequestSerialization();
            }

            initialized = true;
        }

        public override void OnDeserialization()
        {
            initialized = true;
        }

        private void InitializeOwnerSnapshot()
        {
            matchPhase = PHASE_READY;
            redScore = 0;
            blueScore = 0;
            configuredDurationSeconds =
                Mathf.Max(60, defaultMatchDurationSeconds);
            networkEndTimeMilliseconds = 0;
            teamSwitchingOpen = true;
            winnerTeam = TEAM_NONE;
            persistentAnnouncement = ANNOUNCEMENT_NONE;
            announcementSequence = 0;
            goalPauseEndTimeMilliseconds = 0;

            FillPlayerSlots(redPlayerIds);
            FillPlayerSlots(bluePlayerIds);
        }

        private void FillPlayerSlots(int[] playerIds)
        {
            if (playerIds == null)
            {
                return;
            }

            for (int i = 0; i < playerIds.Length; i++)
            {
                playerIds[i] = EMPTY_PLAYER_SLOT;
            }
        }

        public bool IsInitialized()
        {
            return initialized;
        }

        public int GetMatchPhase()
        {
            return matchPhase;
        }

        public int GetRedScore()
        {
            return redScore;
        }

        public int GetBlueScore()
        {
            return blueScore;
        }

        public int GetConfiguredDurationSeconds()
        {
            return configuredDurationSeconds;
        }

        public int GetNetworkEndTimeMilliseconds()
        {
            return networkEndTimeMilliseconds;
        }

        public bool IsTeamSwitchingOpen()
        {
            return teamSwitchingOpen;
        }

        public int GetWinnerTeam()
        {
            return winnerTeam;
        }

        public int[] GetRedPlayerIds()
        {
            return redPlayerIds;
        }

        public int[] GetBluePlayerIds()
        {
            return bluePlayerIds;
        }

        public int GetPersistentAnnouncement()
        {
            return persistentAnnouncement;
        }

        public int GetAnnouncementSequence()
        {
            return announcementSequence;
        }

        public int GetGoalPauseEndTimeMilliseconds()
        {
            return goalPauseEndTimeMilliseconds;
        }
    }
}