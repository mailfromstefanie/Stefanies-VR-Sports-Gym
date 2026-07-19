
using UdonSharp;
using UnityEngine;
using UnityEngine.Animations;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class BasketBallBall : UdonSharpBehaviour
{
    [SerializeField] private float ballSpeedBuff = 1.0f;
    [SerializeField] private float standardAvatarHeight = 2f;
    [SerializeField] private float ballSpeedDebuff = 1.0f;
    [SerializeField] private float ballSyncDuration = 0.2f;
    [SerializeField] private bool immobilizeWhenShoot = true;
    [SerializeField] private float jumpImpulseWhenShoot = 6f;
    [SerializeField] private int jumpNumWhenShoot = 1;
    [SerializeField] private float stealBallDistance = 0.5f;
    [SerializeField] private float groundY = 0.0f;
    [SerializeField] private float ballRadius = 0.11f;
    [SerializeField] private float dribbleOffsetY = 0.11f;
    [SerializeField] private float gripOffsetX = 0.11f;
    [SerializeField] private bool enableBlock = true;
    [SerializeField] private float blockDistance = 1.5f;
    [SerializeField] private float blockWidth = 0.4f;
    [SerializeField] private float blockTime = 1f;
    [SerializeField] private float blockWalkSpeed = 0.5f;
    [SerializeField] private float blockRunSpeed = 0.5f;
    [SerializeField] private float blockStrafeSpeed = 0.5f;
    [SerializeField] private BasketBallPickup firstBall = null;
    [SerializeField] private BasketBallPickup secondBall = null;
    [SerializeField] private PositionConstraint firstBallConstraint = null;
    [SerializeField] private PositionConstraint secondBallConstraint = null;
    [SerializeField] private BasketBallBlock blockTrigger = null;
    
    [UdonSynced, FieldChangeCallback(nameof(BallPosition))]
    private Vector3 _ballPosition = Vector3.zero;
    
    [UdonSynced, FieldChangeCallback(nameof(BallRotation))]
    private Quaternion _ballRotation = Quaternion.identity;

    [UdonSynced, FieldChangeCallback(nameof(IsLeftHand))]
    private bool _isLeftHand = false;

    [UdonSynced] private bool IsFirstBall = true;
    [UdonSynced] private byte _ballState = 0;
    [UdonSynced] private bool IsPicked = false;
    [UdonSynced] private Vector3 pickupRelPosition = Vector3.zero;
    [UdonSynced] private Quaternion pickupRelRotation = Quaternion.identity;
    private byte BallStateLocal = 0;
    private VRCPlayerApi localPlayer = null;
    private float defaultWalkSpeed = 2.0f;
    private float defaultRunSpeed = 4.0f;
    private float defaultStrafeSpeed = 2.0f;
    private float defaultJumpImpulse = 3.0f;
    private int jumpNum = 0;
    private bool isFirstBallLocal = true;
    private float blockStartTime = 0.0f;
    private float ballSyncTime = 0.0f;
    private float setBoneTime = 0.0f;
    private bool defaultScaling = false;
    // private bool isFistBallLocal = true;
    public byte BallState
    {
        set
        {
            _ballState = value;
        }
        get => _ballState;
    }
    public Vector3 BallPosition
    {
        set
        {
            _ballPosition = value;
            if (value == Vector3.zero) return;
            if (!Networking.IsOwner(gameObject)) {
                if (IsFirstBall) {
                    firstBall.transform.position = value;
                } else {
                    secondBall.transform.position = value;
                }
            }
        }
        get => _ballPosition;
    }
    public Quaternion BallRotation
    {
        set
        {
            _ballRotation = value;
            if (!Networking.IsOwner(gameObject)) {
                if (IsFirstBall) {
                    firstBall.transform.rotation = value;
                } else {
                    secondBall.transform.rotation = value;
                }
            }
        }
        get => _ballRotation;
    }
    public bool IsLeftHand
    {
        set
        {
            _isLeftHand = value;
        }
        get => _isLeftHand;
    }
    public float StealBallDistance
    {
        get => stealBallDistance;
    }
    public float GroundY
    {
        get => groundY;
    }
    public float BallRadius
    {
        get => ballRadius;
    }
    public float DribbleOffsetY
    {
        get => dribbleOffsetY;
    }
    public float GripOffsetX
    {
        get => gripOffsetX;
    }
    public float BlockDistance
    {
        get => blockDistance;
    }
    public float BlockWidth
    {
        get => blockWidth;
    }
    void Start()
    {
    }
    void OnEnable()
    {
        if (localPlayer == null) {
            localPlayer = Networking.LocalPlayer;
            defaultScaling = localPlayer.GetManualAvatarScalingAllowed();
            Physics.IgnoreCollision(firstBall.GetComponent<Collider>(), secondBall.GetComponent<Collider>());
            Vector3 parentLossyScale = blockTrigger.transform.parent.lossyScale;
            blockTrigger.transform.localScale = new Vector3(blockWidth / parentLossyScale.x, 1.6f / parentLossyScale.y, blockDistance / parentLossyScale.z);
            blockTrigger.transform.localPosition = new Vector3(0.0f, 0.8f, blockDistance / 2.0f);
            // GetDefaultPalameters();
            SetBallBuff();
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(RequestSerialization_));
    }
    public void GetDefaultPalameters()
    {
        if (Utilities.IsValid(localPlayer)) {
            defaultWalkSpeed = localPlayer.GetWalkSpeed();
            defaultRunSpeed = localPlayer.GetRunSpeed();
            defaultStrafeSpeed = localPlayer.GetStrafeSpeed();
            defaultJumpImpulse = localPlayer.GetJumpImpulse();
        }
    }
    void Update()
    {
        if (setBoneTime != 0f && Time.time - setBoneTime > 1f) {
            setBoneTime = 0f;
            if (BallState == 2) {
                SetRelPosition(true);
            } else {
                SetRelPosition(false);
            }
        }
        if (enableBlock) {
            if (BallState == 1 && Networking.IsOwner(gameObject)) {
                blockTrigger.transform.parent.position = new Vector3(localPlayer.GetPosition().x, groundY, localPlayer.GetPosition().z);
                blockTrigger.transform.parent.rotation = localPlayer.GetRotation();
                blockTrigger.transform.parent.gameObject.SetActive(true);
                if (blockStartTime != 0f && Time.time - blockStartTime > blockTime) {
                    blockStartTime = 0f;
                    localPlayer.SetWalkSpeed(defaultWalkSpeed);
                    localPlayer.SetRunSpeed(defaultRunSpeed);
                    localPlayer.SetStrafeSpeed(defaultStrafeSpeed);
                }
            } else {
                blockTrigger.transform.parent.gameObject.SetActive(false);
                if (blockStartTime != 0f) {
                    blockStartTime = 0f;
                    localPlayer.SetWalkSpeed(defaultWalkSpeed);
                    localPlayer.SetRunSpeed(defaultRunSpeed);
                    localPlayer.SetStrafeSpeed(defaultStrafeSpeed);
                }
            }
        }
        if (IsFirstBall) {
            if (!Networking.IsOwner(gameObject) && IsPicked) {
                Vector3 handPosition;
                Quaternion handRotation;
                if (IsLeftHand) {
                    handPosition = Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.LeftHand);
                    handRotation = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.LeftHand);
                } else {
                    handPosition = Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.RightHand);
                    handRotation = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.RightHand);
                }
                firstBall.transform.position = handPosition + (handRotation * pickupRelPosition);
                firstBall.transform.rotation = handRotation * pickupRelRotation;
            } else if (Networking.IsOwner(gameObject) && !IsPicked && BallState == 2) {
                if (ballSyncTime == 0f || Time.time - ballSyncTime > ballSyncDuration) {
                    ballSyncTime = Time.time;
                    BallPosition = firstBall.transform.position;
                    BallRotation = firstBall.transform.rotation;
                    RequestSerialization();
                }
            }
            secondBall.DropBall();
            if (BallState == 0){    // Idle
                if (BallStateLocal != 0 || !isFirstBallLocal) {
                    BallStateLocal = 0;
                    secondBall.DribbleStop();
                    firstBall.EnablePickup();
                    firstBall.ShowMesh();
                }
            } else if (BallState == 1) {    // Dribble
                if (BallStateLocal != 1 || !isFirstBallLocal) {
                    BallStateLocal = 1;
                    secondBall.DribbleStart();
                    firstBall.Picked();
                }
                secondBall.Dribble(firstBall.GetGripPositionY());
            } else if (BallState == 2) {    // Shoot
                if (BallStateLocal != 2 || !isFirstBallLocal) {
                    BallStateLocal = 2;
                    firstBall.EnableShoot();
                    secondBall.DribbleStop();
                }
                if (!IsPicked && !Networking.IsOwner(gameObject)) {
                    firstBall.SetRigidbody(false);
                }
            }
            isFirstBallLocal = true;
            firstBallConstraint.enabled = false;
            secondBallConstraint.enabled = true;
            firstBall.CheckRespawn();
        } else {
            if (!Networking.IsOwner(gameObject) && IsPicked) {
                Vector3 handPosition;
                Quaternion handRotation;
                if (IsLeftHand) {
                    handPosition = Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.LeftHand);
                    handRotation = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.LeftHand);
                } else {
                    handPosition = Networking.GetOwner(gameObject).GetBonePosition(HumanBodyBones.RightHand);
                    handRotation = Networking.GetOwner(gameObject).GetBoneRotation(HumanBodyBones.RightHand);
                }
                secondBall.transform.position = handPosition + (handRotation * pickupRelPosition);
                secondBall.transform.rotation = handRotation * pickupRelRotation;
            } else if (Networking.IsOwner(gameObject) && !IsPicked && BallState == 2) {
                if (ballSyncTime == 0f || Time.time - ballSyncTime > ballSyncDuration) {
                    ballSyncTime = Time.time;
                    BallPosition = secondBall.transform.position;
                    BallRotation = secondBall.transform.rotation;
                    RequestSerialization();
                }
            }
            firstBall.DropBall();
            if (BallState == 0) {   // Idle
                if (BallStateLocal != 0 || isFirstBallLocal) {
                    BallStateLocal = 0;
                    firstBall.DribbleStop();
                    secondBall.EnablePickup();
                    secondBall.ShowMesh();
                }
            } else if (BallState == 1) { // Dribble
                if (BallStateLocal != 1 || isFirstBallLocal) {
                    BallStateLocal = 1;
                    firstBall.DribbleStart();
                    secondBall.Picked();
                }
                firstBall.Dribble(secondBall.GetGripPositionY());
            } else if (BallState == 2) { // Shoot
                if (BallStateLocal != 2 || isFirstBallLocal) {
                    BallStateLocal = 2;
                    secondBall.EnableShoot();
                    firstBall.DribbleStop();
                }
                if (!IsPicked && !Networking.IsOwner(gameObject)) {
                    secondBall.SetRigidbody(false);
                }
            }
            isFirstBallLocal = false;
            firstBallConstraint.enabled = true;
            secondBallConstraint.enabled = false;
            secondBall.CheckRespawn();
        }
    }
    public void SetBallBuff()
    {
        if (!Utilities.IsValid(localPlayer)) return;
        if (defaultScaling) localPlayer.SetManualAvatarScalingAllowed(false);
        float ballBuff = 1f - ballSpeedBuff * (localPlayer.GetAvatarEyeHeightAsMeters() - standardAvatarHeight);
        firstBall.SetBallBoost(ballBuff);
        secondBall.SetBallBoost(ballBuff);
        if (defaultScaling) localPlayer.SetManualAvatarScalingAllowed(true);
    }
    public void ApplyBlockSpeed()
    {
        if (!Utilities.IsValid(localPlayer)) return;
        if (!enableBlock) return;
        if (BallState == 1) {
            localPlayer.SetWalkSpeed(blockWalkSpeed);
            localPlayer.SetRunSpeed(blockRunSpeed);
            localPlayer.SetStrafeSpeed(blockStrafeSpeed);
            blockStartTime = Time.time;
        }
    }
    public void ChangeBall(bool isFirstBall, bool isLeftHand)
    {
        if (!Utilities.IsValid(localPlayer)) return;
        Networking.SetOwner(localPlayer, gameObject);
        IsFirstBall = isFirstBall;
        BallState = 1;
        IsPicked = true;
        IsLeftHand = isLeftHand;
        // isFistBallLocal = isFirstBall;
        RequestSerialization();
        setBoneTime = Time.time;
    }
    public void SetRelPosition(bool isShoot)
    {
        Vector3 handPosition;
        Quaternion handRotation;
        if (IsLeftHand) {
            handPosition = localPlayer.GetBonePosition(HumanBodyBones.LeftHand);
            handRotation = localPlayer.GetBoneRotation(HumanBodyBones.LeftHand);
        } else {
            handPosition = localPlayer.GetBonePosition(HumanBodyBones.RightHand);
            handRotation = localPlayer.GetBoneRotation(HumanBodyBones.RightHand);
        }
        if (isShoot) {
            if (IsFirstBall) {
                pickupRelPosition = Quaternion.Inverse(handRotation) * (firstBall.transform.position - handPosition);
                pickupRelRotation = Quaternion.Inverse(handRotation) * firstBall.transform.rotation;
            } else {
                pickupRelPosition = Quaternion.Inverse(handRotation) * (secondBall.transform.position - handPosition);
                pickupRelRotation = Quaternion.Inverse(handRotation) * secondBall.transform.rotation;
            }
        } else {
            if (IsFirstBall) {
                pickupRelPosition = Quaternion.Inverse(handRotation) * (firstBall.transform.Find("GripPosition").position - handPosition);
                pickupRelRotation = Quaternion.Inverse(handRotation) * firstBall.transform.rotation;
            } else {
                pickupRelPosition = Quaternion.Inverse(handRotation) * (secondBall.transform.Find("GripPosition").position - handPosition);
                pickupRelRotation = Quaternion.Inverse(handRotation) * secondBall.transform.rotation;
            }
        }
        RequestSerialization();
    }
    public void DropBall()
    {
        if (!Utilities.IsValid(localPlayer)) return;
        if (firstBall.IsHeld() || secondBall.IsHeld()) return;  // localPlayer is holding the ball
        if (BallState > 0) {
            if (immobilizeWhenShoot) {
                localPlayer.SetWalkSpeed(defaultWalkSpeed);
                localPlayer.SetRunSpeed(defaultRunSpeed);
                localPlayer.SetStrafeSpeed(defaultStrafeSpeed);
                localPlayer.SetJumpImpulse(defaultJumpImpulse);
            }
        }
        if (!firstBall.IsTheft && !secondBall.IsTheft) {
            Networking.SetOwner(localPlayer, gameObject);
            if (BallState != 2) {
                BallState = 0;
                if (IsFirstBall) {
                    BallPosition = firstBall.transform.position;
                    BallRotation = firstBall.transform.rotation;
                } else {
                    BallPosition = secondBall.transform.position;
                    BallRotation = secondBall.transform.rotation;
                }
            }
            IsPicked = false;
            RequestSerialization();
        }
        jumpNum = 0;
        setBoneTime = 0f;
    }
    public void ResetAll(Vector3 position)
    {
        BallPosition = position;
        BallRotation = Quaternion.identity;
        BallState = 0;
        RequestSerialization();
    }
    public void ShootMode()
    {
        if (!Utilities.IsValid(localPlayer)) return;
        if (BallState == 2) return;
        Networking.SetOwner(localPlayer, gameObject);
        BallState = 2;
        SetRelPosition(true);
        if (immobilizeWhenShoot) {
            // GetDefaultPalameters();
            localPlayer.SetWalkSpeed(0f);
            localPlayer.SetRunSpeed(0f);
            localPlayer.SetStrafeSpeed(0f);
            localPlayer.SetJumpImpulse(jumpImpulseWhenShoot);
        }
        RequestSerialization();
    }
    public override void InputJump(bool value, VRC.Udon.Common.UdonInputEventArgs args)
    {
        if (!Utilities.IsValid(localPlayer)) return;
        if (BallState != 2) return;
        if (!firstBall.IsHeld() && !secondBall.IsHeld()) return;
        if (value) {
            if (jumpNum < jumpNumWhenShoot) {
                jumpNum++;
            } else {
                localPlayer.SetJumpImpulse(0f);
            }
        }
    }
    public void RespawnBall()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, nameof(RespawnBall_));
    }
    public void RespawnBall_()
    {
        if (!Utilities.IsValid(localPlayer)) return;
        if (IsFirstBall) {
            firstBall.Respawn();
        } else {
            secondBall.Respawn();
        }
    }
    public override void OnAvatarChanged(VRCPlayerApi player)
    {
        if (player != localPlayer) return;
        SetBallBuff();
        if (!Networking.IsOwner(gameObject)) return;
        if (!IsPicked) return;
        setBoneTime = Time.time;
    }
    public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
    {
        if (player != localPlayer) return;
        SetBallBuff();
        if (!Networking.IsOwner(gameObject)) return;
        if (!IsPicked) return;
        setBoneTime = Time.time;
    }
    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if(Networking.IsOwner(gameObject)){
            SendCustomEventDelayedSeconds(nameof(RequestSerialization_), 5f);
        }
        if (player == Networking.LocalPlayer) {
            GetDefaultPalameters();
        }
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (!Networking.IsOwner(gameObject)) return;
        DropBall();
    }

    public void RequestSerialization_()
    {
        RequestSerialization();
    }
}
