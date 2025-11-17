using LazyCoder.Core;
using UnityEngine;
using UnityEngine.UI;

namespace LazyCoder.Gui
{
    [RequireComponent(typeof(Button))]
    public class GuiButton : MonoBase
    {
        private Button _button;

        public Button Button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();

                return _button;
            }
        }

        protected virtual void Awake()
        {
            Button.onClick.AddListener(Button_OnClick);
        }

        public virtual void Button_OnClick()
        {
        }
    }
}