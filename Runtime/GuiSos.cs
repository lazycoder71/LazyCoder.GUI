using LazyCoder.Audio;
using LazyCoder.SO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazyCoder.Gui
{
    public class GuiSos : ScriptableObjectSingleton<GuiSos>
    {
        [Title("Button Lazy")]
        [SerializeField] private AudioConfig _btnLazySfxClickDefault;

        public static AudioConfig BtnLazySfxClickDefault => Instance._btnLazySfxClickDefault;
    }
}