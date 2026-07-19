
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class BasketBallBlock : UdonSharpBehaviour
{
    [SerializeField] private BasketBallBall settings = null;
    void Start()
    {
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == Networking.LocalPlayer) return;
        if (settings == null) return;
        if (!Networking.IsOwner(settings.gameObject)) return;
        settings.ApplyBlockSpeed();
    }
}
