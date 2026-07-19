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

        [Header("Temporary Debug - Read Only")]
        [SerializeField]
        private int debugLocalPlayerTeam = TEAM_NONE;

        [SerializeField]
        private int debugRedPlayerCount;

        [SerializeField]
        private int debugBluePlayerCount;

        private void Start()
        {
            if (Networking.IsOwner(gameObject))
            {
                InitializeOwnerSnapshot();
                RequestSerialization();
            }

            initialized = true;
            RefreshDebugValues();
        }

        public override void OnDeserialization()
        {
            initialized = true;
            RefreshDebugValues();
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (player == null || !Networking.IsOwner(gameObject))
            {
                return;
            }

            int playerId = player.playerId;
            bool removed = RemovePlayerId(redPlayerIds, playerId);
            removed = RemovePlayerId(bluePlayerIds, playerId) || removed;

            if (removed)
            {
                RefreshDebugValues();
                RequestSerialization();
            }
        }

        public void RequestJoinRed()
        {
            RequestJoinTeam(TEAM_RED);
        }

        public void RequestJoinBlue()
        {
            RequestJoinTeam(TEAM_BLUE);
        }

        public void RequestLeaveGame()
        {
            VRCPlayerApi localPlayer = Networking.LocalPlayer;
            if (!IsValidPlayer(localPlayer))
            {
                return;
            }

            int playerId = localPlayer.playerId;
            if (GetPlayerTeam(playerId) == TEAM_NONE)
            {
                return;
            }

            if (!TakeLocalOwnership(localPlayer))
            {
                return;
            }

            bool removed = RemovePlayerId(redPlayerIds, playerId);
            removed = RemovePlayerId(bluePlayerIds, playerId) || removed;

            if (removed)
            {
                RefreshDebugValues();
                RequestSerialization();
            }
        }

        private void RequestJoinTeam(int targetTeam)
        {
            if (targetTeam != TEAM_RED && targetTeam != TEAM_BLUE)
            {
                return;
            }

            if (!teamSwitchingOpen)
            {
                return;
            }

            VRCPlayerApi localPlayer = Networking.LocalPlayer;
            if (!IsValidPlayer(localPlayer))
            {
                return;
            }

            int playerId = localPlayer.playerId;
            if (GetPlayerTeam(playerId) == targetTeam)
            {
                return;
            }

            int[] targetIds = targetTeam == TEAM_RED
                ? redPlayerIds
                : bluePlayerIds;

            if (FindEmptySlot(targetIds) < 0)
            {
                return;
            }

            if (!TakeLocalOwnership(localPlayer))
            {
                return;
            }

            RemovePlayerId(redPlayerIds, playerId);
            RemovePlayerId(bluePlayerIds, playerId);

            int emptySlot = FindEmptySlot(targetIds);
            if (emptySlot < 0)
            {
                return;
            }

            targetIds[emptySlot] = playerId;
            RefreshDebugValues();
            RequestSerialization();
        }

        private bool TakeLocalOwnership(VRCPlayerApi localPlayer)
        {
            if (Networking.IsOwner(gameObject))
            {
                return true;
            }

            Networking.SetOwner(localPlayer, gameObject);
            return Networking.IsOwner(gameObject);
        }

        private bool IsValidPlayer(VRCPlayerApi player)
        {
            return player != null && player.IsValid();
        }

        private int FindEmptySlot(int[] playerIds)
        {
            if (playerIds == null)
            {
                return -1;
            }

            for (int i = 0; i < playerIds.Length; i++)
            {
                if (playerIds[i] == EMPTY_PLAYER_SLOT)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool RemovePlayerId(int[] playerIds, int playerId)
        {
            if (playerIds == null)
            {
                return false;
            }

            bool removed = false;

            for (int i = 0; i < playerIds.Length; i++)
            {
                if (playerIds[i] == playerId)
                {
                    playerIds[i] = EMPTY_PLAYER_SLOT;
                    removed = true;
                }
            }

            return removed;
        }

        private void RefreshDebugValues()
        {
            debugRedPlayerCount = CountPlayers(redPlayerIds);
            debugBluePlayerCount = CountPlayers(bluePlayerIds);

            VRCPlayerApi localPlayer = Networking.LocalPlayer;
            debugLocalPlayerTeam = IsValidPlayer(localPlayer)
                ? GetPlayerTeam(localPlayer.playerId)
                : TEAM_NONE;
        }

        private int CountPlayers(int[] playerIds)
        {
            if (playerIds == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < playerIds.Length; i++)
            {
                if (playerIds[i] != EMPTY_PLAYER_SLOT)
                {
                    count++;
                }
            }

            return count;
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

        public int GetPlayerTeam(int playerId)
        {
            if (ContainsPlayerId(redPlayerIds, playerId))
            {
                return TEAM_RED;
            }

            if (ContainsPlayerId(bluePlayerIds, playerId))
            {
                return TEAM_BLUE;
            }

            return TEAM_NONE;
        }

        private bool ContainsPlayerId(int[] playerIds, int playerId)
        {
            if (playerIds == null)
            {
                return false;
            }

            for (int i = 0; i < playerIds.Length; i++)
            {
                if (playerIds[i] == playerId)
                {
                    return true;
                }
            }

            return false;
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
