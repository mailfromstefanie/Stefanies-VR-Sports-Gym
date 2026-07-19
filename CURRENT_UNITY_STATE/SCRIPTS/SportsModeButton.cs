using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace StefanieInVR
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SportsModeButton : UdonSharpBehaviour
    {
        [Header("Manager")]
        [Tooltip("De centrale SportsModeManager.")]
        public SportsModeManager sportsModeManager;

        [Header("Button Visual")]
        [Tooltip(
            "De Image waarvan de sprite moet veranderen."
        )]
        public Image buttonImage;

        [Header("Sport Mode")]
        [Tooltip(
            "0 = Basketball, 1 = Soccer, " +
            "2 = Volleyball, 3 = Soccer Hockey"
        )]
        [Range(0, 3)]
        public int modeIndex;

        [Header("Sprites")]
        [Tooltip("Sprite wanneer deze sport niet actief is.")]
        public Sprite inactiveSprite;

        [Tooltip("Sprite wanneer deze sport actief is.")]
        public Sprite activeSprite;

        public void OnButtonPressed()
        {
            if (sportsModeManager == null)
            {
                return;
            }

            sportsModeManager.SetMode(modeIndex);
        }

        public void RefreshVisual()
        {
            if (sportsModeManager == null ||
                buttonImage == null)
            {
                return;
            }

            bool isActive =
                sportsModeManager.IsModeActive(modeIndex);

            Sprite targetSprite =
                isActive
                    ? activeSprite
                    : inactiveSprite;

            if (targetSprite != null &&
                buttonImage.sprite != targetSprite)
            {
                buttonImage.sprite = targetSprite;
            }
        }
    }
}