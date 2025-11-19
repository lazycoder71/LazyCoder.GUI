using LazyCoder.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LazyCoder.Gui
{
    public class GuiButtonLazy : GuiButton, IPointerDownHandler, IPointerUpHandler
    {
        [Title("Transition")]
        [SerializeReference] private GuiButtonLazyTransition _transition;

        [Title("Sound")]
        [SerializeField] private bool _playSfxOnClick = true;

        [ShowIf("@_playSfxOnClick")]
        [SerializeField] private AudioConfig _sfxClickOverride;

        protected override void Awake()
        {
            base.Awake();

            if (_transition != null)
                _transition.Init(this);
        }

        private void PlaySfx()
        {
            if (!_playSfxOnClick)
                return;

            if (_sfxClickOverride != null)
                AudioManager.Play(_sfxClickOverride);
            else if (GuiSos.BtnLazySfxClickDefault != null)
                AudioManager.Play(GuiSos.BtnLazySfxClickDefault);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            PlaySfx();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            
        }
    }
}