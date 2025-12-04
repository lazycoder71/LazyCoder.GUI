using UnityEngine;
using UnityEngine.UI;

namespace LazyCoder.Gui
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform), typeof(MaskableGraphic))]
    public abstract class GuiRoundedCorners : MonoBehaviour
    {
        protected static readonly int PropWidthHeightRadius = Shader.PropertyToID("_WidthHeightRadius");
        protected static readonly int PropOuterUV = Shader.PropertyToID("_OuterUV");
        protected static readonly int PropHalfSize = Shader.PropertyToID("_halfSize");
        protected static readonly int PropRadius = Shader.PropertyToID("_r");
        protected static readonly int PropRect2Props = Shader.PropertyToID("_rect2props");

        [HideInInspector]
        [SerializeField] protected MaskableGraphic MaskableGraphic;

        protected Material Material;

        protected Vector4 OuterUV = new Vector4(0, 0, 1, 1);

        protected virtual string ShaderPath => "LazyCoder/UI/RoundedCorners";

        private void OnValidate()
        {
            Validate();
            Refresh();
        }

        private void OnDestroy()
        {
            if (MaskableGraphic != null)
            {
                // This makes so that when the component is removed, the UI material returns to null
                MaskableGraphic.material = null;
            }

            SafeDestroy(Material);

            MaskableGraphic = null;
            Material = null;
        }

        private void OnEnable()
        {
            Validate();
            Refresh();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (enabled && Material != null)
            {
                Refresh();
            }
        }

        private void Validate()
        {
            if (Material == null)
            {
                Material = new Material(Shader.Find(ShaderPath));
            }

            if (MaskableGraphic == null)
            {
                TryGetComponent(out MaskableGraphic);
            }

            MaskableGraphic.material = Material;

            if (MaskableGraphic is Image image && image.sprite != null)
            {
                OuterUV = UnityEngine.Sprites.DataUtility.GetOuterUV(image.sprite);
            }
        }

        protected virtual void Refresh()
        {
        }

        private void SafeDestroy(Object obj)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Object.Destroy(obj);
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
#else
			Object.Destroy(obj);
#endif
        }
    }
}