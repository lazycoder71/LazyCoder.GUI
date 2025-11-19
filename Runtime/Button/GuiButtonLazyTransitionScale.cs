using LazyCoder.Core;
using UnityEngine;

namespace LazyCoder.Gui
{
    public class GuiButtonLazyTransitionScale : GuiButtonLazyTransition
    {
        [SerializeField] private GuiGraphicScale.Config _config;

        public override void Init(GuiButtonLazy button)
        {
            GuiGraphicScale guiGraphicScale = button.gameObject.AddComponent<GuiGraphicScale>();

            guiGraphicScale.Construct(_config);
        }
    }
}