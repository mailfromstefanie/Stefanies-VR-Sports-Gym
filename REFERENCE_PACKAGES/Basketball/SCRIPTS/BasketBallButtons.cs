
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

enum BasketBallButtonTypes
{
    RespawnBall,
    JoinGame,
    LeaveGame,
    StartGame,
    IncreaseGameMinutes,
    DecreaseGameMinutes,
    GameMinutesNoLimit,
}

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class BasketBallButtons : UdonSharpBehaviour
{
    [SerializeField] private BasketBallButtonTypes buttonType = BasketBallButtonTypes.RespawnBall;
    [SerializeField] private BasketBallBall ballSettings = null;
    [SerializeField] private BasketBallGrouping groupSettings = null;
    void Start()
    {
        
    }
    public override void Interact()
    {
        if (buttonType == BasketBallButtonTypes.RespawnBall) {
            if (ballSettings) ballSettings.RespawnBall();
        } else if (buttonType == BasketBallButtonTypes.JoinGame) {
            if (groupSettings) groupSettings.JoinGame();
        } else if (buttonType == BasketBallButtonTypes.LeaveGame) {
            if (groupSettings) groupSettings.LeaveGame();
        } else if (buttonType == BasketBallButtonTypes.StartGame) {
            if (groupSettings) groupSettings.StartGame();
        } else if (buttonType == BasketBallButtonTypes.IncreaseGameMinutes) {
            if (groupSettings) groupSettings.IncreaseGameMinutes();
        } else if (buttonType == BasketBallButtonTypes.DecreaseGameMinutes) {
            if (groupSettings) groupSettings.DecreaseGameMinutes();
        } else if (buttonType == BasketBallButtonTypes.GameMinutesNoLimit) {
            if (groupSettings) groupSettings.SetGameTimeNoLimit();
        }
    }
}
