using LazyCoder.Core;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LazyCoder.Gui
{
    [RequireComponent(typeof(GuiNavPage))]
    public class GuiNavPagePressBackHandler : MonoBase
    {
        private GuiNavPage _page;

        private void Awake()
        {
            _page = GetComponent<GuiNavPage>();
        }

#if ENABLE_INPUT_SYSTEM
        protected override void OnEnable()
        {
            base.OnEnable();

            if (GuiNavSos.InputActionBack != null)
            {
                GuiNavSos.InputActionBack.action.performed += InputAction_BackPerformed;
            }
            else
            {
                LDebug.LogWarning<GuiNavPagePressBackHandler>(
                    "InputActionBack is null, unable to detect when back button is pressed.");
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (GuiNavSos.InputActionBack != null)
                GuiNavSos.InputActionBack.action.performed -= InputAction_BackPerformed;
        }

        private void InputAction_BackPerformed(InputAction.CallbackContext context)
        {
            HandleBackButton();
        }
#else
        protected override void Tick()
        {
            base.Tick();
            
            if (Input.GetKeyDown(KeyCode.Escape))
                HandleBackButton();
        }
#endif

        private void HandleBackButton()
        {
            _page.Close();
        }
    }
}