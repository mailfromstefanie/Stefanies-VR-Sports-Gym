
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

public enum BasketBallTeamType
{
    None,
    Red,
    Blue
}

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class BasketBallGoal : UdonSharpBehaviour
{
    [SerializeField] private bool IsSinglePointGoal = false;
    [SerializeField] private BasketBallTeamType teamType = BasketBallTeamType.Red;
    [SerializeField] private BasketBallGrouping grouping = null;
    [SerializeField] private GameObject bottomColliderObject = null;
    [SerializeField] private ParticleSystem[] goalEffects = null;
    [SerializeField] private AudioSource goalSound = null;

    [UdonSynced] private bool EffectTrigger = false;

    private bool effectTriggerLocal = false;
    
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.gameObject == null) return;
        if (other.gameObject.name != "BasketBallBall") return;
        if (!Networking.IsOwner(other.gameObject)) return;
        BasketBallPickup ball = other.gameObject.GetComponent<BasketBallPickup>();
        if (ball == null) return;
        if (!ball.IsOwner()) return;
        if (ball.BallState != 2) return;
        if (bottomColliderObject) bottomColliderObject.SetActive(false);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;
        if (other.gameObject == null) return;
        if (other.gameObject.name != "BasketBallBall") return;
        BasketBallPickup ball = other.gameObject.GetComponent<BasketBallPickup>();
        if (ball == null) return;
        if (!ball.IsOwner()) return;
        if (ball.BallState != 2) return;
        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        if (!Utilities.IsValid(localPlayer)) return;
        if (bottomColliderObject) bottomColliderObject.SetActive(true);
        if (other.transform.position.y > transform.position.y) return;
        Networking.SetOwner(localPlayer, gameObject);
        EffectTrigger = !EffectTrigger;
        RequestSerialization();
        PlayEffect();
        if (grouping) {
            int sign = 1;
            if ((teamType == BasketBallTeamType.Red && grouping.GetTeam() == 0)
                || (teamType == BasketBallTeamType.Blue && grouping.GetTeam() == 1)) {
                sign = -1;
            }
            if (IsSinglePointGoal) {
                grouping.AddScore(sign * 1);
            } else {
                Vector3 position = transform.InverseTransformPoint(localPlayer.GetPosition());
                position.y = 0f;
                if (position.z < 1.4151f) {
                    if (position.x < -6.6f || position.x > 6.6f) {
                        grouping.AddScore(sign * 3);
                    } else {
                        grouping.AddScore(sign * 2);
                    }
                } else {
                    if (position.sqrMagnitude > 6.75f * 6.75f) {
                        grouping.AddScore(sign * 3);
                    } else {
                        grouping.AddScore(sign * 2);
                    }
                }
            }
        }
    }
    public override void OnDeserialization(DeserializationResult result)
    {
        if (result.sendTime < 0) return;
        if (Time.realtimeSinceStartup - result.sendTime > 1f) return;
        if (EffectTrigger == effectTriggerLocal) return;
        effectTriggerLocal = EffectTrigger;
        PlayEffect();
    }
    public void PlayEffect()
    {
        for (int i = 0; i < goalEffects.Length; i++) {
            if (goalEffects[i] == null) continue;
            goalEffects[i].Play();
        }
        if (goalSound && !goalSound.isPlaying) goalSound.Play();
    }
}
