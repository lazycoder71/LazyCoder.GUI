using LazyCoder.SO;
using Sirenix.OdinInspector;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LazyCoder.Gui
{
    public class GuiNavSos : ScriptableObjectSingleton<GuiNavSos>
    {
#if ENABLE_INPUT_SYSTEM
        [Title("Input")]
        [SerializeField] private InputActionReference _inputActionBack;
        
        public static InputActionReference InputActionBack => Instance._inputActionBack;
#endif
    }
}