using LazyCoder.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LazyCoder.Gui
{
    public class GuiButtonLazy : GuiButton, IPointerDownHandler
    {
        [System.Serializable]
        public enum TransitionType
        {
            None,
            Scale,
        }
        
        [Title("Transition")]
        [SerializeField] private TransitionType _transitionType; 
        
        [Title("Config")]
        [SerializeField] private bool _playSfxOnClick = true;

        [ShowIf("@_playSfxOnClick")]
        [SerializeField] private AudioConfig _sfxClickOverride;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (_playSfxOnClick)
                return;

            if (_sfxClickOverride != null)
                AudioManager.Play(_sfxClickOverride);
            else if (GuiSos.BtnLazySfxClickDefault != null)
                AudioManager.Play(GuiSos.BtnLazySfxClickDefault);
        }
    }
}