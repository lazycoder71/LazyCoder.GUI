using LazyCoder.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LazyCoder.Gui
{
    public class GuiButtonLazy : GuiButton, IPointerDownHandler
    {
        [Title("Config")]
        [SerializeField] private bool _playSoundOnClick = true;

        [SerializeField] private AudioConfig _clickSound;
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}
