using LazyCoder.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazyCoder.Gui
{
    public class GuiNavPageBackground : MonoBehaviour, IGuiNavPage
    {
        [Title("Config")]
        [SerializeField] private bool _closeOnClick = false;

        private GuiNavPage _page;

        private void Button_OnClick()
        {
            if (!_closeOnClick)
                return;

            _page.Close();
        }

        void IGuiNavPage.OnConstruct(GuiNavPage page, GuiNavContext context)
        {
            _page = page;

            GetComponent<Button>().onClick.AddListener(Button_OnClick);
        }

        void IGuiNavPage.OnStateChanged(GuiNavPage.State currentState)
        {
        }

        public static void Spawn(GuiNavPage page)
        {
            if (page.GetComponentInChildren<GuiNavPageBackground>() != null)
                return;

            // Spawn background object
            GameObject objBg = new GameObject("Background",
                typeof(CanvasGroup));

            // Setup rect transform component
            RectTransform rect = objBg.AddComponent<RectTransform>();

            rect.SetParent(page.TransformCached);
            rect.SetSiblingIndex(page.transform.GetSiblingIndex());
            rect.SetScale(1.0f);
            rect.Fill();

            // Setup image component
            Image image = objBg.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.8f);

            // Setup button component
            Button button = objBg.AddComponent<Button>();
            button.transition = Selectable.Transition.None;

            // Setup GuiNavPageBackground component
            objBg.AddComponent<GuiNavPageBackground>();
        }
    }
}