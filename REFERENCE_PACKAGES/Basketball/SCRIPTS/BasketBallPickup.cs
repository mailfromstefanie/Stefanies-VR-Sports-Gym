
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class BasketBallPickup : UdonSharpBehaviour
{
    [SerializeField] private bool isFirstBall = true;
    [SerializeField] private BasketBallBall settings = null;
    [SerializeField] private AudioSource bounceSound = null;

    private bool isPicked = false;
    private VRC_Pickup pickup = null;
    private Animator ballAnimator = null;
    private Rigidbody ballRigidbody = null;
    private Collider ballCollider = null;
    private GameObject meshObject = null;
    private Vector3 startPosition = Vector3.zero;
    private bool isTheft = false;

    public bool IsTheft
    {
        get => isTheft;
    }
    public byte BallState
    {
        get => settings.BallState;
    }

    void Start()
    {
        
    }
    void OnEnable()
    {
        if (ballCollider == null) ballCollider = GetComponent<Collider>();
        if (ballAnimator == null) ballAnimator = GetComponentInParent<Animator>();
        if (ballRigidbody == null) ballRigidbody = GetComponent<Rigidbody>();
        if (meshObject == null) {
            Renderer rend = GetComponentInChildren<Renderer>();
            if (rend) meshObject = rend.gameObject;
            if (!isFirstBall) meshObject.SetActive(false);
        }
        if (pickup == null) {
            pickup = GetComponent<VRC_Pickup>();
            pickup.proximity = settings.StealBallDistance;
            startPosition = transform.position;
        }
    }
    public void Dribble(float ballHeight)
    {
        if (!isPicked) {
            if (transform.position.y < settings.GroundY + settings.BallRadius + 0.01f) {
                if (ballAnimator.GetInteger("State") != 2) {
                    transform.parent.localPosition = new Vector3(0, transform.localPosition.y, 0);
                    // transform.localPosition = Vector3.zero;
                    ballAnimator.SetInteger("State", 2);
                    ballAnimator.Update(0f);
                    if (bounceSound && !bounceSound.isPlaying) bounceSound.Play();
                }
            } else if (transform.position.y > ballHeight - 0.2f) {
                if (ballAnimator.GetInteger("State") != 1) {
                    ballAnimator.SetInteger("State", 1);
                    ballAnimator.Update(0f);
                    transform.parent.localPosition = new Vector3(0, -settings.DribbleOffsetY, 0);
                    // transform.localPosition = Vector3.zero;
                }
            }
        }
    }
    public void DropBall()
    {
        if (pickup && pickup.IsHeld) {
            isTheft = true;
            pickup.Drop();
            ResetPosition();
            meshObject.SetActive(true);
        }
    }
    public override void OnPickup()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        bool isLeftHand = false;
        if (pickup && pickup.currentHand == VRC_Pickup.PickupHand.Left) {
            isLeftHand = true;
        }
        SetGripOffset(isLeftHand);
        settings.ChangeBall(isFirstBall, isLeftHand);
        isTheft = false;
    }
    public void SetGripOffset(bool isLeftHand)
    {
        GameObject gripPosition = transform.Find("GripPosition").gameObject;
        if (isLeftHand) {
            gripPosition.transform.localPosition = new Vector3(0, -settings.GripOffsetX, 0);
        } else {
            gripPosition.transform.localPosition = new Vector3(0, settings.GripOffsetX, 0);
        }
    }
    // public void SetGripPosition(bool hasOffset)
    // {
    //     if (pickup && !pickup.IsHeld) return;
    //     GameObject gripPosition = transform.Find("GripPosition").gameObject;
    //     if (hasOffset) {
    //         if (pickup && pickup.currentHand == VRC_Pickup.PickupHand.Left) {
    //             gripPosition.transform.localPosition = new Vector3(0, -settings.GripOffsetX, 0);
    //         } else {
    //             gripPosition.transform.localPosition = new Vector3(0, settings.GripOffsetX, 0);
    //         }
    //     } else {
    //         gripPosition.transform.localPosition = Vector3.zero;
    //     }
    // }
    public float GetGripPositionY()
    {
        GameObject gripPosition = transform.Find("GripPosition").gameObject;
        return gripPosition.transform.position.y;
    }
    public override void OnDrop()
    {
        settings.DropBall();
        isTheft = false;
    }
    public override void OnPickupUseDown()
    {
        settings.ShootMode();
    }
    public void DribbleStart()
    {
        ResetPosition();
        ballAnimator.enabled = true;
        ballAnimator.SetInteger("State", 1);
        meshObject.SetActive(true);
        EnablePickup();
        SetRigidbody(false);
    }
    public void ResetPosition()
    {
        transform.parent.localPosition = Vector3.zero;
        transform.localPosition = Vector3.zero;
    }
    public void Picked()
    {
        if (pickup && !pickup.IsHeld) pickup.pickupable = false;
        isPicked = true;
        StopBall();
        SetRigidbody(false);
    }
    public void StopBall()
    {
        ballAnimator.SetInteger("State", 0);
        ballAnimator.enabled = false;
        meshObject.SetActive(false);
    }
    public void EnablePickup()
    {
        pickup.pickupable = true;
        ballCollider.enabled = true;
        isPicked = false;
    }
    public void ShowMesh()
    {
        meshObject.SetActive(true);
        SetRigidbody(false);
    }
    public void EnableShoot()
    {
        EnablePickup();
        SetRigidbody(true);
        meshObject.SetActive(true);
    }
    public void DribbleStop()
    {
        StopBall();
        ResetPosition();
        isPicked = false;
        pickup.pickupable = false;
        ballCollider.enabled = false;
        SetRigidbody(false);
    }
    public void SetBallBoost(float boostScale)
    {
        if (pickup == null) OnEnable();
        if (boostScale < 0.2) boostScale = 0.2f;
        pickup.ThrowVelocityBoostScale = boostScale;
    }
    public void SetRigidbody(bool useGravity)
    {
        if (useGravity) {
            ballRigidbody.isKinematic = false;
            ballRigidbody.useGravity = true;
            ballRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        } else {
            ballRigidbody.isKinematic = true;
            ballRigidbody.useGravity = false;
            ballRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
    public void CheckRespawn()
    {
        if (!Networking.IsOwner(settings.gameObject)) return;
        if (pickup && pickup.IsHeld) return;
        if (transform.position.y < settings.GroundY - settings.BallRadius * 2) {
            Respawn();
        }
    }
    public void Respawn()
    {
        if (pickup && pickup.IsHeld) return;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        settings.ResetAll(startPosition);
    }
    public bool IsHeld()
    {
        if (pickup == null) return false;
        if (pickup.currentPlayer != Networking.LocalPlayer) return false;
        return pickup.IsHeld;
    }
    public bool IsOwner()
    {
        return Networking.IsOwner(settings.gameObject);
    }
    // public void MovePosition(Vector3 position)
    // {
    //     Vector3 velocity = (position - transform.position) * 50f;
    //     ballRigidbody.MovePosition(Vector3.SmoothDamp(transform.position, position, ref velocity, 0.02f));
    // }
    private void OnCollisionEnter(Collision other)
    {
        if (other == null) return;
        if (other.gameObject == null) return;
        if (other.gameObject.name == "BasketBall_GoalNet" || other.gameObject.name == "BasketBall_BottomCollider") return;
        if (bounceSound && !bounceSound.isPlaying) bounceSound.Play();
    }
}
