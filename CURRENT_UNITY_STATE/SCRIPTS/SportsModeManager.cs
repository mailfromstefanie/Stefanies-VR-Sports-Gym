using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDKBase;

namespace StefanieInVR
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SportsModeManager : UdonSharpBehaviour
    {
        public const int MODE_BASKETBALL = 0;
        public const int MODE_SOCCER = 1;
        public const int MODE_VOLLEYBALL = 2;
        public const int MODE_SOCCER_HOCKEY = 3;

        [Header("Start Mode")]
        [Tooltip(
            "0 = Basketball, 1 = Soccer, " +
            "2 = Volleyball, 3 = Soccer Hockey"
        )]
        [Range(0, 3)]
        [SerializeField]
        private int defaultMode = MODE_BASKETBALL;

        [Header("Basketball Only")]
        [Tooltip(
            "Objecten die alleen bij Basketball actief zijn."
        )]
        public GameObject[] basketballObjects;

        [Header("Soccer Only")]
        [Tooltip(
            "Objecten die alleen bij Soccer actief zijn. " +
            "Plaats hier alle avatarcolliders, pivots en collidercontrollers."
        )]
        [FormerlySerializedAs("soccerObjects")]
        public GameObject[] soccerOnlyObjects;

        [Header("Volleyball Only")]
        [Tooltip(
            "Objecten die alleen bij Volleyball actief zijn."
        )]
        public GameObject[] volleyballObjects;

        [Header("Soccer Hockey Only")]
        [Tooltip(
            "Objecten die alleen bij Soccer Hockey actief zijn. " +
            "Bijvoorbeeld hockeysticks en hockeyspecifieke UI."
        )]
        public GameObject[] soccerHockeyObjects;

        [Header("Soccer + Hockey Shared")]
        [Tooltip(
            "Objecten die bij Soccer en Soccer Hockey actief zijn. " +
            "Bijvoorbeeld de voetbal, doelen en gedeelde goallogica."
        )]
        public GameObject[] soccerHockeySharedObjects;

        [Header("SoccerBox Games Shared")]
        [Tooltip(
            "Technische SoccerBox-objecten die actief zijn bij Soccer, " +
            "Volleyball en Soccer Hockey. Bijvoorbeeld Programs en Play Area. " +
            "Plaats hier geen avatarcolliders."
        )]
        [FormerlySerializedAs("soccerVolleyballSharedObjects")]
        public GameObject[] soccerBoxSharedObjects;

        [Header("Always Active")]
        [Tooltip(
            "Objecten die bij alle vier de sporten actief blijven."
        )]
        public GameObject[] sharedObjects;

        [Header("Floor Material")]
        [Tooltip("De Renderer van de sportvloer.")]
        public Renderer floorRenderer;

        [Tooltip(
            "Het materiaal-slot op de vloer dat moet wisselen."
        )]
        public int floorMaterialSlot = 0;

        [Tooltip("Vloermateriaal met basketballijnen.")]
        public Material basketballFloorMaterial;

        [Tooltip("Vloermateriaal met soccer-/futsallijnen.")]
        public Material soccerFloorMaterial;

        [Tooltip("Vloermateriaal met volleyballijnen.")]
        public Material volleyballFloorMaterial;

        [Tooltip(
            "Vloermateriaal voor Soccer Hockey. " +
            "Laat dit leeg om het Soccer-materiaal te gebruiken."
        )]
        public Material soccerHockeyFloorMaterial;

        [Header("Sport Buttons")]
        [Tooltip(
            "Sleep hier de vier SportsModeButton-componenten in."
        )]
        public SportsModeButton[] modeButtons;

        [UdonSynced]
        private int activeMode;

        private bool initialized;

        private void Start()
        {
            if (Networking.IsOwner(gameObject))
            {
                activeMode = ClampMode(defaultMode);
                RequestSerialization();
            }

            initialized = true;
            ApplyActiveMode();
        }

        public void SetBasketball()
        {
            SetMode(MODE_BASKETBALL);
        }

        public void SetSoccer()
        {
            SetMode(MODE_SOCCER);
        }

        public void SetVolleyball()
        {
            SetMode(MODE_VOLLEYBALL);
        }

        public void SetSoccerHockey()
        {
            SetMode(MODE_SOCCER_HOCKEY);
        }

        public void SetMode(int newMode)
        {
            newMode = ClampMode(newMode);

            if (activeMode == newMode)
            {
                ApplyActiveMode();
                return;
            }

            VRCPlayerApi localPlayer = Networking.LocalPlayer;

            if (localPlayer == null || !localPlayer.IsValid())
            {
                return;
            }

            if (!Networking.IsOwner(gameObject))
            {
                Networking.SetOwner(localPlayer, gameObject);
            }

            activeMode = newMode;

            ApplyActiveMode();
            RequestSerialization();
        }

        public override void OnDeserialization()
        {
            if (!initialized)
            {
                return;
            }

            activeMode = ClampMode(activeMode);
            ApplyActiveMode();
        }

        public override void OnOwnershipTransferred(
            VRCPlayerApi player
        )
        {
            ApplyActiveMode();
        }

        public int GetActiveMode()
        {
            return activeMode;
        }

        public bool IsModeActive(int mode)
        {
            return activeMode == ClampMode(mode);
        }

        public void ApplyActiveMode()
        {
            bool basketballActive =
                activeMode == MODE_BASKETBALL;

            bool soccerActive =
                activeMode == MODE_SOCCER;

            bool volleyballActive =
                activeMode == MODE_VOLLEYBALL;

            bool soccerHockeyActive =
                activeMode == MODE_SOCCER_HOCKEY;

            bool soccerOrHockeyActive =
                soccerActive || soccerHockeyActive;

            bool soccerBoxGameActive =
                soccerActive ||
                volleyballActive ||
                soccerHockeyActive;

            SetObjectArrayActive(
                basketballObjects,
                basketballActive
            );

            SetObjectArrayActive(
                soccerOnlyObjects,
                soccerActive
            );

            SetObjectArrayActive(
                volleyballObjects,
                volleyballActive
            );

            SetObjectArrayActive(
                soccerHockeyObjects,
                soccerHockeyActive
            );

            SetObjectArrayActive(
                soccerHockeySharedObjects,
                soccerOrHockeyActive
            );

            SetObjectArrayActive(
                soccerBoxSharedObjects,
                soccerBoxGameActive
            );

            SetObjectArrayActive(
                sharedObjects,
                true
            );

            ApplyFloorMaterial();
            RefreshButtonVisuals();
        }

        public void RefreshButtonVisuals()
        {
            if (modeButtons == null)
            {
                return;
            }

            for (int i = 0; i < modeButtons.Length; i++)
            {
                SportsModeButton modeButton =
                    modeButtons[i];

                if (modeButton != null)
                {
                    modeButton.RefreshVisual();
                }
            }
        }

        private void ApplyFloorMaterial()
        {
            if (floorRenderer == null)
            {
                return;
            }

            Material targetMaterial =
                GetFloorMaterialForActiveMode();

            if (targetMaterial == null)
            {
                return;
            }

            Material[] currentMaterials =
                floorRenderer.sharedMaterials;

            if (currentMaterials == null ||
                currentMaterials.Length == 0)
            {
                return;
            }

            if (floorMaterialSlot < 0 ||
                floorMaterialSlot >= currentMaterials.Length)
            {
                return;
            }

            if (currentMaterials[floorMaterialSlot] ==
                targetMaterial)
            {
                return;
            }

            currentMaterials[floorMaterialSlot] =
                targetMaterial;

            floorRenderer.sharedMaterials =
                currentMaterials;
        }

        private Material GetFloorMaterialForActiveMode()
        {
            if (activeMode == MODE_BASKETBALL)
            {
                return basketballFloorMaterial;
            }

            if (activeMode == MODE_SOCCER)
            {
                return soccerFloorMaterial;
            }

            if (activeMode == MODE_VOLLEYBALL)
            {
                return volleyballFloorMaterial;
            }

            if (soccerHockeyFloorMaterial != null)
            {
                return soccerHockeyFloorMaterial;
            }

            return soccerFloorMaterial;
        }

        private void SetObjectArrayActive(
            GameObject[] objects,
            bool shouldBeActive
        )
        {
            if (objects == null)
            {
                return;
            }

            for (int i = 0; i < objects.Length; i++)
            {
                GameObject target = objects[i];

                if (target == null)
                {
                    continue;
                }

                if (target.activeSelf != shouldBeActive)
                {
                    target.SetActive(shouldBeActive);
                }
            }
        }

        private int ClampMode(int mode)
        {
            if (mode < MODE_BASKETBALL)
            {
                return MODE_BASKETBALL;
            }

            if (mode > MODE_SOCCER_HOCKEY)
            {
                return MODE_SOCCER_HOCKEY;
            }

            return mode;
        }
    }
}