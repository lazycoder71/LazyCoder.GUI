using UnityEngine;

namespace LazyCoder.Gui
{
    [ExecuteInEditMode]
    public class GuiRoundedCornersIndependent : GuiRoundedCorners
    {
        // Vector2.right rotated clockwise by 45 degrees
        private static readonly Vector2 WNorm = new Vector2(.7071068f, -.7071068f);

        // Vector2.right rotated counter-clockwise by 45 degrees
        private static readonly Vector2 HNorm = new Vector2(.7071068f, .7071068f);

        [SerializeField] private Vector4 _r = new Vector4(40f, 40f, 40f, 40f);

        // xy - position,
        // zw - halfSize
        [HideInInspector]
        [SerializeField] private Vector4 _rect2Props;

        protected override string ShaderPath => "LazyCoder/UI/RoundedCornersIndependent";

        protected override void Refresh()
        {
            base.Refresh();

            var rect = ((RectTransform)transform).rect;

            RecalculateProps(rect.size);

            Material.SetVector(PropRect2Props, _rect2Props);
            Material.SetVector(PropHalfSize, rect.size * .5f);
            Material.SetVector(PropRadius, _r);
            Material.SetVector(PropOuterUV, OuterUV);
        }

        private void RecalculateProps(Vector2 size)
        {
            // Vector that goes from left to right sides of rect2
            var aVec = new Vector2(size.x, -size.y + _r.x + _r.z);

            // Project vector aVec to wNorm to get magnitude of rect2 width vector
            var halfWidth = Vector2.Dot(aVec, WNorm) * .5f;
            _rect2Props.z = halfWidth;


            // Vector that goes from bottom to top sides of rect2
            var bVec = new Vector2(size.x, size.y - _r.w - _r.y);

            // Project vector bVec to hNorm to get magnitude of rect2 height vector
            var halfHeight = Vector2.Dot(bVec, HNorm) * .5f;
            _rect2Props.w = halfHeight;


            // Vector that goes from left to top sides of rect2
            var efVec = new Vector2(size.x - _r.x - _r.y, 0);

            // Vector that goes from point E to point G, which is top-left of rect2
            var egVec = HNorm * Vector2.Dot(efVec, HNorm);

            // Position of point E relative to center of coord system
            var ePoint = new Vector2(_r.x - (size.x / 2), size.y / 2);

            // Origin of rect2 relative to center of coord system
            // ePoint + egVec == vector to top-left corner of rect2
            // wNorm * halfWidth + hNorm * -halfHeight == vector from top-left corner to center
            var origin = ePoint + egVec + WNorm * halfWidth + HNorm * -halfHeight;
            _rect2Props.x = origin.x;
            _rect2Props.y = origin.y;
        }
    }
}