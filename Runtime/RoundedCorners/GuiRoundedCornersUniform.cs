using UnityEngine;

namespace LazyCoder.Gui
{
    public class GuiRoundedCornersUniform : GuiRoundedCorners
    {
        [SerializeField] private float _radius = 40f;

        protected override void Refresh()
        {
            var rect = ((RectTransform)transform).rect;

            // Multiply radius value by 2 to make the radius value appear consistent with GuiImageRoundedCornersUniform script.
            // Right now, the GuiImageRoundedCornersUniform appears to have double the radius than this.
            Material.SetVector(PropWidthHeightRadius, new Vector4(rect.width, rect.height, _radius * 2, 0));
            Material.SetVector(PropOuterUV, OuterUV);
        }
    }
}