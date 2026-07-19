
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class BasketBallGrouping : UdonSharpBehaviour
{
    [SerializeField] private byte maxPlayer = 10;
    [SerializeField] private byte worldCapacity = 32;
    [SerializeField] private byte maxGameMinutes = 60;
    [SerializeField] private float tagOffsetFromHead = 0.4f;
    [SerializeField] private float tagOffsetForGeneric = 1.75f;
    [SerializeField] private string team0Name = "Red Team";
    [SerializeField] private string team1Name = "Blue Team";
    [SerializeField] private Color team0Color = new Color(255f/255f, 50f/255f, 50f/255f, 1f);
    [SerializeField] private Color team1Color = new Color(50f/255f, 50f/255f, 255f/255f, 1f);
    [SerializeField] private string gameMinutesPrefix = "Game Time：";
    [SerializeField] private string gameMinutesSuffix = " minutes";

    [SerializeField] private AudioSource finishSound = null;
    [SerializeField] private TextMeshProUGUI team0TotalScoreText = null;
    [SerializeField] private TextMeshProUGUI team1TotalScoreText = null;
    [SerializeField] private TextMeshProUGUI team0PlayerNameText = null;
    [SerializeField] private TextMeshProUGUI team1PlayerNameText = null;
    [SerializeField] private TextMeshProUGUI unassignedPlayerNameText = null;
    [SerializeField] private TextMeshProUGUI team0PlayerScoreText = null;
    [SerializeField] private TextMeshProUGUI team1PlayerScoreText = null;
    [SerializeField] private TextMeshProUGUI unassignedScoreText = null;
    [SerializeField] private TextMeshProUGUI minutesText = null;
    [SerializeField] private TextMeshProUGUI secondsText = null;
    [SerializeField] private TextMeshProUGUI gameMinutesText = null;
    [SerializeField] private GameObject tagParent = null;

    [UdonSynced] private byte[] ScoresPlus = null;
    [UdonSynced] private byte[] ScoresMinus = null;
    [UdonSynced] private ushort[] PlayerIDs = null;
    
    [UdonSynced, FieldChangeCallback(nameof(StartTime))]
    private string _startTime = "";

    [UdonSynced, FieldChangeCallback(nameof(GameMinutes))]
    private byte _gameMinutes = 10;

    private VRCPlayerApi localPlayer = null;
    private int myPlayerDataIndex = -1;
    private byte[] scoresLocal = null;
    private UnityEngine.UI.Image[] tagImages = null;
    private TextMeshProUGUI[] tagTexts = null;
    private float timeSpan = 0f;
    private System.DateTime startDateTime;

    public string StartTime
    {
        get => _startTime;
        set
        {
            if (value != "") {
                if (GameMinutes != 0) {
                    startDateTime = System.DateTime.ParseExact(value, "yyyy/MM/dd HH:mm:ss.fff", null);
                    startDateTime = startDateTime.AddSeconds(60 * GameMinutes - timeSpan);
                } else {
                    if (minutesText) minutesText.text = "88";
                    if (secondsText) secondsText.text = "88";
                }
            }
            _startTime = value;
        }
    }
    public byte GameMinutes
    {
        get => _gameMinutes;
        set
        {
            _gameMinutes = value;
            if (gameMinutesText) {
                if (value == 0) {
                    gameMinutesText.text = gameMinutesPrefix + "No Limit";
                } else {
                    gameMinutesText.text = gameMinutesPrefix + value + gameMinutesSuffix;
                }
            }
        }
    }

    void Start()
    {
        
    }
    void OnEnable()
    {
        if (tagImages == null) {
            System.DateTime now = System.DateTime.Now;
            System.DateTime exactTime = Networking.GetNetworkDateTime();
            System.DateTime univExactTime = exactTime.ToUniversalTime();
            System.DateTime univNow = now.ToUniversalTime();
            System.TimeSpan ts0 = univExactTime - univNow;
            timeSpan = (float)ts0.TotalSeconds;

            if (worldCapacity > 80) {
                worldCapacity = 80;
            }
            worldCapacity += 2;
            PlayerIDs = new ushort[worldCapacity];
            ScoresPlus = new byte[worldCapacity];
            ScoresMinus = new byte[worldCapacity];
            scoresLocal = new byte[worldCapacity];
            for (int i = 0; i < worldCapacity; i++) {
                ScoresPlus[i] = 0;
                ScoresMinus[i] = 0;
                scoresLocal[i] = 0;
                PlayerIDs[i] = 0xffff;
            }
            localPlayer = Networking.LocalPlayer;
            tagImages = tagParent.GetComponentsInChildren<UnityEngine.UI.Image>(true);
            tagTexts = tagParent.GetComponentsInChildren<TextMeshProUGUI>(true);
            for (int i = 0; i < tagImages.Length; i++) {
                tagImages[i].transform.parent.gameObject.SetActive(false);
            }
            if (gameMinutesText) gameMinutesText.text = gameMinutesPrefix +  GameMinutes + gameMinutesSuffix;
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(RequestSerialization_));
    }
    void Update()
    {
        if (StartTime != "") {
            if (GameMinutes != 0) {
                System.DateTime now = System.DateTime.Now;
                System.DateTime univNow = now.ToUniversalTime();
                System.TimeSpan ts = startDateTime - univNow;
                float time = (float)ts.TotalSeconds;
                if (time < 0) {
                    time = 0f;
                    if (finishSound) finishSound.Play();
                    if (Networking.IsOwner(gameObject)) {
                        StartTime = "";
                        RequestSerialization();
                    }
                }
                if (minutesText) minutesText.text = ((int)(time / 60)).ToString("00");
                if (secondsText) secondsText.text = ((int)(time % 60)).ToString("00");
            }
        }
        if (!tagParent.activeSelf) return;
        if (!Utilities.IsValid(localPlayer)) return;
        if (PlayerIDs == null) return;
        for (int i = 0; i < worldCapacity; i++) {
            VRCPlayerApi tmpPlayer = VRCPlayerApi.GetPlayerById(PlayerIDs[i] & 0x3fff);
            if (PlayerIDs[i] == 0xffff || !Utilities.IsValid(tmpPlayer)) {
                tagImages[i].transform.parent.gameObject.SetActive(false);
                continue;
            }
            Vector3 headPosition = tmpPlayer.GetBonePosition(HumanBodyBones.Head);
            if ((PlayerIDs[i] >> 14) > 1) {
                tagImages[i].transform.parent.gameObject.SetActive(false);
                continue;
            }
            tagImages[i].transform.parent.gameObject.SetActive(true);
            if (headPosition == Vector3.zero) {
                tagImages[i].transform.parent.position = tmpPlayer.GetPosition() + new Vector3(0, tagOffsetForGeneric, 0);
            } else {
                tagImages[i].transform.parent.position = headPosition + new Vector3(0, tagOffsetFromHead, 0);
            }
            if ((PlayerIDs[i] & 0x3fff) == localPlayer.playerId) {
                tagImages[i].transform.parent.rotation = localPlayer.GetRotation();
            } else {
                tagImages[i].transform.parent.rotation = Quaternion.LookRotation(localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position - tagImages[i].transform.position);
            }
            if ((PlayerIDs[i] >> 14) == 0) {
                tagImages[i].color = team0Color;
                tagTexts[i].text = team0Name;
            } else {
                tagImages[i].color = team1Color;
                tagTexts[i].text = team1Name;
            }
        }
    }
    public void JoinGame()
    {
        if (!Utilities.IsValid(localPlayer)) return;
        if (myPlayerDataIndex >= 0) return;
        for (int i = 0; i < worldCapacity; i++) {
            if ((PlayerIDs[i] & 0x3fff) == localPlayer.playerId) {
                myPlayerDataIndex = i;
                return;
            }
        }
        for (int i = 0; i < worldCapacity; i++) {
            if (PlayerIDs[i] == 0xffff) {
                Networking.SetOwner(localPlayer, gameObject);
                PlayerIDs[i] = (ushort)(localPlayer.playerId + 0xc000);
                myPlayerDataIndex = i;
                RequestSerialization();
                break;
            }
        }
    }
    public void StartGame()
    {
        System.DateTime now = System.DateTime.Now;
        System.DateTime univNow = now.ToUniversalTime();
        System.DateTime startDateTime = univNow.AddSeconds(timeSpan);
        Networking.SetOwner(localPlayer, gameObject);
        StartTime = startDateTime.ToString("yyyy/MM/dd HH:mm:ss.fff");
        int playerNum = 0;
        for (int i = 0; i < worldCapacity; i++) {
            ScoresPlus[i] = 0;
            ScoresMinus[i] = 0;
            if (PlayerIDs[i] == 0xffff) continue;
            if (!Utilities.IsValid(VRCPlayerApi.GetPlayerById((PlayerIDs[i] & 0x3fff)))) {
                PlayerIDs[i] = 0xffff;
                continue;
            }
            playerNum++;
        }
        if (playerNum > maxPlayer) playerNum = maxPlayer;
        int[] teamArray = new int[playerNum];
        for (int i = 0; i < playerNum; i++) {
            if (i < playerNum / 2) {
                teamArray[i] = 1;
            } else {
                teamArray[i] = 0;
            }
        }
        Utilities.ShuffleArray(teamArray);
        int index = 0;
        for (int i = 0; i < worldCapacity; i++) {
            if (PlayerIDs[i] == 0xffff) continue;
            if (index >= playerNum) {
                PlayerIDs[i] = (ushort)(PlayerIDs[i] | 0xc000); // set teamID to 3
            } else {
                PlayerIDs[i] = (ushort)((((byte)teamArray[index]) << 14) | (PlayerIDs[i] & 0x3fff)); // set teamID
            }
            index++;
        }
        RequestSerialization();
        UpdateScore();
    }
    public void LeaveGame()
    {
        if (myPlayerDataIndex < 0) return;
        Networking.SetOwner(localPlayer, gameObject);
        PlayerIDs[myPlayerDataIndex] = 0xffff;
        ScoresPlus[myPlayerDataIndex] = 0;
        ScoresMinus[myPlayerDataIndex] = 0;
        myPlayerDataIndex = -1;
        RequestSerialization();
    }
    public void AddScore(int score)
    {
        Networking.SetOwner(localPlayer, gameObject);
        if (myPlayerDataIndex < 0) {
            JoinGame();
        }
        if (score >= 0) {
            ScoresPlus[myPlayerDataIndex] += (byte)score;
        } else {
            ScoresMinus[myPlayerDataIndex] += (byte)(-score);
        }
        RequestSerialization();
        UpdateScore();
    }
    public void UpdateScore()
    {
        // bool isUpdated = false;
        // for (int i = 0;i < worldCapacity; i++) {
        //     if (PlayerIDs[i] == 0xffff) continue;
        //     if (scoresLocal[i] != ScoresPlus[i]) {
        //         scoresLocal[i] = ScoresPlus[i];
        //         isUpdated = true;
        //     }
        // }
        // if (!isUpdated) return;
        int team0ScoreSum = 0;
        int team1ScoreSum = 0;
        for (int i = 0; i < worldCapacity; i++) {
            if (PlayerIDs[i] == 0xffff) continue;
            int teamID = PlayerIDs[i] >> 14;
            if (teamID == 0) {
                team0ScoreSum += ScoresPlus[i];
                team1ScoreSum += ScoresMinus[i];
            } else if (teamID == 1) {
                team1ScoreSum += ScoresPlus[i];
                team0ScoreSum += ScoresMinus[i];
            } else {
                continue;
            }
        }
        string team0PlayerNameString = "";
        string team1PlayerNameString = "";
        string unassignedPlayerNameString = "";
        string team0ScoreString = "";
        string team1ScoreString = "";
        string unassignedScoreString = "";
        for (int i = 0; i < worldCapacity; i++) {
            if (PlayerIDs[i] == 0xffff) continue;
            int teamID = PlayerIDs[i] >> 14;
            if (teamID == 0) {
                if (Utilities.IsValid(VRCPlayerApi.GetPlayerById(PlayerIDs[i] & 0x3fff))) {
                    team0PlayerNameString += VRCPlayerApi.GetPlayerById((PlayerIDs[i] & 0x3fff)).displayName + "\n";
                } else {
                    team0PlayerNameString += "Left Player\n";
                }
                if (ScoresMinus[i] > 0) {
                    team0ScoreString += ScoresPlus[i] + " (-" + ScoresMinus[i] + ")\n";
                } else {
                    team0ScoreString += ScoresPlus[i] + "\n";
                }
            } else if (teamID == 1){
                if (Utilities.IsValid(VRCPlayerApi.GetPlayerById(PlayerIDs[i] & 0x3fff))) {
                    team1PlayerNameString += VRCPlayerApi.GetPlayerById((PlayerIDs[i] & 0x3fff)).displayName + "\n";
                } else {
                    team1PlayerNameString += "Left Player\n";
                }
                if (ScoresMinus[i] > 0) {
                    team1ScoreString += ScoresPlus[i] + " (-" + ScoresMinus[i] + ")\n";
                } else {
                    team1ScoreString += ScoresPlus[i] + "\n";
                }
            } else if (teamID == 3) {
                if (Utilities.IsValid(VRCPlayerApi.GetPlayerById(PlayerIDs[i] & 0x3fff))) {
                    unassignedPlayerNameString += VRCPlayerApi.GetPlayerById((PlayerIDs[i] & 0x3fff)).displayName + "\n";
                } else {
                    unassignedPlayerNameString += "Left Player\n";
                }
                if (ScoresMinus[i] > 0) {
                    unassignedScoreString += ScoresPlus[i] + " (-" + ScoresMinus[i] + ")\n";
                } else {
                    unassignedScoreString += ScoresPlus[i] + "\n";
                }
            }
        }
        if (team0TotalScoreText) team0TotalScoreText.text = "" + team0ScoreSum;
        if (team1TotalScoreText) team1TotalScoreText.text = "" + team1ScoreSum;
        if (team0PlayerNameText) team0PlayerNameText.text = team0PlayerNameString;
        if (team1PlayerNameText) team1PlayerNameText.text = team1PlayerNameString;
        if (unassignedPlayerNameText) unassignedPlayerNameText.text = unassignedPlayerNameString;
        if (team0PlayerScoreText) team0PlayerScoreText.text = team0ScoreString;
        if (team1PlayerScoreText) team1PlayerScoreText.text = team1ScoreString;
        if (unassignedScoreText) unassignedScoreText.text = unassignedScoreString;
    }
    public byte GetTeam(){
        if (myPlayerDataIndex < 0) return 3;
        return (byte)(PlayerIDs[myPlayerDataIndex] >> 14);
    }
    public void IncreaseGameMinutes()
    {
        if (GameMinutes == maxGameMinutes) return;
        Networking.SetOwner(localPlayer, gameObject);
        if (GameMinutes + 5 > maxGameMinutes) {
            GameMinutes = maxGameMinutes;
        } else if (GameMinutes == 1) {
            GameMinutes = 2;
        } else if (GameMinutes == 2 ) {
            GameMinutes = 3;
        } else if (GameMinutes == 3) {
            GameMinutes = 5;
        } else if (GameMinutes == 0) {
            GameMinutes = maxGameMinutes;
        } else {
            GameMinutes = (byte)((GameMinutes / 5) * 5 + 5);
        }
        RequestSerialization();
    }
    public void DecreaseGameMinutes()
    {
        if (GameMinutes == 1) return;
        Networking.SetOwner(localPlayer, gameObject);
        if (GameMinutes == 0) {
            GameMinutes = maxGameMinutes;
        } else if (GameMinutes == 2) {
            GameMinutes = 1;
        } else if (GameMinutes == 3) {
            GameMinutes = 2;
        } else if (GameMinutes == 5) {
            GameMinutes = 3;
        } else {
            GameMinutes = (byte)((GameMinutes / 5) * 5 - 5);
        }
        RequestSerialization();
    }
    public void SetGameTimeNoLimit()
    {
        Networking.SetOwner(localPlayer, gameObject);
        if (GameMinutes == 0) {
            GameMinutes = 10;
        } else {
            GameMinutes = 0;
        }
        RequestSerialization();
    }
    public override void OnDeserialization()
    {
        UpdateScore();
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if(Networking.IsOwner(gameObject)){
            SendCustomEventDelayedSeconds(nameof(RequestSerialization_), 5f);
        }
    }

    public void RequestSerialization_()
    {
        RequestSerialization();
    }
}
